using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dex.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Dex.DataAccess.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DexContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DexContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) { throw new ArgumentNullException(nameof(entityToDelete)); }

            _dbSet.Remove(entityToDelete);
        }

        public void Delete(object id)
        {
            if (id == null) { throw new ArgumentNullException(nameof(id)); }

            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public IEnumerable<TEntity> GetRepo(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<TEntity> repo = _dbSet;

            if (filter != null)
            {
                repo = repo.Where(filter);
            }

            if (includeProperties != null)
            {
                repo = includeProperties.Aggregate(repo, (current, property) => current.Include(property));
            }

            if (orderBy != null)
            {
                repo = orderBy(repo);
            }

            return repo.AsEnumerable();
        }

        public TEntity GetByID(object id)
        {
            if (id == null) { throw new ArgumentNullException(nameof(id)); }

            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }

            _dbSet.Add(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null) { throw new ArgumentNullException(nameof(entityToUpdate)); }

            _dbSet.Attach(entityToUpdate);
            var entry = _dbContext.Entry(entityToUpdate);
            entry.State = EntityState.Modified;
        }
    }
}