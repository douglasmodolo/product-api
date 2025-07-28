using WebAPI.Context;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Transactions.Interfaces;

namespace WebAPI.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        public AppDbContext _context { get; }

        public UnitOfWork(AppDbContext context) => _context = context;

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
