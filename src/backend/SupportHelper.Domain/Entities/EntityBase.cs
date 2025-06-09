using System.Text;

namespace SupportHelper.Domain.Entities
{
    public abstract class EntityBase
    {
        public string Id { get; protected set; }

        public static string GenerateId()
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
