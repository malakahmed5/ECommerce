using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.IdentityModuleEntities;
using ECommerce.Persistence.IdentityData.Contexts;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repository
{
    public class IdentityAddressRepository : IIdentityAddressRepository
    {
        private readonly StoreIdentityDbContext _dbContext;

        public IdentityAddressRepository(StoreIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Address address)
            => await _dbContext.AddAsync(address);

        public void Delete(Address address)
            => _dbContext.Remove(address);

        public async Task<IEnumerable<Address>> GetAllAsync(Func<IQueryable<Address>, IQueryable<Address>>? query = null)
        {
            IQueryable<Address> queryable = _dbContext.Set<Address>();
            if(query is not null)
                queryable = query(queryable);
            return await queryable.ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(int id)
            => await _dbContext.Set<Address>().FindAsync(id);

        public async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public void Update(Address address)
            => _dbContext.Update(address);
    }
}
