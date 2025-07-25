using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {       
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Category>? GetCategoriesWithProducts()
        {
            return _context.Categories?
                .Include(c => c.Products)
                .ToList();
        }
    }
}
