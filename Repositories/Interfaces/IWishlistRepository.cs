using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace E_commerce.Repositories.Interfaces
{
    public interface IWishlistRepository : IBaseRepository<Wishlist>
    {
        Task<ICollection<Wishlist>> GetAllWishlistsAsync();
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        Task<Wishlist> GetWishlistAsync(Expression<Func<Wishlist, bool>> predicate);
    }
}
