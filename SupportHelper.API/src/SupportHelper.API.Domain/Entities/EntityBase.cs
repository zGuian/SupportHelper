using System.Text;

namespace SupportHelper.API.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }

        protected EntityBase()
        {
            Id = -1;
        }

        protected EntityBase(int id)
        {
            Id = id;
        }

        public static string GenerateId()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .TrimEnd('=');
        }
    }
}
