using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.OrderModuleDTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; } = default!;
        public string UserEmail { get; set; } = default!;
        public ICollection<OrderItemDTO> Items { get; set; } = default!;
        public AddressDTO Address { get; set; } = default!;
        public string DeliveryMethodShortName { get; set; } = default!;
        public string OrderStatus { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = default!;
        public decimal SubTotal { get; set; } = default!;
        public decimal Total { get; set; } = default!;
    }
}
