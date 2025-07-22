using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories?.AsNoTracking().ToList();

            if (categories == null || !categories.Any())
                return new List<Category>();

            return categories;
        }

        public Category GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");

            var category = _context.Categories?.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            return category;
        }

        public IEnumerable<Category> GetCategoriesWithProducts()
        {            
            return _context.Categories?.Include(c => c.Products).ToList() ?? new List<Category>();
        }

        public Category Create(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            _context.Categories?.Add(category);
            _context.SaveChanges();

            return category;
        }

        public Category Update(int id, Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            if (id != category.Id)
                throw new ArgumentException("Category ID mismatch.", nameof(id));

            var existingCategory = _context.Categories?.Find(id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            existingCategory.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingCategory).State = EntityState.Modified;
            _context.SaveChanges();

            return existingCategory;
        }

        public Category Delete(int id)
        {
            var category = _context.Categories?.Find(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            _context.Categories?.Remove(category);
            _context.SaveChanges();

            return category;
        }
    }
}
