using System.Linq.Expressions;
using E_commerce.Core.Entities;
namespace E_commerce.Repositories.Interfaces
{
    public interface IUserProfileRepository :IBaseRepository<UserProfile>
    {
        Task<UserProfile?> GetProfileByIdAsync (Guid id);
        Task<UserProfile?> GetProfileAsync(Expression<Func<UserProfile, bool>> predicate);
        Task<ICollection<UserProfile?>> GetAll();
    }
}
