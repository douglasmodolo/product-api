using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products ?? Enumerable.Empty<Product>().AsQueryable();
        }

        public Product GetById(int id)
        {
            var product = _context.Products?.FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return product;
        }

        public Product Create(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products?.Add(product);
            _context.SaveChanges();

            return product;
        }

        public bool Update(int id, Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            var existingProduct = _context.Products?.Find(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");

            if (_context.Products?.Any(p => p.Id != id) != true)
                return false;
            
            existingProduct.UpdatedAt = DateTime.UtcNow;
            _context.Products?.Update(existingProduct);
            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var product = _context.Products?.Find(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            if (_context.Products?.Any(p => p.Id == id) != true)
                return false;

            _context.Products?.Remove(product);
            _context.SaveChanges();
            
            return true;
        }
    }
}
