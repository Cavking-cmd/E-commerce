using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        private readonly E_commerceDbContext _context;
        public VendorRepository(E_commerceDbContext context) : base(context)
        {
            _context = _context;
        }

        public async Task<Vendor> GetAsync(Guid id)
        {
           return await _context.Set<Vendor>()
                .Include(a => a.Email)
                .Include(a => a.Description)
                .Include(a => a.BusinessName)
                .Include(a => a.StoreLocation)
                .Include(a => a.UserId)
                .Include(a=> a.User)
                .ThenInclude(a=> a.Profile)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);

        }

        public async Task<Vendor> GetAsync(Expression<Func<Vendor, bool>> predicate)
        {
            return await _context.Set<Vendor>()
                .Include(a => a.Email)
                .Include(a => a.Description)
                .Include(a => a.BusinessName)
                .Include(a => a.StoreLocation)
                .Include(a => a.UserId)
                .Include(a => a.User)
                .ThenInclude(a => a.Profile)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<ICollection<Vendor>> GetAllAsync()
        {
            return await _context.Set<Vendor>()
                .Include(a => a.Email)
                .Include(a => a.Description)
                .Include(a => a.BusinessName)
                .Include(a => a.StoreLocation)
                .Include(a => a.UserId)
                .Include(a => a.User)
                .ThenInclude(a => a.Profile)
                .ToListAsync();
                
        }
    }
}
