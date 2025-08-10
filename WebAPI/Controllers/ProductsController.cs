using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;
using WebAPI.Transactions.Interfaces;

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
        public ActionResult<IEnumerable<ProductDTO>> GetAllPaginated([FromQuery] ProductsParameters productsParameters)
        {
            var products = _uow.ProductRepository.GetAllPaginated(productsParameters);

            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado.");

            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.PageNumber,
                products.TotalPages,
                products.HasNextPage,
                products.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetAll()
        {
            var products = _uow.ProductRepository.GetAll()?.ToList();

            if (products == null)
                return NotFound("Nenhum produto encontrado.");
            
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ProductDTO> GetById(int id)
        {
            var product = _uow.ProductRepository.Get(p => p.Id == id);

            if (product == null)
                return NotFound("Produto não encontrado.");
            
            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        [HttpGet("category/{categoryId:int}")]
        public ActionResult<IEnumerable<ProductDTO>> GetByCategoryId(int categoryId)
        {
            var products = ((IProductRepository)_uow.ProductRepository).GetProductsByCategoryId(categoryId)?.ToList();
            
            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado para a categoria especificada.");

            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products).ToList();

            return Ok(productDtos);
        }

        [HttpPost]
        public ActionResult<ProductDTO> Create(ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest();
            
            var product = _mapper.Map<Product>(productDto);
            var createdProduct = _uow.ProductRepository.Create(product);
            _uow.Commit();

            var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);

            return CreatedAtAction(nameof(GetById), new { id = createdProductDto.Id }, createdProductDto);
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        public ActionResult<UpdateProductResponseDto> Patch(int id, JsonPatchDocument<UpdateProductRequestDto> patchProductDto)
        {
            if (patchProductDto == null || id <= 0)
                return BadRequest("Dados inválidos.");

            var product = _uow.ProductRepository.Get(p => p.Id == id);

            if (product == null)
                return NotFound("Produto não encontrado.");

            var productToPatch = _mapper.Map<UpdateProductRequestDto>(product);
            patchProductDto.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(productToPatch, product);

            _uow.ProductRepository.Update(product);
            _uow.Commit();

            var updatedProductDto = _mapper.Map<UpdateProductResponseDto>(product);
            return Ok(updatedProductDto);
        }


        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> Update(int id, ProductDTO productDto)
        {
            if (productDto == null || id != productDto.Id)
                return BadRequest("Produto inválido ou ID não corresponde.");

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = _uow.ProductRepository.Update(product);
            _uow.Commit();

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
    }
}
