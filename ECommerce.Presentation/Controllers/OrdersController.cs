using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.OrderDTOs;
using ECommerce.Shared.DTOs.OrderModuleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO createOrderDTO)
        {
            var userEmail = GetUserEmailFromToken();
            var result = await _orderService.CreateOrderAsync(createOrderDTO, userEmail);
            return HandleResult(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var userEmail = GetUserEmailFromToken();
            var result = await _orderService.GetAllOrdersAsync(userEmail);
            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(Guid orderId)
        {
            var userEmail = GetUserEmailFromToken();
            var result = await _orderService.GetOrderDetails(orderId, userEmail);
            return HandleResult(result);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _orderService.GetDeliveryMethodsAsync();
            return HandleResult(result);
        }
    }
}
