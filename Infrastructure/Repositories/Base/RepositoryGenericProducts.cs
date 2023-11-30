using Dominio.Interfaces.Common;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories.Base
{
    public class RepositoryGenericProducts<TEntity> : IRepositoryProducts<TEntity> where TEntity : class, new()
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public RepositoryGenericProducts(ProductsContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> result = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                result = result.Include(includeProperty);

            return await result.AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await _dbSet.FindAsync(id);

        }

        public virtual async Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public virtual async Task Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public object GetPrimaryKeyValue(TEntity entity)
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            var primaryKey = entityType.FindPrimaryKey();

            return primaryKey.Properties
                .Select(x => x.PropertyInfo.GetValue(entity))
                .FirstOrDefault();
        }
    }
}
