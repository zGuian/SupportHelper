namespace SupportHelper.Service.Domain.Events
{
    public delegate void MachineShutdownEventHandler();
    public class MachineEvents
    {
        public event MachineShutdownEventHandler? OnShutdown;

        public void MachineShutdown()
        {
            OnShutdown?.Invoke();
        }
    }
}
