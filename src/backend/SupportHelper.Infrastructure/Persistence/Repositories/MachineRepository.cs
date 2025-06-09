using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories;
using System.Data;

namespace SupportHelper.Infrastructure.Persistence.Repositories
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

        public async Task<HashSet<Machine>> GetAllMachinesAsync(int pageSize, int count)
        {
            const string sql = @"";

            var machine = await _dbConnection.QueryAsync<Machine>(sql, new
            {
                pageSize,
                count
            });

            return [.. machine];
        }

        public async Task<Machine?> GetMachineAsync(string id)
        {
            try
            {
                const string sql = @"";

                var machine = await _dbConnection.QueryFirstAsync<Machine>(sql, id, 
                    commandTimeout: TimeSpan.FromSeconds(30).Seconds) ?? throw new Exception("NOT FOUND MACHINE");
                return machine;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Não encontrado nenhuma maquina com esse ID [{}]", id);
                _logger.LogWarning("Erro: {}", ex.Message);
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InsertMachineByProcedure(Machine machine)
        {
            try
            {
                const string procedure = "sp_ValidateAndUpdateMachine";
                var line = await _dbConnection.ExecuteAsync(procedure, new
                {
                    m_id = machine.Id,
                    m_hostname = machine.Hostname,
                    m_currentUsername = machine.CurrentUsername,
                    m_domainName = machine.DomainName,
                    m_operationalSystem = machine.OperationalSystem,
                    m_newId = Machine.GenerateId()
                }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("Adicionado com sucesso via STORED PROCEDURE");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao inserir valor no banco de dados. [{}]", ex.Message);
            }
        }
    }
}
