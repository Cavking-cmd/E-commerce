using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly E_commerceDbContext _context;
        public ReviewRepository(E_commerceDbContext context) :base(context) 
        {
            _context = context;
        }

        public async Task<ICollection<Review>> GetAll()
        {
            return await _context.Set<Review>()
                .Include(a => a.Product)
                .Include(a => a.Rating)
                .Include(a => a.Comment)
                .Include(a => a.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review> GetReview(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Set<Review>()
                .Include(a => a.Product)
                .Include(a => a.Rating)
                .Include(a => a.Comment)
                .Include(a => a.ReviewDate)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<Review> GetById(Guid id)
        {
            return await _context.Set<Review>()
                .Include(a => a.Product)
                .Include(a => a.Rating)
                .Include(a => a.Comment)
                .Include(a => a.ReviewDate)
                .FirstOrDefaultAsync(a=>a.Id==id && a.IsDeleted==false);
        }
    }
}
