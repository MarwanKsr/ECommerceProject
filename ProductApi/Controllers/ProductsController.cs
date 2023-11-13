using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Dto;
using ProductApi.Models;
using ProductApi.Repository.Products;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IProductQueryRepository _productQueryRepository;
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductQueryRepository productQueryRepository,
            IProductCommandRepository productCommandRepository,
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
        //[Authorize]
        [Route("Create")]
        public async Task<object> Create(ProductCreateModel productCreateModel/*, IFormFile image*/)
        {
            try
            {
                var productDto = _mapper.Map<ProductDto>(productCreateModel);
                productDto.CreatedBy = "User";
                ProductDto model = await _productCommandRepository.CreateProduct(productDto, default);
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
        //[Authorize]
        [Route("Update")]
        public async Task<object> Update([FromBody] ProductUpdateModel productUpdateModel/*, IFormFile image*/)
        {
            try
            { 
                var productDto = _mapper.Map<ProductDto>(productUpdateModel);
                productDto.ModifiedBy = "User";
                ProductDto model = await _productCommandRepository.UpdateProduct(productDto, default);
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

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        [Route("Delete/{id}")]
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
    }
}
