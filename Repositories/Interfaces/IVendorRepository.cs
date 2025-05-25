 using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IVendorRepository : IBaseRepository<Vendor>
    {
        Task<Vendor> GetVendorAsync(Guid id);
        Task<Vendor> GetVendorAsync(Expression<Func<Vendor, bool>> predicate);
        Task<ICollection<Vendor>> GetAllVendorsAsync();
    }
}
