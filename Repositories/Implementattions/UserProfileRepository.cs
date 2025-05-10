using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        private readonly E_commerceDbContext _context;
            public UserProfileRepository (E_commerceDbContext context) : base (context)
            {
                _context = context;
            }

        public async  Task<ICollection<UserProfile?>> GetAll()
        {
            return await _context.Set<UserProfile>()
                .Where(a => a.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<UserProfile?> GetProfile(Guid id)
        {
            return await _context.Set<UserProfile?>()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
        }

        public async Task<UserProfile?> GetProfileAsync(Expression<Func<UserProfile, bool>> predicate)
        {
            return await _context.Set<UserProfile>()
                .FirstOrDefaultAsync(predicate);
        }
    }
}
