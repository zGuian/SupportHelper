
namespace SupportHelper.Application.Interfaces
{
    public interface IRequestStatusMachineUseCase
    {
        Task ExecuteAsync(string equipmentId);
    }
}
