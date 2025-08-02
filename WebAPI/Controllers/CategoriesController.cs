using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.DTOs.Mappings;
using WebAPI.Filters;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Transactions.Interfaces;

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

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = _uow.CategoryRepository.GetAll()?.ToList();
            
            if (categories == null)
                return NotFound("Nenhuma categoria encontrada.");
            
            var categoryDtos = categories.ToCategoryDTOList();

            return Ok(categoryDtos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<CategoryDTO> GetById(int id)
        {
            var category = _uow.CategoryRepository.Get(c => c.Id == id);

            if (category == null)
                return NotFound("Categoria não encontrada.");
            
            var categoryDto = category.ToCategoryDTO();

            return Ok(categoryDto);
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesWithProducts()
        {
            var categories = ((ICategoryRepository)_uow.CategoryRepository).GetCategoriesWithProducts()?.ToList();
            
            if (categories == null || !categories.Any())
                return NotFound("Categorias com produtos não encontradas.");

            //var categoryDtos = categories.Select(c => new CategoryDTO
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    UrlImage = c.UrlImage,
            //    Products = c.Products?.Select(p => new ProductDTO
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Price = p.Price
            //    }).ToList() ?? new List<ProductDTO>()
            //}).ToList();

            //return Ok(categoryDtos);

            return Ok(categories);
        }

        [HttpPost]
        public ActionResult<CategoryDTO> Create(CategoryDTO categoryDto)
        {
            if (categoryDto == null) return BadRequest();

            var category = categoryDto.ToCategory();

            var createdCategory = _uow.CategoryRepository.Create(category);

            var createdCategoryDto = createdCategory.ToCategoryDTO();

            return CreatedAtAction(nameof(GetById), new { id = createdCategoryDto.Id }, createdCategoryDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> Update(int id, CategoryDTO categoryDto)
        {
            if (categoryDto == null || id != categoryDto.Id)
                return BadRequest("Categoria inválida ou ID não corresponde.");

            var category = categoryDto.ToCategory();

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
    }
}
