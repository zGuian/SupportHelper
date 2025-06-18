namespace SupportHelper.Domain.Interfaces.SignalRContext
{
    public interface IMachineSignalRServices
    {
        Task RequestStatusAsync(string equipmentId);
    }
}
