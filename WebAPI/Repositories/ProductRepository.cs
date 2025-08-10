using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        {         
        }

        //public IEnumerable<Product>? GetAllPaginated(ProductsParameters productsParams)
        //{
        //    return GetAll()
        //        .OrderBy(p => p.Name)
        //        .Skip((productsParams.PageNumber - 1) * productsParams.PageSize)
        //        .Take(productsParams.PageSize)
        //        .ToList();
        //}

        public PagedList<Product>? GetAllPaginated(ProductsParameters productsParams)
        {
            var products = GetAll()
                .OrderBy(p => p.Id)
                .AsQueryable();

            return PagedList<Product>.ToPagedList(products, productsParams.PageNumber, productsParams.PageSize);
        }

        public IEnumerable<Product>? GetProductsByCategoryId(int categoryId)
        {
            return GetAll().Where(p => p.CategoryId == categoryId).ToList();
        }
    }
}
