using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.CustomSpecifications.ProductSpecifictions
{
    internal class ProductSpecificationBrandsWithTypes:BaseSpecifications<int,Product>
    {
        public ProductSpecificationBrandsWithTypes(ProductQueryParams queryParams)
            : base(ProductSpecificationHelper.CreateCriteria(queryParams))
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand); 

            switch (queryParams.sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderByExpression(x => x.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesendingExpression(x => x.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderByExpression(x => x.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesendingExpression(x => x.Price);
                    break;
                default:
                    AddOrderByExpression(x => x.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize , queryParams.PageIndex);

        }

        public ProductSpecificationBrandsWithTypes(int id):base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}
