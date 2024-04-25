using AutoMapper;

namespace Lookeach.Infrastructure.AutoMappers
{
    public class AutoMapperConfig
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(x =>
            {
                x.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod!.IsAssembly;
                x
            })
        });
    }
    public class MapperHandler:Profile
    {

    }
}
