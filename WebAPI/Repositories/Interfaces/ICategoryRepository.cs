using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category>? GetCategoriesWithProducts();
    }
}
