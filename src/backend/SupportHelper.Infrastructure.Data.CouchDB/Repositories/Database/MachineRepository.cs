using CouchDB.Driver.Extensions;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Infrastructure.Data.CouchDB.Context;
using SupportHelper.Infrastructure.Data.CouchDB.Models;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Database
{
    public class MachineRepository : IMachineRepository
    {
        private readonly AppCouchContext _context;

        public MachineRepository(AppCouchContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MachineAggregates>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var machineQuery = _context.Machines.Where(m => m.SignalR.IsActive).Skip(pageNumber).Take(pageSize);
            var machineList = await machineQuery.ToListAsync();
            var machines = new HashSet<MachineAggregates>();
            foreach (var item in machineList)
            {
                machines.Add(MachineAggregates.Converters.ToAggregate(item.Machine, item.SignalR));
            }
            return machines;
        }

        public async Task<MachineAggregates> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var model = await _context.Machines.FirstOrDefaultAsync(m => m.Machine.Hostname == hostname, cancellationToken);
            return MachineAggregates.Create(model.Machine, model.SignalR);
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var machineAggregate = await GetByHostnameAsync(hostname, cancellationToken);
            return machineAggregate.SignalR.ConnectionId;
        }

        public int GetQuantityMachines() => _context.Machines.Count();

        public async Task InsertOrUpdateAsync(MachineAggregates aggregate, CancellationToken cancellationToken = default)
        {
            var model = MachineModel.Converters.ToModel(aggregate.Machine, aggregate.SignalR);
            await _context.Machines.AddOrUpdateAsync(model, cancellationToken: cancellationToken);
        }

        public async Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default)
        {
            var model = await _context.Machines.FindAsync(hostname, cancellationToken: cancellationToken);
            if (model == null)
            {
                await _context.Machines.AddAsync(MachineModel.Factories.CreateNullMachine(hostname, connId), cancellationToken: cancellationToken);
                return;
            }
            model.UpdateSignalR(connId);
            await _context.Machines.AddOrUpdateAsync(model, cancellationToken: cancellationToken);
        }
    }
}
