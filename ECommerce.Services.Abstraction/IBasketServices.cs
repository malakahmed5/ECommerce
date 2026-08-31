using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IBasketServices
    {
        Task<Result<CustomerBasketDTO>> GetBasket(string basketId);
        Task<CustomerBasketDTO> CreateOrUpdateBasket(CustomerBasketDTO createOrUpdatedBasket);
        Task<bool> DeleteBasket(string basketId);

    }
}
