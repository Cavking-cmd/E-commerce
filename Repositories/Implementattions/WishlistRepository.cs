using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        private readonly E_commerceDbContext _context;
        public WishlistRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Wishlist>> GetAllWishlistsAsync()
        {
            return await _context.Set<Wishlist>()
                .Include(w => w.Customer)
                .Include(w => w.WishlistItems)
                .ToListAsync();
        }

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            return await _context.Set<Wishlist>()
                .Include(w => w.Customer)
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }

        public async Task<Wishlist> GetWishlistAsync(Expression<Func<Wishlist, bool>> predicate)
        {
            return await _context.Set<Wishlist>()
                .Include(w => w.Customer)
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(predicate);
        }
    }
}
