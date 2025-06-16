namespace SupportHelper.Infrastructure.Data.DbModels
{
    public class MachineModel
    {

        public string Id { get; private set; } = string.Empty;
        public string Hostname { get; private set; } = string.Empty;
        public string CurrentUsername { get; private set; } = string.Empty;
        public string DomainName { get; private set; } = string.Empty;
        public string OperationalSystem { get; private set; } = string.Empty;
    }
}
