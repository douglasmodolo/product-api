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

        public PagedList<Category>? GetCategoriesNameFilter(CategoriesNameFilter categoriesNameFilter)
        {
            var categories = GetAll()
                .OrderBy(c => c.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(categoriesNameFilter.Name))
                categories = categories.Where(c => c.Name!.ToLower().Contains(categoriesNameFilter.Name.ToLower()));
            
            return PagedList<Category>.ToPagedList(categories, categoriesNameFilter.PageNumber, categoriesNameFilter.PageSize);
        }

        public IEnumerable<Category>? GetCategoriesWithProducts()
        {
            return _context.Categories?
                .Include(c => c.Products)
                .ToList();
        }
    }
}
