using SupportHelper.Service.Domain.DTOs.Responses;

namespace SupportHelper.Service.Domain.Interface.UseCases
{
    public interface IGetStatusMachineUseCase
    {
        ResponseStatusMachineJson Execute();
    }
}
