using ElasticSearch.API.DTOs;
using ElasticSearch.API.Repositories;
using System.Net;

namespace ElasticSearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto dto)
        {

            var response = await _repository.SaveAsync(dto.CreateProduct());
            if (response == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "kayıt esnasında bir hata meydana geldi" }, System.Net.HttpStatusCode.InternalServerError);
            }
            return ResponseDto<ProductDto>.Success(response.CreateDto(), System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            var productListDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature?.Width, x.Feature?.Height, x.Feature?.Color))).ToList();
            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
        }
        public async Task<ResponseDto<ProductDto?>> GetByIdAsync(string id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return ResponseDto<ProductDto?>.Fail("ürün bulunamadı", HttpStatusCode.NotFound);
            }
            return ResponseDto<ProductDto?>.Success(product.CreateDto(), HttpStatusCode.OK);

        }
        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto dto)
        {
            var response = await _repository.UpdateAsync(dto);
            if (!response)
                return ResponseDto<bool>.Fail(new List<string> { "kayıt esnasında bir hata meydana geldi" }, System.Net.HttpStatusCode.InternalServerError);

            return ResponseDto<bool>.Success(true, System.Net.HttpStatusCode.NoContent);


        }
        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var response = await _repository.DeleteAsync(id);
            if (!response)
                return ResponseDto<bool>.Fail(new List<string> { "kayıt esnasında bir hata meydana geldi" }, System.Net.HttpStatusCode.InternalServerError);

            return ResponseDto<bool>.Success(true, System.Net.HttpStatusCode.NoContent);
        }
    }
}
