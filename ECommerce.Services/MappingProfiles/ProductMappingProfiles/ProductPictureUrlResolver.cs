using AutoMapper;
using AutoMapper.Execution;
using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Services.MappingProfiles.ProductMappingProfiles
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;
            if(source.PictureUrl.StartsWith("http") ||  source.PictureUrl.StartsWith("https")) return source.PictureUrl;

            var pictureUrl = $"{_configuration.GetSection("URLs")["BaseUrl"]}{source.PictureUrl}"; 
            return pictureUrl;
        }
    }
}
