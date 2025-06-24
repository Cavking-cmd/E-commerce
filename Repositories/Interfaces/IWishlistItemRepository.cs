using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace E_commerce.Repositories.Interfaces
{
    public interface IWishlistItemRepository : IBaseRepository<WishlistItem>
    {
        Task<ICollection<WishlistItem>> GetAllWishlistItemsAsync();
        Task<WishlistItem> GetWishlistItemByIdAsync(Guid id);
        Task<WishlistItem> GetWishlistItemAsync(Expression<Func<WishlistItem, bool>> predicate);
    }
}
