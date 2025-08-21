using SupportHelper.Communication.Dtos.CouchDbDto;

namespace SupportHelper.Application.Interfaces
{
    public interface IGetAllMachinesActivesUseCase
    {
        Task<IEnumerable<MachineDto>> ExecuteAsync(CancellationToken ct = default);
    }
}
