using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetRoleAsync(string roleName);
    }
}
