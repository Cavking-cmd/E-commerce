using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        private readonly E_commerceDbContext _context;
        public CouponRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Set<Coupon>()
                .Include(a=>a.Orders)
                .ToListAsync();
        }

        public async Task<Coupon> GetCouponAsync(Expression<Func<Coupon, bool>> predicate)
        {
            return await _context.Set<Coupon>()
                           .Include(a => a.Orders)
                           .FirstOrDefaultAsync(predicate);
        }   

        public async Task<Coupon> GetCouponByIdAsync(Guid id)
        {
            return await _context.Set<Coupon>()
                .Include(a => a.Orders)
                .FirstOrDefaultAsync(a=>a.Id==id && a.IsDeleted == false);
        }  
    }
}
