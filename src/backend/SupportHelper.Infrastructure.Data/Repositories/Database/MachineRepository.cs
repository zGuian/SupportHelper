using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using System.Data;

namespace SupportHelper.Infrastructure.Data.Repositories.Database
{
    public class MachineRepository : IMachineRepository
    {
        private readonly ILogger<MachineRepository> _logger;
        private readonly IDbConnection _dbConnection;

        public MachineRepository(ILogger<MachineRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<(HashSet<Machine>?, int)> GetAllMachinesAsync(int pageNumber, int pageSize)
        {
            try
            {
                const string sql = @"
                SELECT 
                    id,
                    hostname,
                    current_username,
                    domain_name,
                    operational_system
                FROM machine
                OFFSET (@page_number - 1) * @page_size
                LIMIT @page_size;

                SELECT COUNT(*) FROM machine;
                ";

                DynamicParameters parameters = new();
                parameters.Add("@page_size", pageSize);
                parameters.Add("@page_number", pageNumber);

                var query = await _dbConnection.QueryMultipleAsync(sql, parameters, commandType: CommandType.Text,
                    commandTimeout: TimeSpan.FromSeconds(30).Seconds);

                var machines = (await query.ReadAsync<Machine>()).ToHashSet();
                var totalRecord = await query.ReadFirstAsync<int>();

                return (machines, totalRecord);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ocorreu um problema ao buscar valores paginados: {message}", ex.Message);
                return (null, 0);
            }
        }

        public async Task<Machine?> GetMachineByIdAsync(string id)
        {
            try
            {
                const string sql = @"
                    SELECT * 
                    FROM machine m
                    WHERE m.id = @id";

                Machine machine = await _dbConnection.QueryFirstAsync<Machine>(sql, id,
                    commandTimeout: TimeSpan.FromSeconds(30).Seconds) ?? throw new Exception("NOT FOUND MACHINE");

                return machine;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Não encontrado nenhuma maquina com esse ID [{id}]", id);
                _logger.LogWarning("Erro: {message}", ex.Message);
                return null;
            }
        }

        public async Task UpdateMachineAsync(Machine machine)
        {
            try
            {
                const string sql = @"
                UPDATE 
                    machine
                SET 
                    hostname = @hostname,
                    CurrentUsername = @currentUsername,
                    DomainName = @domainName,
                    OperationalSystem = @operationalSystem,
                WHERE 
                    id = @id";

                await _dbConnection.ExecuteAsync(sql, new
                {
                    @hostname = machine.Hostname,
                    @currentUsername = machine.CurrentUsername,
                    @domainName = machine.DomainName,
                    @operationalSystem = machine.OperationalSystem,
                    @id = machine.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ocorreu um erro ao realizar atualização: {message}", ex.Message);
            }
        }

        public async Task InsertMachineByProcedure(Machine machine)
        {
            try
            {
                const string function = "SELECT sp_ValidateAndUpdateMachine(@m_id, @m_hostname, @m_currentUsername, @m_domainName, @m_operationalSystem, @m_newId)";
                int line = await _dbConnection.ExecuteAsync(function, new
                {
                    m_id = machine.Id,
                    m_hostname = machine.Hostname,
                    m_currentUsername = machine.CurrentUsername,
                    m_domainName = machine.DomainName,
                    m_operationalSystem = machine.OperationalSystem,
                    m_newId = EntityBase.GenerateId()
                }, commandType: CommandType.Text);
                _logger.LogInformation("Adicionado com sucesso via Function");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao inserir valor no banco de dados. [{}]", ex.Message);
            }
        }
    }
}
