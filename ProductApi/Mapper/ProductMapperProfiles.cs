using AutoMapper;
using ProductApi.Dto;
using ProductApi.Models;

namespace ProductApi.Mapper
{
    public class ProductMapperProfiles : Profile
    {
        public ProductMapperProfiles()
        { 
            CreateMap<ProductDto, ProductCreateModel>().ReverseMap();
            CreateMap<ProductDto, ProductUpdateModel>().ReverseMap();
        }
    }
}
