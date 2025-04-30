using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<UserRole> GetByUserAndRoleIdAsync(Guid userId , Guid roleId);
        Task<ICollection<UserRole>> GetByUserIdAsync(Guid userId );
    }
}
