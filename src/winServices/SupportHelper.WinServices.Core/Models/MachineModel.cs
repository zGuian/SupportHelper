using Microsoft.Win32;
using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.Models
{
    public sealed class MachineModel
    {
        public string Id { get; private set; } = string.Empty;
        public string Hostname { get; private set; } = string.Empty;
        public string CurrentUsername { get; private set; } = string.Empty;
        public string DomainName { get; private set; } = string.Empty;
        public string OperationalSystem { get; private set; } = string.Empty;
        public IEnumerable<NetworkBoard> NetworkBoards { get; private set; } = [];
        public string UpTime { get; set; } = string.Empty;
        public string LastUpdate { get; set; } = string.Empty;


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
            Id = GetOrCreateId();
            Hostname = Environment.MachineName;
            DomainName = Environment.UserDomainName;
            CurrentUsername = Environment.UserName;
            OperationalSystem = Environment.OSVersion.VersionString;
            NetworkBoards = NetworkBoard.GetAllInformation();
            UpTime = GetUpTime();
            LastUpdate = DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
        }

        private static string GetUpTime()
        {
            var upTime = Environment.TickCount64;
            return TimeSpan.FromMilliseconds(upTime).ToString();
        }

        private static string GetOrCreateId()
        {
            try
            {
                const string RegistryKeyPath = @"SOFTWARE\SupportHelper";
                const string RegistryValueName = "MachineId";
                using (var key = Registry.LocalMachine.CreateSubKey(RegistryKeyPath))
                {
                    if (key == null)
                    {
                        throw new Exception("Falha ao abrir ou criar a chave de registro.");
                    }
                    object? existingValue = key.GetValue(RegistryValueName);
                    if (existingValue is string existingId && Guid.TryParse(existingId, out _))
                    {
                        return existingId;
                    }

                    string newId = Guid.NewGuid().ToString();
                    key.SetValue(RegistryValueName, newId, RegistryValueKind.String);
                    return newId;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao acessar ou gravar o MachineId no Registro.", ex);
            }
        }
    }
}
