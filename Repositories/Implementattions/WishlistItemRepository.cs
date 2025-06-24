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
    public class WishlistItemRepository : BaseRepository<WishlistItem>, IWishlistItemRepository
    {
        private readonly E_commerceDbContext _context;
        public WishlistItemRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<WishlistItem>> GetAllWishlistItemsAsync()
        {
            return await _context.Set<WishlistItem>()
                .Include(wi => wi.Wishlist)
                .Include(wi => wi.Product)
                .ToListAsync();
        }

        public async Task<WishlistItem> GetWishlistItemByIdAsync(Guid id)
        {
            return await _context.Set<WishlistItem>()
                .Include(wi => wi.Wishlist)
                .Include(wi => wi.Product)
                .FirstOrDefaultAsync(wi => wi.Id == id && !wi.IsDeleted);
        }

        public async Task<WishlistItem> GetWishlistItemAsync(Expression<Func<WishlistItem, bool>> predicate)
        {
            return await _context.Set<WishlistItem>()
                .Include(wi => wi.Wishlist)
                .Include(wi => wi.Product)
                
                .FirstOrDefaultAsync(predicate);
        }
    }
}
