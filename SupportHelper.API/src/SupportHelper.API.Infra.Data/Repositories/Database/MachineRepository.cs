using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SupportHelper.API.Domain.DTOs.Requests;
using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Entities.ValueObjects;
using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Infra.Data.Context;
using System.Data;
using System.Threading.Tasks;

namespace SupportHelper.API.Infra.Data.Repositories.Database
{
    public class MachineRepository(AppDbContext context
        , IConfiguration configuration)
        : BaseRepository<Machine, string>(context), IMachineRepositoryQuery, IMachineRepositoryCommand
    {
        private readonly AppDbContext _context = context;
        private readonly string _connectionString = configuration.GetConnectionString("SQLServer")
                ?? throw new ArgumentNullException("ConnectionString:SQLServer");

        public async Task<bool> ExistHostname(string hostname)
            => await _context.Machines.AnyAsync(h => h.Hostname.Equals(hostname));

        public async Task<int> GetIdByHostnameAsync(string hostname)
        {
            return await _context.Machines.AsNoTracking()
                .Where(m => m.Hostname.Equals(hostname))
                .Select(m => m.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Machine> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var machine = await _context.Machines
                .AsNoTracking()
                .Include(x => x.NetworkBoards)
                .FirstOrDefaultAsync(x => x.Hostname.Equals(hostname), cancellationToken);
            return machine ?? throw new NotImplementedException();
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var sql = @"SELECT COL_SIGNALR_CONNECTIONID
                        FROM TB_MACHINE 
                        WHERE COL_HOSTNAME = @hostname";
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                var connectionid = await conn.QuerySingleAsync<string>(sql, new { hostname }, commandTimeout: 10);
                return connectionid;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<Dictionary<RequestUpdateSgpClientJson, string>> GetManyConnectionAsync(
            IEnumerable<RequestUpdateSgpClientJson> requests, CancellationToken ct = default)
        {
            try
            {
                var hosts = requests.Select(x => x.Hostname);
                var query = @"SELECT COL_HOSTNAME, COL_SIGNALR_CONNECTIONID
                          FROM TB_MACHINE
                          WHERE COL_HOSTNAME IN @parameters";

                await using var conn = new SqlConnection(_connectionString);
                var result = await conn.QueryAsync(query);

                var dict = result.ToDictionary(r => (string)r.COL_HOSTNAME, r => (string)r.COL_SIGNALR_CONNECTIONID);
                var response = new Dictionary<RequestUpdateSgpClientJson, string>();
                foreach (var request in requests)
                {
                    if (dict.TryGetValue(request.Hostname, out var value))
                    {
                        response.Add(request, value);
                    }
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<(IEnumerable<Machine> models, int count)> GetPageAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Machine entity)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                var sqlMachine = @"UPDATE TB_MACHINE
                               SET COL_ISCONNECTED = @IsConnected
                               	, COL_SGPISRUNNING = @SgpIsRunning
                               	, COL_UPTIME = @UpTime
                                , COL_CURRENTUSERNAME = @CurrentUsername
                               	, COL_SIGNALR_CONNECTIONID = @SignalR_ConnectionId
                               	, COL_SIGNALR_ISACTIVE = @SignalR_IsActive
                               	, COL_LASTUPDATE = @LastUpdate
                               WHERE COL_HOSTNAME = @Hostname";

                var machineParameters = new
                {
                    entity.Hostname,
                    entity.IsConnected,
                    entity.SgpIsRunning,
                    entity.UpTime,
                    entity.CurrentUsername,
                    SignalR_ConnectionId = entity.SignalR.ConnectionId,
                    SignalR_IsActive = entity.SignalR.IsActive,
                    entity.LastUpdate
                };
                await conn.ExecuteAsync(sqlMachine, machineParameters, transaction);

                var sqlNetwork = @"UPDATE TB_NETWORKBOARD
                                   SET COL_IPV4 = @Ipv4
                                   	, COL_INUSE = @InUse
                                   	, COL_IPV6 = @Ipv6
                                    , COL_DESCRIPTION = @Description
                                   WHERE COL_MACADRESS = @MacAddress";
                await conn.ExecuteAsync(sqlNetwork, entity.NetworkBoards, transaction);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateForShutdownAsync(string hostname)
        {
            var procedureName = @"sp_shutdown_client";
            var parameter = new { @hostname = hostname };
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.ExecuteAsync(procedureName, parameter, commandTimeout: 30, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
