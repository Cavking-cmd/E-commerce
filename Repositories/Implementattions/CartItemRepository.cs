using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Repositories.Implementattions
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        private readonly E_commerceDbContext _context;
        public CartItemRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CartItem> GetCartItemByIdAsync(Guid id)
        {
            return await _context.Set<CartItem>().FindAsync(id);
        }

        public async Task<ICollection<CartItem>> GetAllCartItemsAsync()
        {
            return await _context.Set<CartItem>().ToListAsync();
        }

        public async Task<CartItem> GetByCartAndProductID(Guid cartId, Guid productId)
        {
            return await _context.Set<CartItem>()
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);
        }

        public async Task<CartItem> ClearCartAsync(Guid cartId)
        {
            var cartItems = _context.Set<CartItem>().Where(c => c.CartId == cartId);
            _context.Set<CartItem>().RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<ICollection<CartItem>> GetCartItemsByUserIdAsync(Guid userId)
        {
            return await _context.Set<CartItem>()
                .Include(c => c.Cart)
                //.Where(c => c.Cart.UserId == userId)
                //Get current user 
                .ToListAsync();
        }
    }
}
