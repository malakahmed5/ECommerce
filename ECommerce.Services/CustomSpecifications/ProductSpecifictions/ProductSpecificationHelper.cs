using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.CustomSpecifications.ProductSpecifictions
{
    internal static class ProductSpecificationHelper
    {
        public static Expression<Func<Product , bool>> CreateCriteria(ProductQueryParams queryParams)
        {
            return p => (queryParams.brandId == null || p.BrandId == queryParams.brandId) &&
                         (queryParams.typeId == null || p.TypeId == queryParams.typeId) &&
                         (string.IsNullOrEmpty(queryParams.search) || p.Name.ToLower().Contains(queryParams.search.ToLower()));
        }
    }
}
