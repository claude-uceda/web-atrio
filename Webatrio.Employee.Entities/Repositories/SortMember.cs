
using System.Linq.Expressions;

namespace Webatrio.Employee.Entities.Repositories
{
    public class SortMember<TEntity, T> where TEntity : IEntity
    {
        public Expression<Func<TEntity, T>> MemberExpression { get; set; }

        public SortOrder Order { get; set; }
    }
}