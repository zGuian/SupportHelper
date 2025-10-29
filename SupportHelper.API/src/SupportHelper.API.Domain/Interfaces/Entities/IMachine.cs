using SupportHelper.API.Domain.Entities;

namespace SupportHelper.API.Domain.Interfaces.Entities
{
    public interface IMachine
    {
        string Hostname { get; }
        string CurrentUsername { get; }
        string DomainName { get; }
        string OperationalSystem { get; }
        bool SgpIsRunning { get; }
        IEnumerable<NetworkBoard> NetworkBoards { get; }
        string UpTime { get; }
        string LastUpdate { get; }
    }
}
