using AutoMapper;
using OrderApi.Models;
using OrderApi.Models.ViewModel;

namespace OrderApi.Mapper
{
    public class OrderMapperProfiles : Profile
    {
        public OrderMapperProfiles() 
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
        }
    }
}
