using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        IEnumerable<Category> GetCategoriesWithProducts();
        Category Create(Category category);
        Category Update(int id, Category category);
        Category Delete(int id);
    }
}
