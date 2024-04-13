
using System.Linq.Expressions;
using Webatrio.Employee.Core.Entities;

namespace Webatrio.Employee.Core.Repositories
{
    public interface IReadOnlyRepository<TEntity> : IRepository where TEntity : IEntity
    {
        Task<TEntity?> GetOneAsync(CancellationToken cancellationToken);

        Task<TEntity?> GetOneAsync(Guid id, CancellationToken cancellationToken);

        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> memberExpression, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> memberExpression, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

        Task<long> CountAsync(CancellationToken cancellationToken);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> memberExpression, CancellationToken cancellationToken);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> memberExpression, int? limit, CancellationToken cancellationToken);

        Task AfterGetAsync(TEntity entity, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query, int limit, int skip, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query, int limit, int skip, SortMember<TEntity, object> sorting, CancellationToken cancellationToken);        

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query, int limit, int skip, SortMember<TEntity, object>[] sorting, CancellationToken cancellationToken);

    }
}