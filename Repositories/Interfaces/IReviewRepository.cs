using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<Review> GetReview(Expression<Func<Review , bool>> predicate);
        Task<Review> GetById(Guid id);
        Task<ICollection<Review>> GetAll();
    }
}
