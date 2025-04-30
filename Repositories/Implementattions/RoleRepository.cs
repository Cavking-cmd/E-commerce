using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;

namespace E_commerce.Repositories.Implementattions
{
    public class RoleRepository : BaseRepository<User>, IRoleRepository
    {
        private readonly E_commerceDbContext _context;
        public RoleRepository(E_commerceDbContext context) :base(context) 
        {
            _context = context;
        }


        public Task<Role?> GetRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

     
    }
}
