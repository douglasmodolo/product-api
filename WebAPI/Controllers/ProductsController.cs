using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;
using WebAPI.Transactions.Interfaces;
using X.PagedList;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllPaginated([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _uow.ProductRepository.GetAllPaginatedAsync(productsParameters);

            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado.");

            return BuildPaginatedProductsResponse(products);
        }

        [HttpGet("filter/price/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllPaginatedByPrice([FromQuery] ProductsPriceFilter productsPriceFilter)
        {
            var products = await _uow.ProductRepository.GetProductsPriceFilterAsync(productsPriceFilter);

            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado com os filtros de preço especificados.");
            
            return BuildPaginatedProductsResponse(products);
        }        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _uow.ProductRepository.GetAllAsync();

            if (products == null)
                return NotFound("Nenhum produto encontrado.");
            
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _uow.ProductRepository.GetAsync(p => p.Id == id);

            if (product == null)
                return NotFound("Produto não encontrado.");
            
            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByCategoryId(int categoryId)
        {
            var products = await ((IProductRepository)_uow.ProductRepository).GetProductsByCategoryIdAsync(categoryId);
            
            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado para a categoria especificada.");

            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products).ToList();

            return Ok(productDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create(ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest();
            
            var product = _mapper.Map<Product>(productDto);
            var createdProduct = _uow.ProductRepository.Create(product);
            await _uow.CommitAsync();

            var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);

            return CreatedAtAction(nameof(GetById), new { id = createdProductDto.Id }, createdProductDto);
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        public async Task<ActionResult<UpdateProductResponseDto>> Patch(int id, JsonPatchDocument<UpdateProductRequestDto> patchProductDto)
        {
            if (patchProductDto == null || id <= 0)
                return BadRequest("Dados inválidos.");

            var product = await _uow.ProductRepository.GetAsync(p => p.Id == id);

            if (product == null)
                return NotFound("Produto não encontrado.");

            var productToPatch = _mapper.Map<UpdateProductRequestDto>(product);
            patchProductDto.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(productToPatch, product);

            _uow.ProductRepository.Update(product);
            await _uow.CommitAsync();

            var updatedProductDto = _mapper.Map<UpdateProductResponseDto>(product);
            return Ok(updatedProductDto);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, ProductDTO productDto)
        {
            if (productDto == null || id != productDto.Id)
                return BadRequest("Produto inválido ou ID não corresponde.");

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = _uow.ProductRepository.Update(product);
            await _uow.CommitAsync();

            var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            if (!_uow.ProductRepository.Delete(id))
                return NotFound("Produto não encontrado.");

            return NoContent();
        }

        #region privateMethods
        private ActionResult<IEnumerable<ProductDTO>> BuildPaginatedProductsResponse(IPagedList<Product> products)
        {
            var metadata = new
            {
                products.TotalItemCount,
                products.PageSize,
                products.PageNumber,
                products.PageCount,
                products.HasNextPage,
                products.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }
        #endregion
    }
}
