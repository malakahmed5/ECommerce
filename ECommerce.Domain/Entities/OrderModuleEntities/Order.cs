using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.OrderModuleEntities
{
    public class Order:BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress Address { get; set; } = default!;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>(); // = BasketItem
        public decimal SubTotal { get; set; } // = basketItems.Sum(item => item.Price * item.Quantity);
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
    }
}
