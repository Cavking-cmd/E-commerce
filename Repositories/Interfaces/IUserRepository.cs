using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate);
        Task<User?> GetEmailAsync(string email);
        Task<User?> GetAsync(Guid id);
    }
}
