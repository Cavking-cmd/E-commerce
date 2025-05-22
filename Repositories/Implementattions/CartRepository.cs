using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly E_commerceDbContext _context;
        public CartRepository(E_commerceDbContext context) : base(context) 
        {
            _context = context;
        }

        public async  Task<ICollection<Cart>> GetAllAsync()
        {
            return await _context.Set<Cart>()
                .Include(a=>a.Customer)
                .Include(a=>a.CartItems)
                .Include(a=>a.Coupons)
                .ToListAsync();
        }

        public  async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _context.Set<Cart>()
                .Include(a => a.Customer)
                .Include(a => a.CartItems)
                .Include(a => a.Coupons)
                .FirstOrDefaultAsync(a=>a.Id == id && !a.IsDeleted);
        }

        public async Task<Cart> GetCartAsync(Expression<Func<Cart, bool>> predicate)
        {
            return await _context.Set<Cart>()
                .Include(a => a.Customer)
                .Include(a => a.CartItems)
                .Include(a => a.Coupons)
                .FirstOrDefaultAsync(predicate);
        }
    }
}
