using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI.Context;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }                

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public bool Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            
            if (entity == null)
                return false;
            
            _context.Set<T>().Remove(entity);
            
            return _context.SaveChanges() > 0;
        }
    }
}
