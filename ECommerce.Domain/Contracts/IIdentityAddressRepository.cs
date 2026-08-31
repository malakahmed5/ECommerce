using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.IdentityModuleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IIdentityAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAsync(Func<IQueryable<Address> , IQueryable<Address>>? query = null);
        Task<Address?> GetByIdAsync(int id);
        Task AddAsync(Address entity);
        void Update(Address entity);
        void Delete(Address entity);
        Task<int> SaveChangesAsync();

    }
}
