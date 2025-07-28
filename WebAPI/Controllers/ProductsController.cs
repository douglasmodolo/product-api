using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Transactions.Interfaces;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ProductsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            var products = _uow.ProductRepository.GetAll()?.ToList();

            if (products == null)
                return NotFound("Nenhum produto encontrado.");
            
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _uow.ProductRepository.Get(p => p.Id == id);

            if (product == null)
                return NotFound("Produto não encontrado.");
            
            return Ok(product);
        }

        [HttpGet("category/{categoryId:int}")]
        public ActionResult<IEnumerable<Product>> GetByCategoryId(int categoryId)
        {
            var products = ((IProductRepository)_uow.ProductRepository).GetProductsByCategoryId(categoryId)?.ToList();
            
            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado para a categoria especificada.");
            
            return Ok(products);
        }

        [HttpPost]
        public ActionResult<Product> Create(Product product)
        {
            if (product == null)
                return BadRequest();
                        
            var createdProduct = _uow.ProductRepository.Create(product);
            _uow.Commit();

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Product> Update(int id, Product product)
        {
            if (product == null || id != product.Id)
                return BadRequest("Produto inválido ou ID não corresponde.");

            var updatedProduct = _uow.ProductRepository.Update(product);
            _uow.Commit();

            return Ok(updatedProduct);
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
