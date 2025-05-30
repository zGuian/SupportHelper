using SupportHelper.FrontEnd.MVC.ValueObjects;

namespace SupportHelper.FrontEnd.MVC.Models
{
    public class MachineModel
    {
        public string Id { get; private set; }
        public string Hostname { get; private set; }
        public string CurrentUsername { get; private set; }
        public string DomainName { get; private set; }
        public string OperationalSystem { get; private set; }
        public ICollection<NetworkBoardVO> NetworkBoards { get; private set; }

        public MachineModel()
        {
            Id = string.Empty;
            Hostname = string.Empty;
            DomainName = string.Empty;
            CurrentUsername = string.Empty;
            OperationalSystem = string.Empty;
            NetworkBoards = [];
        }

        public MachineModel(string id, string hostname, string currentUsername, string domainName, string operationalSystem)
        {
            Id = id;
            Hostname = hostname;
            CurrentUsername = currentUsername;
            DomainName = domainName;
            OperationalSystem = operationalSystem;
            NetworkBoards = [];
        }
    }
}
