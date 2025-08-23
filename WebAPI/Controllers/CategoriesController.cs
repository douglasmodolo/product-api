using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.DTOs.Mappings;
using WebAPI.Filters;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;
using WebAPI.Transactions.Interfaces;
using X.PagedList;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllPaginated([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = await _uow.CategoryRepository.GetAllPaginatedAsync(categoriesParameters);
            
            if (categories == null || !categories.Any())
                return NotFound("Nenhuma categoria encontrada.");

            return BuildPaginatedCategoriesResponse(categories);
        }

        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllPaginatedByName([FromQuery] CategoriesNameFilter categoriesNameFilter)
        {
            var categories = await _uow.CategoryRepository.GetCategoriesNameFilterAsync(categoriesNameFilter);

            if (categories == null || !categories.Any())
                return NotFound("Nenhuma categoria encontrada com o filtro de nome especificado.");

            return BuildPaginatedCategoriesResponse(categories);
        }        

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categories = await _uow.CategoryRepository.GetAllAsync();
            
            if (categories == null)
                return NotFound("Nenhuma categoria encontrada.");
            
            var categoryDtos = categories.ToCategoryDTOList();

            return Ok(categoryDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await _uow.CategoryRepository.GetAsync(c => c.Id == id);

            if (category == null)
                return NotFound("Categoria não encontrada.");
            
            var categoryDto = category.ToCategoryDTO();

            return Ok(categoryDto);
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesWithProducts()
        {
            var categories = await ((ICategoryRepository)_uow.CategoryRepository).GetCategoriesWithProductsAsync();
            
            if (categories == null || !categories.Any())
                return NotFound("Categorias com produtos não encontradas.");

            var category = categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                UrlImage = c.UrlImage,
                Products = c.Products?.Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToList() ?? new List<Product>()
            }).ToList();

            return Ok(categories);
        }

        [HttpPost]
        public ActionResult<CategoryDTO> Create(CategoryDTO categoryDto)
        {
            if (categoryDto == null) return BadRequest();

            var category = categoryDto.ToCategory();

            if (category == null)
                return BadRequest("Erro ao converter DTO para entidade.");

            var createdCategory = _uow.CategoryRepository.Create(category);

            if (createdCategory == null)
                return BadRequest("Erro ao criar a categoria.");

            var createdCategoryDto = createdCategory.ToCategoryDTO();

            if (createdCategoryDto == null)
                return BadRequest("Erro ao converter entidade para DTO.");

            return CreatedAtAction(nameof(GetById), new { id = createdCategoryDto.Id }, createdCategoryDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> Update(int id, CategoryDTO categoryDto)
        {
            if (categoryDto == null || id != categoryDto.Id)
                return BadRequest("Categoria inválida ou ID não corresponde.");

            var category = categoryDto.ToCategory();

            if (category == null)
                return BadRequest("Erro ao converter DTO para entidade.");

            var updatedCategory = _uow.CategoryRepository.Update(category);

            var updatedCategoryDto = updatedCategory.ToCategoryDTO();

            return Ok(updatedCategoryDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            if (!_uow.CategoryRepository.Delete(id))
                return NotFound("Categoria não encontrada.");

            return NoContent();
        }

        #region privateMethods
        private ActionResult<IEnumerable<CategoryDTO>> BuildPaginatedCategoriesResponse(IPagedList<Category> categories)
        {
            var metadata = new
            {
                categories.TotalItemCount,
                categories.PageSize,
                categories.PageNumber,
                categories.PageCount,
                categories.HasNextPage,
                categories.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

            var categoryDtos = categories.ToCategoryDTOList();

            return Ok(categoryDtos);
        }
        #endregion
    }
}
