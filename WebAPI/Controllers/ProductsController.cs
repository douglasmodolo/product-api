using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _context.Products?.ToList();
            if (products == null)
            {
                return NotFound("Produtos não encontrados.");
            }
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _context.Products?.Find(id);
            if (product == null)
            {
                return NotFound("Produto não encontrado.");
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products?.Add(product);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Product> Update(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Produto não encontrado.");
            }
            var existingProduct = _context.Products?.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            product.UpdatedAt = DateTime.UtcNow;
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(existingProduct);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products?.Find(id);
            if (product == null)
            {
                return NotFound("Produto não encontrado.");
            }
            _context.Products?.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
