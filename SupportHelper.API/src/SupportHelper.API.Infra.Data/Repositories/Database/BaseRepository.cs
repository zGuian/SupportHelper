using Microsoft.EntityFrameworkCore;
using SupportHelper.API.Domain.Interfaces.Repositories.Commons;
using SupportHelper.API.Infra.Data.Context;
using System.Runtime.CompilerServices;

namespace SupportHelper.API.Infra.Data.Repositories.Database
{
    public abstract class BaseRepository<TEntity, UId>(AppDbContext context) : IBaseRepositoryQuery<TEntity, UId>,
        IBaseRepositoryCommand<TEntity, UId> where TEntity : class
    {
        private readonly DbSet<TEntity> _context = context.Set<TEntity>();

        public virtual async IAsyncEnumerable<TEntity> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var query = _context.AsNoTracking().AsAsyncEnumerable().WithCancellation(cancellationToken);
            await foreach (var u in query)
            {
                yield return u;
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(UId id, CancellationToken cancellationToken = default)
            => await _context.FindAsync([id], cancellationToken)
                ?? throw new Exception("NÃO ENCONTRADO NENHUM VALOR NO BANCO DE DADOS");

        public virtual async Task RegisterAsync(TEntity entity, CancellationToken cancellationToken = default)
            => await _context.AddAsync(entity, cancellationToken);

        public virtual void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public virtual async Task DeleteAsync(UId id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            _context.Remove(entity);
        }
    }
}
