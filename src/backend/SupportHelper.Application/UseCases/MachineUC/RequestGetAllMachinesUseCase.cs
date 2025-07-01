using SupportHelper.Application.DTOs;
using SupportHelper.Application.Interfaces;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public sealed class RequestGetAllMachinesUseCase : IRequestGetAllMachinesUseCase
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineMemoryRepository _machineMemoryRepository;

        public RequestGetAllMachinesUseCase(IMachineRepository machineRepository, IMachineMemoryRepository machineMemoryRepository)
        {
            _machineRepository = machineRepository;
            _machineMemoryRepository = machineMemoryRepository;
        }

        public async Task<ResponsePageableDto<HashSet<Machine>>> ExecuteAsync(int pageNumber, int pageSize)
        {
            (HashSet<Machine>? datas, int total) = await _machineRepository.GetAllMachinesAsync(pageNumber, pageSize);
            if (datas == null)
            {
                throw new ArgumentNullException(nameof(datas));
            }

            return new ResponsePageableDto<HashSet<Machine>>
            {
                CurrentPage = pageNumber,
                PageCount = pageSize,
                TotalQuantity = total,
                Datas = datas
            };
        }
    }
}
