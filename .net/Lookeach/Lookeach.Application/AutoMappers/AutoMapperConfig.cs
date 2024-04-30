using AutoMapper;
using Lookeach.Core.Entities;
using Lookeach.Core.ViewModels.Brands;
using Lookeach.Core.ViewModels.Categories;
using Lookeach.Core.ViewModels.GenderTypes;
using Lookeach.Core.ViewModels.Images;
using Lookeach.Core.ViewModels.Roles;
using Lookeach.Core.ViewModels.Users;

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
            CreateMap<CategoryImage, CategoryImageViewModel>();
            CreateMap<GenderType, GenderTypeViewModel>();
        }
    }
}
