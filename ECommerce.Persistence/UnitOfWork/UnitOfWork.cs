using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.Contetxts;
using ECommerce.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() 
            where TEntity : BaseEntity<TKey>
        {
            var entity = typeof(TEntity);
            if(_repositories.TryGetValue(entity , out var repository))
                return (IGenericRepository<TKey,TEntity>) repository;

            var newRepository = new GenericRepository<TKey, TEntity>(_dbContext);
            _repositories[entity] = newRepository;
            return newRepository;
        }

        public async Task<int> SaveChanges() => await _dbContext.SaveChangesAsync();
    }
}
