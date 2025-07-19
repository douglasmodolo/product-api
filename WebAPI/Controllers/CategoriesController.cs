using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            var categories = _context.Categories?.ToList();
            if (categories == null || !categories.Any())
            {
                return NotFound("Categorias não encontradas.");
            }
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _context.Categories?.Find(id);
            if (category == null)
            {
                return NotFound("Categoria não encontrada.");
            }
            return Ok(category);
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
        {
            var categories = _context.Categories?.Include(c => c.Products).ToList();
            if (categories == null || !categories.Any())
            {
                return NotFound("Categorias com produtos não encontradas.");
            }
            return Ok(categories);
        }

        [HttpPost]
        public ActionResult<Category> Create(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            _context.Categories?.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Category> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            var existingCategory = _context.Categories?.Find(id);
            if (existingCategory == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            existingCategory.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingCategory).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(existingCategory);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categories?.Find(id);
            if (category == null)
            {
                return NotFound("Categoria não encontrada.");
            }
            _context.Categories?.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
