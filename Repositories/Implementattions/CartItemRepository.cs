using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        private readonly E_commerceDbContext _context;
        public CartItemRepository(E_commerceDbContext context) : base(context) 
        {
            _context = context;
        }

        

        public async Task<CartItem> ClearCartAsync(Guid cartId)
        {
            {
                return await _context.Set<CartItem>()
                    .Include(a => a.Quantity)
                    .Include(a => a.ProductName)
                    .Include(a => a.PricePerUnit)
                    .Include(a => a.Cart)
                    .Include(a => a.Product)
                    .FirstOrDefaultAsync(a => !a.IsDeleted && a.CartId == cartId);

            }
        }

        public async Task<CartItem> GetCartItemByIdAsync(Guid id)
        {
            return await _context.Set<CartItem>()
                .Include(a => a.Quantity)
                .Include(a => a.ProductName)
                .Include(a => a.PricePerUnit)
                .Include(a => a.Cart)
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a => !a.IsDeleted && a.Id == id);
                
        }

        public async Task<CartItem> GetByCartAndProductID(Guid cartId, Guid productId)
        {
            {
                return await _context.Set<CartItem>()
                    .Include(a => a.Quantity)
                    .Include(a => a.ProductName)
                    .Include(a => a.PricePerUnit)
                    .Include(a => a.Cart)
                    .Include(a => a.Product)
                    .FirstOrDefaultAsync(a => !a.IsDeleted && a.CartId == cartId && a.ProductId==productId);

            }
        }

        public async Task<ICollection<CartItem>> GetAllCartItemsAsync()
        {
            return await _context.Set<CartItem>()
                .Include(a => a.Quantity)
                .Include(a => a.ProductName)
                .Include(a => a.PricePerUnit)
                .Include(a => a.Cart)
                .Include(a => a.Product)
                .ToListAsync();

        }
    }
}
