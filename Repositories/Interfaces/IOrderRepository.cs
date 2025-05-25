using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order> 
    {
        Task<Order>GetOrderByIdAsync(Guid id );
        Task<Order> GetOrderAsync(Expression<Func<Order, bool>> predicate);
        Task<ICollection<Order>> GetAllOrdersAsync();
    }
}
