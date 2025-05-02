using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
       

        public UserRoleRepository(E_commerceDbContext context) : base(context)
        {
        }

        public async Task<UserRole> GetByUserAndRoleIdAsync(Guid userId, Guid roleId)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(a => a.Id == userId && a.RoleId == roleId);
        }

        public async Task<ICollection<UserRole>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(a=> a.UserId == userId)
                .Include(a=>a.Role)// Load Related Role  
                .ToListAsync();
        }
    }
}

