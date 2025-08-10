using WebAPI.Models;
using WebAPI.Pagination;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        PagedList<Category>? GetAllPaginated(CategoriesParameters categoriesParams);
        IEnumerable<Category>? GetCategoriesWithProducts();
    }
}
