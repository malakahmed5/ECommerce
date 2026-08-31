using AutoMapper;
using ECommerce.Domain.Entities.BasketModuleEntities;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles.BasketMappingProfile
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasketDTO , CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDTO,BasketItem>().ReverseMap();
        }
    }
}
