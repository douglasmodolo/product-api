using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product>? GetProductsByCategoryId(int categoryId);
    }
}
