using System.Linq.Expressions;
using E_commerce.Core.Dtos;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICouponRepository : IBaseRepository<Coupon>
    {
        Task<Coupon> GetCouponAsync(Expression<Func<Coupon ,bool>>predicate);
        Task<Coupon> GetCouponByIdAsync(Guid id);
        Task<ICollection<Coupon>> GetAllCouponsAsync();
        
    }
}
