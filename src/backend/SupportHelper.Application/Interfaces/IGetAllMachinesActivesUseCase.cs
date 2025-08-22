using SupportHelper.Application.DTOs;

namespace SupportHelper.Application.Interfaces
{
    public interface IGetAllMachinesActivesUseCase
    {
        Task<IEnumerable<MachineDto>> ExecuteAsync(CancellationToken ct = default);
    }
}
