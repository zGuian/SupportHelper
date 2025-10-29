namespace SupportHelper.API.Domain.Interfaces.Repositories.Commons
{
    public interface IBaseRepositoryCommand<TEntity, UId>
    {
        Task RegisterAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        Task DeleteAsync(UId id, CancellationToken cancellationToken = default);

    }
}
