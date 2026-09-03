using AutoMapper;
using ECommerce.Domain.Entities.BasketModuleEntities;
using ECommerce.Domain.Entities.OrderModuleEntities;
using ECommerce.Domain.Entities.ProductModuleEntities; // تأكدي من الـ Namespace الخاص بالـ Product
using ECommerce.Shared.DTOs.OrderDTOs;
using ECommerce.Shared.DTOs.OrderModuleDTOs;

namespace ECommerce.Services.MappingProfiles.OrderModuleMapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // 1. Map Address DTO -> Domain Entity
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();

            CreateMap<BasketItem, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore()) 
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductItemOrdered
                {
                    ProductId = src.Id,
                    ProductName = src.ProductName,
                    PictureUrl = src.PictureUrl
                }));

            // 3. Map CreateOrderDTO -> Order Entity
            CreateMap<CreateOrderDTO, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.SubTotal, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            // 4. Map OrderItem -> OrderItemsDTO
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.DeliveryMethodShortName, opt => opt.MapFrom(src => src.DeliveryMethod != null ? src.DeliveryMethod.ShortName : string.Empty))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.DeliveryMethod != null ? src.GetTotal() : src.SubTotal))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status.ToString()));
                
            CreateMap<DeliveryMethod, DeliveryMethodDTO>().ReverseMap();
        }
    }
}