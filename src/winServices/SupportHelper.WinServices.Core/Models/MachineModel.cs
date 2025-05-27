using SupportHelper.WinServices.Core.ValueObjects;

namespace SupportHelper.WinServices.Core.Models
{
    public sealed class MachineModel
    {
        public string Hostname { get; private set; }
        public string CurrentUsername { get; private set; }
        public string DomainName { get; private set; }
        public string OperationalSystem { get; private set; }
        public NetworkBoard[] NetworkBoards { get; private set; }

        public MachineModel()
        {
            Hostname = string.Empty;
            DomainName = string.Empty;
            CurrentUsername = string.Empty;
            OperationalSystem = string.Empty;
            NetworkBoards = [];
        }

        public void GetAllInformationFromMachine()
        {
            Hostname = Environment.MachineName;
            DomainName = Environment.UserDomainName;
            CurrentUsername = Environment.UserName;
            OperationalSystem = Environment.OSVersion.VersionString;
            NetworkBoards = NetworkBoard.GetAllInformation();
        }
    }
}
