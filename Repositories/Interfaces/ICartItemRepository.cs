using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICartItemRepository :IBaseRepository<CartItem>
    {
        Task<CartItem> GetCartItemByIdAsync(Guid id);
        Task<ICollection<CartItem>> GetAllCartItemsAsync();
        Task<CartItem> GetByCartAndProductID(Guid cartId, Guid productId);
        Task <CartItem> ClearCartAsync(Guid cartId);
        
    }
}
