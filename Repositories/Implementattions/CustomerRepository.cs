using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly E_commerceDbContext _context;
        public CustomerRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Customer>> GetAllAsync()
        {
            return await _context.Set<Customer>()
                .Include(a=>a.User)
                .ThenInclude(a => a.Profile)
                .Include(a => a.Carts)
                .ThenInclude(a => a.CartItems)
                .Include(a => a.Wishlist)
                .ThenInclude(a => a.WishlistItems)
                .Include(a => a.Orders)
                .Include(a => a.Reviews)
                .Include(a => a.ShippingAddresses)
                .ToListAsync();
        }

        public async  Task<Customer> GetAsync(Guid id)
        {
            return await _context.Set<Customer>()
                .Include(a => a.User)
                .ThenInclude(a => a.Profile)
                .Include(a => a.Carts)
                .ThenInclude(a => a.CartItems)
                .Include(a => a.Wishlist)
                .ThenInclude(a => a.WishlistItems)
                .Include(a => a.Orders)
                .Include(a => a.Reviews)
                .Include(a => a.ShippingAddresses)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
        }

        public async  Task<Customer> GetAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await _context.Set<Customer>()
                .Include(a => a.User)
                .ThenInclude(a => a.Profile)
                .Include(a => a.Carts)
                .ThenInclude(a => a.CartItems)
                .Include(a => a.Wishlist)
                .ThenInclude(a => a.WishlistItems)
                .Include(a => a.Orders)
                .Include(a => a.Reviews)
                .Include(a => a.ShippingAddresses)
                 .FirstOrDefaultAsync(predicate);
        }
    }
}
