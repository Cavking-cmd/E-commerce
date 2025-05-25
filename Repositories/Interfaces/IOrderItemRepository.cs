using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IOrderItemRepository :IBaseRepository<OrderItem>
    {
        Task<OrderItem> GetOrderAsync(Expression<Func<OrderItem, bool>> predicate);
        Task<OrderItem> GetOrderByIdAsync(Guid id);
        Task<ICollection<OrderItem>> GetAllOrdersAsync();
    }
}
