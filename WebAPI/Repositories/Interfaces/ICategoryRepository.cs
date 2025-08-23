using WebAPI.Models;
using WebAPI.Pagination;
using X.PagedList;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IPagedList<Category>?> GetAllPaginatedAsync(CategoriesParameters categoriesParams);
        Task<IPagedList<Category>?> GetCategoriesNameFilterAsync(CategoriesNameFilter categoriesNameFilter);
        Task<IEnumerable<Category>?> GetCategoriesWithProductsAsync();
    }
}
