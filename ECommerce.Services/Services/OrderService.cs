using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModuleEntities;
using ECommerce.Domain.Entities.OrderModuleEntities;
using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Services.Abstraction;
using ECommerce.Services.CustomSpecifications.OrderSpecification;
using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.OrderDTOs;
using ECommerce.Shared.DTOs.OrderModuleDTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork,
            IMapper mapper , ILogger<OrderService> logger)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<OrderDTO>> CreateOrderAsync(CreateOrderDTO createOrderDTO, string userEmail)
        {
            // 1. Check Basket
            var customerBakset = await _basketRepository.GetBasketById(createOrderDTO.BasketId);
            if (customerBakset is null || !customerBakset.Items.Any())
                return Error.NotFound("Basket.NotFound", "The Requested Basket was Not Found Or It's Empty");

            // 2. Validate Products & Create OrderItems
            var orderItems = new List<OrderItem>();
            var productRepo = _unitOfWork.GetRepository<int, Product>();

            foreach (var item in customerBakset.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product.NotFound", $"The Product with Id {item.Id} was Not Found");

                orderItems.Add(CreateOrderItem(item, product));
            }

            // 3. Check Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetByIdAsync(createOrderDTO.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", "The Requested Delivery Method was Not Found");

            // 4. Create Order Object
            var order = _mapper.Map<Order>(createOrderDTO);
            order.Items = orderItems;
            order.SubTotal = orderItems.Sum(item => item.Price * item.Quantity);
            order.UserEmail = userEmail;

            // 5. Save to DB
            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            if (!(await _unitOfWork.SaveChanges() > 0))
                return Error.Faliure("Order.CreationFailed", "Failed to Create Order");

            // 6. Delete Basket after successful order
            //await _basketRepository.DeleteBasket(createOrderDTO.BasketId);

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<Result<IEnumerable<OrderDTO>>> GetAllOrdersAsync(string email)
        {
            var orderSpec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(orderSpec);
            if(orders is null || !orders.Any())
                return Error.NotFound("Order.NotFound", $"No Orders Found For The User With This Email '{email}'");

            return _mapper.Map<List<OrderDTO>>(orders.ToList());
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();

            if (deliveryMethods is null || !deliveryMethods.Any())
                return Error.NotFound("DeliveryMethod.NotFound", "No Delivery Methods Found");

            return _mapper.Map<List<DeliveryMethodDTO>>(deliveryMethods.ToList());
        }

        public async Task<Result<OrderDTO>> GetOrderDetails(Guid orderId, string email)
        {
            var orderSpec = new OrderSpecification(orderId,email);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetByIdAsync(orderSpec);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"No Order Found With Id '{orderId}' For The User With Email '{email}'");

            return _mapper.Map<OrderDTO>(order);
        }

        #region Helper Method
        private OrderItem CreateOrderItem(BasketItem item, Product product)
        {
            var createOrderItem = _mapper.Map<OrderItem>(item);
            createOrderItem.Price = product.Price; // Real price from DB
            return createOrderItem;
        } 
        #endregion
    }
}
