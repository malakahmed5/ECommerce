using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.ProductModuleEntities
{
    public class ProductBrand:BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;
    }
}
