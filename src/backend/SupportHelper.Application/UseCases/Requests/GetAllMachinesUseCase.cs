using SupportHelper.Application.DTOs;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Interfaces.Repositories.Database;

namespace SupportHelper.Application.UseCases.Requests
{
    public sealed class GetAllMachinesUseCase : IGetAllMachinesUseCase
    {
        private readonly IMachineRepository _machineRepository;

        public GetAllMachinesUseCase(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<ResponsePageableDto<HashSet<RowDto>>> ExecuteAsync(int pageNumber, int pageSize)
        {
            (AllDocsDto alldocs, int count) = await _machineRepository.GetAllAsync(pageNumber, pageSize);
            var hs = new HashSet<RowDto>();
            foreach (var row in alldocs.Rows)
            {
                hs.Add(row);
            }

            return new ResponsePageableDto<HashSet<RowDto>>
            {
                TotalQuantity = count,
                CurrentPage = pageNumber,
                PageCount = pageNumber,
                Datas = hs
            };
        }
    }
}
