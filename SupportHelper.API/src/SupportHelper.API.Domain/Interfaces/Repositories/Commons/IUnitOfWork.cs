namespace SupportHelper.API.Domain.Interfaces.Repositories.Commons
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
    }
}
