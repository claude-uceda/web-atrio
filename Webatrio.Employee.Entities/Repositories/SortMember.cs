
using System.Linq.Expressions;
using Webatrio.Employee.Core.Entities;

namespace Webatrio.Employee.Core.Repositories
{
    public class SortMember<TEntity, T> where TEntity : IEntity
    {
        public Expression<Func<TEntity, T>> MemberExpression { get; set; }

        public SortOrder Order { get; set; }
    }
}