using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly E_commerceDbContext _context;
            public RoleRepository(E_commerceDbContext context) :base(context) 
                {
                    _context = context;
                }


        public async Task<Role?> GetRoleAsync(string roleName)
        {
            return  _context.Set<Role>()
                .Include(a => a.UserRoles)
                .FirstOrDefault(r => r.Name== roleName);
        }

     
    }
}
