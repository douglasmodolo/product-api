using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            var categories = _repository.GetAll()?.ToList();
            
            if (categories == null)
                return NotFound("Nenhuma categoria encontrada.");
            
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _repository.Get(c => c.Id == id);

            if (category == null)
                return NotFound("Categoria não encontrada.");
            
            return Ok(category);
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
        {
            var categories = ((ICategoryRepository)_repository).GetCategoriesWithProducts()?.ToList();
            
            if (categories == null || !categories.Any())
                return NotFound("Categorias com produtos não encontradas.");
            
            return Ok(categories);
        }

        [HttpPost]
        public ActionResult<Category> Create(Category category)
        {
            if (category == null)
                return BadRequest();
            
            var createdCategory = _repository.Create(category);
            
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Category> Update(int id, Category category)
        {
            if (category == null || id != category.Id)
                return BadRequest("Categoria inválida ou ID não corresponde.");

            var updatedCategory = _repository.Update(category);
            
            return Ok(updatedCategory);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            if (!_repository.Delete(id))
                return NotFound("Categoria não encontrada.");

            return NoContent();
        }
    }
}
