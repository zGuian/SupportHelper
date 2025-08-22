using CouchDB.Driver.Extensions;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Models;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Exceptions.ExceptionsBase;
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

        public async Task<IEnumerable<IMachineModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var machineQuery = _context.Machines.Where(m => m.SignalR.IsActive);
            var machines = await machineQuery.ToListAsync(cancellationToken);
            return machines;
        }

        public async Task<IMachineModel> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var model = await _context.Machines.FirstOrDefaultAsync(m => m.Machine.Hostname == hostname, cancellationToken) 
                ?? throw new NotFoundException([$"NÃO FOI ENCONTRADO: {hostname} NO BANCO DE DADOS"]);
            return model;
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var model = await GetByHostnameAsync(hostname, cancellationToken).ConfigureAwait(false);
            return model.SignalR.ConnectionId;
        }

        public int GetQuantityMachines() => _context.Machines.Count();

        public async Task InsertOrUpdateAsync(MachineAggregates aggregate, CancellationToken cancellationToken = default)
        {
            var model = await GetByHostnameAsync(aggregate.Machine.Hostname, cancellationToken);
            model.Update(aggregate.Machine, aggregate.SignalR);
            if(model is MachineModel concreteModel)
            {
                await _context.Machines.AddOrUpdateAsync(concreteModel, cancellationToken: cancellationToken);
                return;
            }
            throw new InvalidOperationException("HOUVE UM PROBLEMA NO CAST DA INTERFACE MODEL");
        }

        public async Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default)
        {
            var model = await GetByHostnameAsync(hostname, cancellationToken);
            if (model == null)
            {
                model = MachineModel.Factories.CreateNullMachine(hostname, connId);
                await _context.Machines.AddOrUpdateAsync((MachineModel)model, cancellationToken: cancellationToken);
                return;
            }
            model.UpdateSignalR(connId);
            await _context.Machines.AddOrUpdateAsync((MachineModel)model, cancellationToken: cancellationToken);
        }
    }
}
