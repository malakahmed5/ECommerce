using ECommerce.Domain.Entities.BasketModuleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketById(string basketId);
        Task<CustomerBasket?> CreateOrUpdateBasket(CustomerBasket basket, TimeSpan timeToLive = default);
        Task<bool> DeleteBasket(string basketId);
    }
}
