using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        {         
        }

        public IEnumerable<Product>? GetProductsByCategoryId(int categoryId)
        {
            return GetAll().Where(p => p.CategoryId == categoryId).ToList();
        }
    }
}
