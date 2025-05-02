using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly E_commerceDbContext _context;
        public UserRepository(E_commerceDbContext context) : base(context) 
        {
            _context = context;
        }

        public Task<User?> GetAsync(Guid id)
        {
           return _context.Set<User>()
                .Include(a => a.UserRoles)
                .ThenInclude(a =>a.Role)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
           return _context.Set<User>()
                 .Include(a => a.UserRoles)
                 .ThenInclude(a => a.Role)
                 .FirstAsync(a => a.Email == email && a.IsDeleted == false);
        }

        public Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate)
        {
            return _context.Set<User>()
                            .Include(a => a.UserRoles)
                            .ThenInclude(a => a.Role)
                            .FirstOrDefaultAsync(predicate);
        }
    }
}
