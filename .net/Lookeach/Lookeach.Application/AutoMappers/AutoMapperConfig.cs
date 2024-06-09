using AutoMapper;
using Lookeach.Application.DTOs;
using Lookeach.Application.Features.Auths.Models.Reads;
using Lookeach.Application.Features.Chats.Models;
using Lookeach.Application.ViewModels.Brands;
using Lookeach.Application.ViewModels.Categories;
using Lookeach.Application.ViewModels.Colors;
using Lookeach.Application.ViewModels.GenderTypes;
using Lookeach.Application.ViewModels.Images;
using Lookeach.Application.ViewModels.Orders;
using Lookeach.Application.ViewModels.PaymentMethods;
using Lookeach.Application.ViewModels.Products;
using Lookeach.Application.ViewModels.Sizes;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Application.ViewModels.Vouchers;
using Lookeach.Core.Entities;

namespace Lookeach.Application.AutoMappers
{
    public class AutoMapperConfig
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(x =>
            {
                x.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod!.IsAssembly;
                x.AddProfile<MapperHandler>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper => Lazy.Value;
    }

    public class MapperHandler : Profile
    {
        public MapperHandler()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<Brand, BrandViewModel>().ReverseMap();
            CreateMap<BrandImage, BrandImageViewModel>().ReverseMap();
            CreateMap<Image, ImageViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryImage, CategoryImageViewModel>().ReverseMap();
            CreateMap<GenderType, GenderTypeViewModel>().ReverseMap();
            CreateMap<Size, SizeViewModel>().ReverseMap();
            CreateMap<Color, ColorViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<ProductImage, ProductImageViewModel>().ReverseMap();
            CreateMap<Voucher, VoucherViewModel>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodViewModel>().ReverseMap();
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Resource, ResourceDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<Lookeach.Core.Entities.Action, ActionDTO>().ReverseMap();
            CreateMap<Chat, ChatViewModel>().ReverseMap();
            CreateMap<ChatImage, ChatImageViewModel>().ReverseMap();
            CreateMap<ChatUser, ChatUserViewModel>().ReverseMap();
            CreateMap<ChatMessage, ChatMessageViewModel>().ReverseMap();
            CreateMap<User, UserChatViewModel>().ReverseMap();
            CreateMap<Core.Entities.Action, ActionViewModel>().ReverseMap();
            CreateMap<Resource, ResourceViewModel>().ReverseMap();
            CreateMap<Permission, PermissionViewModel>().ReverseMap();
        }
    }
}
