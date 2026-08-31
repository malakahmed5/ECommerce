using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModuleEntities;
using ECommerce.Services.Abstraction;
using ECommerce.Services.Exceptions;
using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Services
{
    public class BasketServices : IBasketServices
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketServices(IBasketRepository basketRepo , IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDTO> CreateOrUpdateBasket(CustomerBasketDTO createOrUpdatedBasket)
        { 
            var basketData = _mapper.Map<CustomerBasket>(createOrUpdatedBasket);
            var createdOrUpdatedBasket = await _basketRepo.CreateOrUpdateBasket(basketData);
            return _mapper.Map<CustomerBasketDTO>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasket(string basketId)
            => await _basketRepo.DeleteBasket(basketId);

        public async Task<Result<CustomerBasketDTO>> GetBasket(string basketId)
        {
            var basket = await _basketRepo.GetBasketById(basketId);

            if (basket is null)
                Error.NotFound("Baslket.NotFound", $"Basket With Id = {basketId} Is Not Found");

            return _mapper.Map<CustomerBasketDTO>(basket);
        }
    }
}
