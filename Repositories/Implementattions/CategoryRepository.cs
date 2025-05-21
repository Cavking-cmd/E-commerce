using System;
using System.Linq.Expressions;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly E_commerceDbContext _context;
        public CategoryRepository(E_commerceDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Category> Get(Expression<Func<Category, bool>> predicate)
        {
           return await _context.Set<Category>()
                .Include(a=>a.Name)
                .Include(a=>a.Description)
                .Include(a=>a.Products)
                .Include(a=>a.SubCategories)
                .FirstOrDefaultAsync(predicate);        
        }

        public async Task<ICollection<Category>> GetAll()
        {
            return await _context.Set<Category>()
                           .Include(a => a.Name)
                           .Include(a => a.Description)
                           .Include(a => a.Products)
                           .Include(a => a.SubCategories)
                           .ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Set<Category>()
                           .Include(a => a.Name)
                           .Include(a => a.Description)
                           .Include(a => a.Products)
                           .Include(a => a.SubCategories)
                           .FirstOrDefaultAsync(a=>a.Id==id && a.IsDeleted==false);
        }
    }
}
