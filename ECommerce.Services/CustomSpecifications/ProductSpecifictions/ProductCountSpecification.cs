using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.CustomSpecifications.ProductSpecifictions
{
    internal class ProductCountSpecification:BaseSpecifications<int , Product>
    {
        public ProductCountSpecification(ProductQueryParams queryParams)
            :base(ProductSpecificationHelper.CreateCriteria(queryParams))
        {
            
        }
    }
}
