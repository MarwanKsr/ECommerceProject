using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Services.Orders;
using SharedLibrary.Dtos;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        protected ResponseDto _response;
        private IOrderCommandService _orderCommandService;

        public OrdersController(IOrderCommandService orderCommandService)
        {
            _orderCommandService = orderCommandService;
            this._response = new ResponseDto();
        }

        [HttpPost]
        [Route("{id}/MakeOrderSuccess")]
        public async Task<object> MakeOrderSuccess(int id)
        {
            try
            {
                var isSuccess = await _orderCommandService.UpdateOrderStatus(id, true);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
