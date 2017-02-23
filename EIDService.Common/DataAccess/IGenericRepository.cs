using EIDService.Common.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EIDService.Common.DataAccess
{
    public interface IGenericRepository<TEntity>
        where TEntity : class, IEntity

    {
        IQueryable<TModel> All<TModel>(params string[] includePaths) where TModel : class, IEntity;
        IQueryable<TModel> Query<TModel>(Expression<Func<TModel, bool>> predicate, params string[] includePaths) where TModel : class, IEntity;

        IQueryable<TEntity> Get();

        void Create(TEntity entity);
        void Update(TEntity entity);
    }
}
