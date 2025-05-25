using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<Review> GetReviewAsync(Expression<Func<Review , bool>> predicate);
        Task<Review> GetReviewByIdAsync(Guid id);
        Task<ICollection<Review>> GetAllReviewsAsync();
    }
}
