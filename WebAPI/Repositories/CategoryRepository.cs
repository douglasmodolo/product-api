using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;
using X.PagedList;

namespace WebAPI.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {       
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Category>?> GetAllPaginatedAsync(CategoriesParameters categoriesParams)
        {
            var categories = await GetAllAsync();
            var ordenedCategories = categories.OrderBy(c => c.Id).AsQueryable();

            var pagedCategories = await ordenedCategories.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);

            return pagedCategories;
        }

        public async Task<IPagedList<Category>?> GetCategoriesNameFilterAsync(CategoriesNameFilter categoriesNameFilter)
        {
            var categories = await GetAllAsync();
            var ordenedeCategories = categories.AsQueryable();

            if (!string.IsNullOrEmpty(categoriesNameFilter.Name))
                ordenedeCategories = ordenedeCategories.Where(c => c.Name!.ToLower().Contains(categoriesNameFilter.Name.ToLower()));
            
            if (ordenedeCategories == null)
                return null;

            var pagedCategories = await ordenedeCategories.ToPagedListAsync(categoriesNameFilter.PageNumber, categoriesNameFilter.PageSize);

            return pagedCategories;
        }

        public async Task<IEnumerable<Category>?> GetCategoriesWithProductsAsync()
        {
            if (_context.Categories == null)
                return null;

            var categoriesWithProducts = await _context.Categories.Include(c => c.Products).ToListAsync();

            return categoriesWithProducts;
        }
    }
}
