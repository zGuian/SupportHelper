using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Infrastructure.Data.Interfaces;

namespace SupportHelper.Infrastructure.Data.Services
{
    public class MachineServices : IMachineServices
    {
        private readonly IMachineRepository _repository;
        private readonly IMachineMemoryRepository _machineMemory;

        public MachineServices(IMachineRepository repository, IMachineMemoryRepository machineMemory)
        {
            _repository = repository;
            _machineMemory = machineMemory;
        }

        public async Task SaveInDatabaseAndInMemory(MachineInformationResponse response)
        {
            var machine = Machine.Create(response);
            _machineMemory.Register(machine.Id, machine);
            await _repository.InsertMachineByProcedure(machine);
        }
    }
}
