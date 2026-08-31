using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TEntity : BaseEntity<TKey>;
        public Task<int> SaveChanges();
    }
}
