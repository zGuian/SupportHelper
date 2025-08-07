using System.Text;

namespace SupportHelper.Domain.Entities
{
    public abstract class EntityBase
    {
        public string Id { get; protected set; } = string.Empty;
        public bool IsConnected { get; protected set; }

        public static string GenerateId()
        {
            var guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            int integerValue = BitConverter.ToInt32(bytes, 0);
            var split = guid.ToString().Split('-');
            var guidString = string.Join("", split);
            var sb = new StringBuilder();
            sb.Append(guidString);
            sb.Append(integerValue);
            return sb.ToString();
        }
    }
}
