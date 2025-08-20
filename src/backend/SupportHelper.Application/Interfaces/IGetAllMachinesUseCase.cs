using SupportHelper.Application.DTOs;
using SupportHelper.Communication.Dtos.CouchDbDto;

namespace SupportHelper.Application.Interfaces
{
    public interface IGetAllMachinesUseCase
    {
        Task<ResponsePageableDto<IEnumerable<MachineDto>>> ExecuteAsync(int pageNumber, int pageSize);
    }
}
