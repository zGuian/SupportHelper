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

        public async Task<(HashSet<Machine>?, int total)> GetAllMachinesAsync(int pageNumber, int pageSize)
        {
            try
            {
                const string nameStoredProcedure = @"sp_GetPagedData";

                DynamicParameters parameters = new();
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                IEnumerable<Machine> data = await _dbConnection.QueryAsync<Machine>(nameStoredProcedure,
                    parameters, commandType: CommandType.StoredProcedure);

                int totalRecords = parameters.Get<int>("@TotalRecords");

                return (data.ToHashSet(), totalRecords);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ocorreu um problema ao buscar valores paginados.");
                return (null, 0);
            }
        }

        public async Task<Machine?> GetMachineAsync(string id)
        {
            try
            {
                const string sql = @"";

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
            catch (Exception)
            {
                throw;
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
