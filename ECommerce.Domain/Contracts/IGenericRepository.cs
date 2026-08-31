using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IGenericRepository<TKey, TEntity>
        where TEntity : BaseEntity<TKey>
    {
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TKey, TEntity> specifications);
        public Task<TEntity?> GetByIdAsync(TKey id);
        public Task<TEntity?> GetByIdAsync(ISpecifications<TKey, TEntity> specifications);
        public Task AddAsync(TEntity entity);
        public void Delete(TEntity entity);
        public void Update(TEntity entity);
        public Task<int> CountAsync(ISpecifications<TKey, TEntity> specifications);
    }
}
