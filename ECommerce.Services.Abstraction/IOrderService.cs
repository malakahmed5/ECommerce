using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.OrderDTOs;
using ECommerce.Shared.DTOs.OrderModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IOrderService
    {
        Task<Result<OrderDTO>> CreateOrderAsync(CreateOrderDTO createOrderDTO , string userEmail);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethodsAsync();
        Task<Result<IEnumerable<OrderDTO>>> GetAllOrdersAsync(string email);
        Task<Result<OrderDTO>> GetOrderDetails(Guid orderId , string email);

    }
}
