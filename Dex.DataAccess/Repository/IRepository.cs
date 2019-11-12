using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dex.DataAccess.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Delete(TEntity entityToDelete);

        void Delete(object id);

        IEnumerable<TEntity> GetRepo(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties);

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Update(TEntity entityToUpdate);
    }
}
