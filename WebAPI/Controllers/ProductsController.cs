using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _repository.GetAll()?.ToList();

            if (products == null)
                return NotFound("Produtos não encontrados.");
            
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _repository.GetById(id);

            if (product == null)
                return NotFound("Produto não encontrado.");
            
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            if (product == null)
                return BadRequest();
                        
            var createdProduct = _repository.Create(product);
            
            if (createdProduct == null)
                return BadRequest("Erro ao criar o produto.");

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, Product product)
        {
            if (product == null || id != product.Id)
                return BadRequest("Produto inválido ou ID não corresponde.");

            if (!_repository.Update(id, product))
                return NotFound($"Produto com ID {id} não encontrado.");

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
