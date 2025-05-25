using SupportHelper.Domain.ValueObjects;
using System.Text;

namespace SupportHelper.Domain.Entities
{
    public class Machine
    {
        public string Id { get; private set; }
        public string Hostname { get; private set; }
        public NetworkBoard[]? NetworkBoard { get; private set; }

        public Machine(string hostname)
        {
            Id = GenerateId();
            Hostname = hostname;
        }
        
        private static string GenerateId()
        {
            string guid = Guid.NewGuid().ToString();
            string[] split = guid.Split('-');
            string id = string.Join("", split);
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var sb = new StringBuilder();
            sb.Append(id);
            sb.Append(date);
            return sb.ToString();
        }
    }
}
