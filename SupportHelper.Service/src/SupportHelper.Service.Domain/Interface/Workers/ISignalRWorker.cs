namespace SupportHelper.Service.Domain.Interface.Workers
{
    public interface ISignalRWorker
    {
        event System.EventHandler? OnMachineShutdown;
    }
}
