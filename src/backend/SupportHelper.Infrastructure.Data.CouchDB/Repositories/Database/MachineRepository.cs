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

        public async Task<IEnumerable<MachineAggregates>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var machineQuery = _context.Machines.Where(m => m.SignalR.IsActive);
            var machineList = await machineQuery.ToListAsync(cancellationToken);
            var machines = new HashSet<MachineAggregates>();
            foreach (var item in machineList)
            {
                machines.Add(MachineAggregates.Converters.ToAggregate(item.Machine, item.SignalR));
            }
            return machines;
        }

        public async Task<MachineAggregates> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var model = await _context.Machines.FindAsync(hostname, cancellationToken: cancellationToken);
            var aggregate = MachineAggregates.Converters.ToAggregate(model.Machine, model.SignalR);
            return aggregate;
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var machineAggregate = await GetByHostnameAsync(hostname, cancellationToken);
            return machineAggregate.SignalR.ConnectionId;
        }

        public int GetQuantityMachines() => _context.Machines.Count();

        public async Task InsertOrUpdateAsync(MachineAggregates aggregate, CancellationToken cancellationToken = default)
        {
            var model = await _context.Machines.Where(m => m.Machine.Hostname == aggregate.Machine.Hostname)
                .FirstAsync(cancellationToken);
            model.Update(aggregate.Machine, aggregate.SignalR);
            await _context.Machines.AddOrUpdateAsync(model, cancellationToken: cancellationToken);
        }

        public async Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default)
        {
            var model = await _context.Machines.FindAsync(hostname, cancellationToken: cancellationToken);
            if (model == null)
            {
                model = MachineModel.Factories.CreateNullMachine(hostname, connId);
                await _context.Machines.AddOrUpdateAsync(model, cancellationToken: cancellationToken);
                return;
            }
            model.UpdateSignalR(connId);
            await _context.Machines.AddOrUpdateAsync(model, cancellationToken: cancellationToken);
        }
    }
}
