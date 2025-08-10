using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {       
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Category>? GetAllPaginated(CategoriesParameters categoriesParams)
        {
            var categories = GetAll()
                .OrderBy(c => c.Id)
                .AsQueryable();

            return PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);
        }

        public IEnumerable<Category>? GetCategoriesWithProducts()
        {
            return _context.Categories?
                .Include(c => c.Products)
                .ToList();
        }
    }
}
