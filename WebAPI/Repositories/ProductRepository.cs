using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Pagination;
using WebAPI.Repositories.Interfaces;
using X.PagedList;

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

        public async Task<IPagedList<Product>?> GetAllPaginatedAsync(ProductsParameters productsParams)
        {
            var products = await GetAllAsync();
            var ordenedProducts = products.OrderBy(p => p.Id).AsQueryable();

            var pagedProducts = await ordenedProducts.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);

            return pagedProducts;
        }

        public async Task<IEnumerable<Product>?> GetProductsByCategoryIdAsync(int categoryId)
        {
            var products = await GetAllAsync();
            var filteredProducts = products.Where(p => p.CategoryId == categoryId);

            return filteredProducts;
        }

        public async Task<IPagedList<Product>?> GetProductsPriceFilterAsync(ProductsPriceFilter productsPriceFilter)
        {
            var products = await GetAllAsync();
            var productsQueryable = products.AsQueryable();

            if (productsPriceFilter.Price.HasValue && !string.IsNullOrEmpty(productsPriceFilter.PriceCriterias))
            {
                switch (productsPriceFilter.PriceCriterias.ToLower())
                {
                    case "greaterthan":
                        productsQueryable = productsQueryable.Where(p => p.Price > productsPriceFilter.Price.Value);
                        break;
                    case "lessthan":
                        productsQueryable = productsQueryable.Where(p => p.Price < productsPriceFilter.Price.Value);
                        break;
                    case "equalto":
                        productsQueryable = productsQueryable.Where(p => p.Price == productsPriceFilter.Price.Value);
                        break;
                    default:
                        throw new ArgumentException("Invalid price criteria specified.");
                }
            }

            var pagedProducts = await productsQueryable.ToPagedListAsync(productsPriceFilter.PageNumber, productsPriceFilter.PageSize);

            return pagedProducts;
        }
    }
}
