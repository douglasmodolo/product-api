using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
        Product GetById(int id);
        Product Create(Product product);
        bool Update(int id, Product product);
        bool Delete(int id);
    }
}
