using AutoMapper;
using ShoppingCardApi.Models.ViewModel;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using System.Reflection.PortableExecutable;

namespace ShoppingCardApi.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductDto, ProductCreateModel>().ReverseMap();
            CreateMap<ImageDto, Image>().ReverseMap();
            CreateMap<CardHeader, CardHeaderDto>().ReverseMap();
            CreateMap<CardHeaderCreateModel, CardHeaderDto>().ReverseMap();
            CreateMap<CardDetails, CardDetailsDto>().ReverseMap();
            CreateMap<CardDetailsCreateModel, CardDetailsDto>().ReverseMap();
            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<CardCreateModel, CardDto>().ReverseMap();
        }
    }
}
