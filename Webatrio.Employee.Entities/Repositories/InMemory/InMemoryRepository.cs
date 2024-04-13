using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Webatrio.Employee.Entities.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Webatrio.Employee.Entities.Repositories.InMemory
{
    public class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        #region Field(s)

        protected readonly List<TEntity> collection;
        protected readonly bool cloneReturn;

        public IReadOnlyCollection<TEntity> Collection
        {
            get { return collection.AsReadOnly(); }
        }

        #endregion

        #region Constructor(s)        

        public InMemoryRepository() : this(new List<TEntity>()) { }

        public InMemoryRepository(params TEntity[] entities) : this(entities.ToList()) { }

        public InMemoryRepository(List<TEntity> collection, bool cloneReturn = false)
        {
            this.collection = collection ?? new List<TEntity>();
            this.cloneReturn = cloneReturn;
        }

        #endregion

        public Task<TEntity?> GetOneAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromResult(default(TEntity));

            var item = collection.FirstOrDefault();
            if (item != null && cloneReturn)
                item = (TEntity)ReflectionUtil.Clone(item);

            return Task.FromResult(item);
        }

        public Task<TEntity?> GetOneAsync(Guid id, CancellationToken cancellationToken)
        {
            return GetOneAsync(x => x.Id == id, cancellationToken);
        }

        public Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> memberExpression, CancellationToken cancellationToken)
        {
            var item = collection.FirstOrDefault(memberExpression.Compile());
            if (item != null && cloneReturn)
                item = (TEntity)ReflectionUtil.Clone(item);

            return Task.FromResult(item);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> memberExpression, CancellationToken cancellationToken)
        {
            var entity = await GetOneAsync(memberExpression, cancellationToken);
            return entity != null;
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetOneAsync(x => x.Id == id, cancellationToken);

            return entity != null;
        }

        public async Task OverwriteAsync(TEntity item)
        {
            var entity = collection.Single(x => x.Id == item.Id);
            collection.Remove(entity);
            await InsertAsync(item);
        }

        public Task AfterGetAsync(TEntity entity, CancellationToken cancellationToken) { return Task.CompletedTask; }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? query = null)
        {
            if (query == null)
                return collection.AsReadOnly();
            return collection.Where(query.Compile());
        }

        public Task<Guid> InsertAsync(TEntity entity)
        {
            collection.Add(entity);
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            return Task.FromResult(entity.Id);
        }

        public async Task InsertBatchAsync(IEnumerable<TEntity> listToInsert)
        {
            foreach (TEntity item in listToInsert)
                await InsertAsync(item);
        }

        public Task<long> DeleteAsync(Expression<Func<TEntity, bool>> memberExpression)
        {
            var toDelete = collection.Where(memberExpression.Compile()).ToList();
            foreach (TEntity item in toDelete)
            {
                collection.Remove(item);
            }
            return Task.FromResult((long)toDelete.Count);
        }

        public async Task<long> UpdateAsync(TEntity entity)
        {
            await OverwriteAsync(entity);

            return 1;
        }

        public Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return CountAsync(null, null, cancellationToken);
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> memberExpression, CancellationToken cancellationToken)
        {
            return CountAsync(memberExpression, null, cancellationToken);
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>>? memberExpression, int? limit, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromResult(0L);

            if (memberExpression == null)
                return Task.FromResult((long)collection.Count);
            var count = collection.Count(memberExpression.Compile());
            // kind of dumb bit s to have same behaviour than mongo one
            return Task.FromResult((long)(limit.HasValue && count >= limit.Value ? limit.Value : count));
        }

        public virtual Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken)
        {
            return GetAsync(null, -1, -1, (SortMember<TEntity, object>[]?)null, cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query, CancellationToken cancellationToken)
        {
            return GetAsync(query, -1, -1, (SortMember<TEntity, object>[]?)null, cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? query, int limit, int skip, CancellationToken cancellationToken)
        {
            return GetAsync(query, limit, skip, (SortMember<TEntity, object>[]?)null, cancellationToken);
        }

        public virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? query, int limit, int skip, SortMember<TEntity, object> sorting, CancellationToken cancellationToken)
        {
            return GetAsync(query, limit, skip, new SortMember<TEntity, object>[] { sorting }, cancellationToken);
        }

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? query, int limit, int skip, SortMember<TEntity, object>[]? sorting, CancellationToken cancellationToken)
        {
            return Task.FromResult(Get(query, limit, skip, sorting));
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? query, int? limit = null, int? skip = null, SortMember<TEntity, object>[]? sorting = null)
        {
            Func<TEntity, bool> fquery = query == null ? x => true : query.Compile();
            var items = collection.Where(fquery).AsQueryable();
            if ((skip ?? -1) > -1)
                items = items.Skip(skip!.Value);
            if ((limit ?? -1) > -1)
                items = items.Take(limit!.Value);
            if (cloneReturn)
                items = items.Select(x => x == null ? default : (TEntity)ReflectionUtil.Clone(x));

            if (sorting != null && sorting.Length > 0)
                items = ToOrderedQueryable(items, sorting);

            return items;
        }

        #region Helper(s)

        protected IOrderedQueryable<TEntity>? ToOrderedQueryable(IQueryable<TEntity> items, SortMember<TEntity, object>[] sorting)
        {
            IOrderedQueryable<TEntity>? ordered = null;
            foreach (var item in sorting)
            {
                if (ordered == null)
                {
                    ordered = item.Order == SortOrder.Descending ? items.OrderByDescending(item.MemberExpression) : items.OrderBy(item.MemberExpression);
                }
                else
                {
                    ordered = item.Order == SortOrder.Descending ? ordered.ThenByDescending(item.MemberExpression) : ordered.ThenBy(item.MemberExpression);
                }
            }

            return ordered;
        }

        private static void SetPropertyValues(TEntity item, Dictionary<Expression, object?> sets)
        {
            foreach (var update in sets)
            {
                var exp = update.Key;
                if (exp is LambdaExpression lambda)
                    exp = lambda.Body;
                if (exp is MemberExpression expression)
                    SetValue(expression.Member, item, update.Value);
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static void SetValue(MemberInfo memberInfo, object forObject, object? value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field: { ((FieldInfo)memberInfo).SetValue(forObject, value); } break;
                case MemberTypes.Property: { ((PropertyInfo)memberInfo).SetValue(forObject, value); } break;
                default: throw new NotImplementedException();
            }
        }

        #endregion
    }
}
