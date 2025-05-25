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

        public async Task<ICollection<Review>> GetAllReviewsAsync()
        {
            return await _context.Set<Review>()
                .Include(a => a.Product)
                .ToListAsync();
        }

        public async Task<Review> GetReviewAsync(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Set<Review>()
                .Include(a => a.Product)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            return await _context.Set<Review>()
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a=>a.Id==id && a.IsDeleted==false);
        }
    }
}
