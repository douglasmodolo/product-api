using WebAPI.Models;
using WebAPI.Pagination;
using X.PagedList;

namespace WebAPI.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<Product>? GetAllPaginated(ProductsParameters productsParams);
        Task<IPagedList<Product>?> GetAllPaginatedAsync(ProductsParameters productsParams);
        Task<IPagedList<Product>?> GetProductsPriceFilterAsync(ProductsPriceFilter productsPriceFilter);
        Task<IEnumerable<Product>?> GetProductsByCategoryIdAsync(int categoryId);
    }
}
