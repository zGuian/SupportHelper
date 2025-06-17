using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.Models
{
    public sealed class MachineModel
    {
        public string Hostname { get; private set; } = string.Empty;
        public string CurrentUsername { get; private set; } = string.Empty;
        public string DomainName { get; private set; } = string.Empty;
        public string OperationalSystem { get; private set; } = string.Empty;
        public NetworkBoard[] NetworkBoards { get; private set; } = [];

        private MachineModel()
        {
            GetAllInformationFromMachine();
        }

        public static MachineModel Create()
        {
            return new MachineModel();
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
