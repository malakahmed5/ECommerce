using AutoMapper;
using ECommerce.Domain.Entities.OrderModuleEntities;
using ECommerce.Shared.DTOs.OrderModuleDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles.OrderModuleMapping
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.Product.PictureUrl)) return string.Empty;
            if(source.Product.PictureUrl.StartsWith("http")|| source.Product.PictureUrl.StartsWith("https"))
                return source.Product.PictureUrl;

            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if(baseUrl is null) return string.Empty;

            return $"{baseUrl}{source.Product.PictureUrl}";
        }
    }
}
