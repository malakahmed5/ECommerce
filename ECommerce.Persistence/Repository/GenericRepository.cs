using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.Contetxts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repository
{
    public class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity>
        where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TKey, TEntity> specifications)
        {
           var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications);
           return await query.ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task<TEntity?> GetByIdAsync(ISpecifications<TKey, TEntity> specifications)
        {
            var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications);
            return await query.FirstOrDefaultAsync();
        }
        public async Task AddAsync(TEntity entity) 
            => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
            =>_dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
            => _dbContext.Set<TEntity>().Update(entity);

        public async Task<int> CountAsync(ISpecifications<TKey, TEntity> specifications)
        {
            var data = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications);
            return await data.CountAsync();
        }
            
    }

}
