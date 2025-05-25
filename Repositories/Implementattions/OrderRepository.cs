using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly E_commerceDbContext _context;
        public OrderRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async  Task<ICollection<Order>> GetAllOrdersAsync()
        {
            return await _context.Set<Order>()
                .Include(a=>a.OrderItems)
                .Include(a=>a.Coupons)
                .ToListAsync();
        }

        public async  Task<Order> GetOrderAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Set<Order>()
                .Include(a => a.OrderItems)
                .Include(a => a.Coupons)
                .FirstOrDefaultAsync();
        }

        public Task<Order> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
