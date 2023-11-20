using AutoMapper.Internal;
using ShoppingCardApi.Models;
using ShoppingCartApi.Models.Dto;

namespace ShoppingCardApi.Services.Base
{
    public interface IBaseService : IDisposable
    {
        ResponseDto responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
