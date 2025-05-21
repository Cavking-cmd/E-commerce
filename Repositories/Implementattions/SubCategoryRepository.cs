using System;
using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class SubCategoryRepository : BaseRepository<SubCategory>, ISubCategoryRepository
    {
        private readonly E_commerceDbContext _context;
        public SubCategoryRepository(E_commerceDbContext context) :base(context)
        {
            _context = context;
        }

        public async Task<SubCategory> Get(Expression<Func<SubCategory, bool>> predicate)
        {
            return await _context.Set<SubCategory>()
                .Include(a=>a.Name)
                .Include(a=>a.Description)
                .Include(a=>a.Category)
                .Include(a=>a.Products)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<ICollection<SubCategory>> GetAll()
        {
            return await _context.Set<SubCategory>()
                .Include(a => a.Name)
                .Include(a => a.Description)
                .Include(a => a.Category)
                .Include(a => a.Products)
               .ToListAsync();
        }

        public async  Task<SubCategory> GetById(Guid id)
        {
            return await _context.Set<SubCategory>()
                .Include(a => a.Name)
                .Include(a => a.Description)
                .Include(a => a.Category)
                .Include(a => a.Products)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);  
                
               
        }
    }
}
