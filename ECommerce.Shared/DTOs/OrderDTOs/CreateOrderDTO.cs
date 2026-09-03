using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.OrderModuleDTOs
{
    public class CreateOrderDTO
    {
        public string BasketId { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
    }
}
