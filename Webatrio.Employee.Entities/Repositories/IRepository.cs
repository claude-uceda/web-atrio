using System.Linq.Expressions;
using Webatrio.Employee.Core.Entities;

namespace Webatrio.Employee.Core.Repositories
{
    public interface IRepository { }

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IEntity
    {
        Task<Guid> InsertAsync(TEntity entity);

        Task InsertBatchAsync(IEnumerable<TEntity> listToInsert);

        Task<long> DeleteAsync(Expression<Func<TEntity, bool>> memberExpression);    

        Task OverwriteAsync(TEntity item);
    }
}