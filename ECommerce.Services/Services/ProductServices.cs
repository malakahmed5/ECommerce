using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Services.Abstraction;
using ECommerce.Services.CustomSpecifications.ProductSpecifictions;
using ECommerce.Shared;
using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams)
        {
            var productRepository = _unitOfWork.GetRepository<int, Product>();

            var productSpecifications = new ProductSpecificationBrandsWithTypes(queryParams);
            var products = await productRepository.GetAllAsync(productSpecifications);
            
            var returnedData = _mapper.Map<IEnumerable<ProductDTO>>(products);
            int dataCounted = await productRepository.CountAsync(new ProductCountSpecification(queryParams));
            return new PaginatedResult<ProductDTO>(dataCounted, returnedData.Count(), queryParams.PageIndex, returnedData);

        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var productRepository = _unitOfWork.GetRepository<int, Product>();

            var specifications = new ProductSpecificationBrandsWithTypes(id);

            var product = await productRepository.GetByIdAsync(specifications);

            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id = {id} Is Not Found");

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var brandRepository = _unitOfWork.GetRepository<int, ProductBrand>();

            var productBrands = await brandRepository.GetAllAsync();
            if(productBrands is null || !productBrands.Any()) return [];

            return _mapper.Map<IEnumerable<BrandDTO>>(productBrands);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var typeRepository = _unitOfWork.GetRepository<int, ProductType>();

            var productTypes = await typeRepository.GetAllAsync();
            if(productTypes is null || !productTypes.Any()) return [];

            return _mapper.Map<IEnumerable<TypeDTO>>(productTypes);
        }

    }
}
