namespace SupportHelper.API.Domain.Interfaces.Repositories.Commons
{
    public interface IBaseRepositoryQuery<TEntity,UId>
    {
        IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(UId id, CancellationToken cancellationToken = default);
    }
}
