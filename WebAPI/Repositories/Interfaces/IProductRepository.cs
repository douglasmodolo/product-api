using WebAPI.Models;
using WebAPI.Pagination;

namespace WebAPI.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<Product>? GetAllPaginated(ProductsParameters productsParams);
        PagedList<Product>? GetAllPaginated(ProductsParameters productsParams);
        PagedList<Product>? GetProductsPriceFilter(ProductsPriceFilter productsPriceFilter);
        IEnumerable<Product>? GetProductsByCategoryId(int categoryId);

    }
}
