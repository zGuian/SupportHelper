using SupportHelper.Application.DTOs;
using SupportHelper.Application.Interfaces;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Database;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public sealed class RequestGetAllMachinesUseCase : IRequestGetAllMachinesUseCase
    {
        private readonly IMachineRepository _machineRepository;

        public RequestGetAllMachinesUseCase(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<ResponsePageableDto<HashSet<Machine>>> ExecuteAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
