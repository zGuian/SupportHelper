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

        public async Task<ResponsePageableDto<IEnumerable<MachineDto>>> ExecuteAsync(int pageNumber, int pageSize)
        {
            var machines = await _machineRepository.GetAllAsync(pageNumber, pageSize);
            var count = _machineRepository.GetQuantityMachines();
            var dto = new List<MachineDto>();
            foreach (var item in machines)
            {
                dto.Add(new MachineDto
                {
                    Id = item.Machine.Id,
                    Hostname = item.Machine.Hostname,
                    CurrentUsername = item.Machine.CurrentUsername,
                    DomainName = item.Machine.DomainName,
                    OperationalSystem = item.Machine.OperationalSystem,
                    NetworkBoard = item.Machine.NetworkBoards.Select(x => 
                        new NetworkBoardDto { Description = x.Description,
                                              Ipv4 = x.Ipv4,
                                              Ipv6 = x.Ipv6,
                                              MacAddress = x.MacAddress,
                                              InUse = x.InUse }),
                    UpTime = item.Machine.UpTime,
                    LastUpdate = item.Machine.LastUpdate
                });
            }
            return new ResponsePageableDto<IEnumerable<MachineDto>>
            {
                TotalQuantity = count,
                CurrentPage = pageNumber,
                PageCount = pageNumber,
                Datas = dto
            };
        }
    }
}
