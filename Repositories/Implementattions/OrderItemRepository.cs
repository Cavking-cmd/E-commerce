using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        private readonly E_commerceDbContext _context;
        public OrderItemRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async  Task<ICollection<OrderItem>> GetAllOrdersAsync()
        {
            return await _context.Set<OrderItem>()
                .Include(a=>a.Order)
                .Include(a=>a.Product)
                .ToListAsync();
        }

        public async Task<OrderItem> GetOrderAsync(Expression<Func<OrderItem, bool>> predicate)
        {
            return await _context.Set<OrderItem>()
                .Include(a => a.Order)
                .Include(a => a.Product)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<OrderItem> GetOrderByIdAsync(Guid id)
        {
            return await _context.Set<OrderItem>()
                .Include(a => a.Order)
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a=> a.Id == id && !a.IsDeleted  );
        }
    }
}
