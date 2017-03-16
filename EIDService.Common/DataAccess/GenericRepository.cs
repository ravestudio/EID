using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace EIDService.Common.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;
        private readonly bool _isSharedContext;

        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context, bool isSharedContext = true)
        {
            Contract.Requires(context != null);
            _context = context;
            _isSharedContext = isSharedContext;
            this.dbSet = context.Set<TEntity>();

        }

        public IQueryable<TModel> All<TModel>(params string[] includePaths)
            where TModel : class, IEntity
        {

            return Query<TModel>(x => true, includePaths);
        }

        public IQueryable<TModel> Query<TModel>(Expression<Func<TModel, bool>> predicate, params string[] includePaths)
            where TModel : class, IEntity
        {
            Contract.Requires(predicate != null);

            var items = GetSetWithIncludedPaths<TModel>(includePaths);

            if (predicate != null)
                return items.Where(predicate);
            return items;
        }

        private DbQuery<TModel> GetSetWithIncludedPaths<TModel>(IEnumerable<string> includedPaths)
            where TModel : class, IEntity
        {
            DbQuery<TModel> items = _context.Set<TModel>();

            foreach (var path in includedPaths ?? Enumerable.Empty<string>())
            {
                items = items.Include(path);
            }
            return items;
        }


        public IQueryable<TEntity> Get()
        {
            IQueryable<TEntity> query = dbSet;
            return query;
        }


        public void Create(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            this.dbSet.Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }
    }
}
