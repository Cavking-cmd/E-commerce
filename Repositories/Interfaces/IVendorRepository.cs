using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IVendorRepository : IBaseRepository<Vendor>
    {
        Task<Vendor> GetAsync(Guid id);
        Task<Vendor> GetAsync(Expression<Func<Vendor, bool>> predicate);
        Task<ICollection<Vendor>> GetAllAsync();
    }
}
