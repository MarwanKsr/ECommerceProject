using AutoMapper;
using ProductApi.Dto;
using ProductApi.Models;

namespace ProductApi.Mapper
{
    public class ProductMapperProfiles 
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, ProductCreateModel>().ReverseMap();
                config.CreateMap<ProductDto, ProductUpdateModel>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
