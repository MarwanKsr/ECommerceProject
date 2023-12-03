using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Dto;
using ProductApi.Models;
using ProductApi.Services.Products;
using SharedLibrary.Dtos;
using System.Security.Claims;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IProductQueryService _productQueryRepository;
        private readonly IProductCommandService _productCommandRepository;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductQueryService productQueryRepository,
            IProductCommandService productCommandRepository,
            IMapper mapper)
        {
            _productQueryRepository = productQueryRepository;
            _productCommandRepository = productCommandRepository;
            _mapper = mapper;
            this._response = new ResponseDto();
        }
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await _productQueryRepository.GetProducts();
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ProductDto productDto = await _productQueryRepository.GetProductById(id);
                _response.Result = productDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<object> Create(ProductCreateModel productCreateModel/*, IFormFile image*/)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var productDto = _mapper.Map<ProductDto>(productCreateModel);
                productDto.CreatedBy = userId;
                ProductDto model = await _productCommandRepository.CreateProduct(productDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<object> Update([FromBody] ProductUpdateModel productUpdateModel)
        {
            try
            { 
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var productDto = _mapper.Map<ProductDto>(productUpdateModel);
                productDto.ModifiedBy = userId;
                ProductDto model = await _productCommandRepository.UpdateProduct(productDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("UpdateImage/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<object> UpdateImage(int id, IFormFile image)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                string imageUrl = await _productCommandRepository.UpdateProductImage(id, image, userId);
                _response.Result = imageUrl;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await _productCommandRepository.DeleteProduct(id);
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

        [HttpGet]
        [Route("{id}/DecreaseStock")]
        [Authorize(Roles = "Consumer")]
        public async Task<object> DecreaseStock(int id,[FromQuery] int wantedCount)
        {
            try
            {
                var isSuccess = await _productCommandRepository.DecreaseStock(id, wantedCount);
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

        [HttpGet]
        [Route("{id}/GetPrice")]
        [Authorize(Roles = "Consumer")]
        public async Task<object> GetPrice(int id)
        {
            try
            {
                var productPrice = await _productQueryRepository.GetProductPriceById(id);
                _response.Result = productPrice;
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
