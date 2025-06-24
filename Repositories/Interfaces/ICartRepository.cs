using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICartRepository :IBaseRepository<Cart>
    {
        Task<Cart> GetCartByIdAsync(Guid id);
        Task<Cart> GetCartAsync(Expression<Func<Cart, bool>>predicate);
        Task<ICollection<Cart>> GetAllCartsAsync();
        Task<Cart?> GetCartByCustomerIdAsync(Guid customerId);

    }
}
