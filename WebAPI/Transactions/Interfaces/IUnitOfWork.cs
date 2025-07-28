using WebAPI.Repositories.Interfaces;

namespace WebAPI.Transactions.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        void Commit();
    }
}
