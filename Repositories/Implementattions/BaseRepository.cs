using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected E_commerceDbContext _context;
        public BaseRepository(E_commerceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }



        public async Task SoftDeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
       public async Task DeleteAsync(T entity)
       {
             _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
       }


    }
}
