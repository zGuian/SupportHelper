using SupportHelper.Application.DTOs;
using SupportHelper.Communication.Dtos.CouchDbDto;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestGetAllMachinesUseCase
    {
        Task<ResponsePageableDto<HashSet<RowDto>>> ExecuteAsync(int pageNumber, int pageSize);
    }
}
