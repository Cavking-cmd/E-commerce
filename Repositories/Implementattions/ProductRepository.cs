using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly E_commerceDbContext _context;
        ProductRepository(E_commerceDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<ICollection<Product>> GetAllProducts()
        {
            return await _context.Set<Product>()
                .Include(a=> a.Name)
                .Include(a=> a.Description)
                .Include(a=> a.Price)
                .Include(a=> a.StockQuantity)
                .Include(a=> a.ImageUrl)
                .Include(a=>a.Reviews)
                .Include(a=>a.Category)
                .Include(a=>a.SubCategory)
                .ToListAsync();

        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Set <Product>()
                .Include(a => a.Name)
                .Include(a => a.Description)
                .Include(a => a.Price)
                .Include(a => a.StockQuantity)
                .Include(a => a.ImageUrl)
                .Include(a => a.Reviews)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .FirstOrDefaultAsync(a=> a.Id == id && a.IsDeleted == false);
        }

        public async Task<Product> GetProduct(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Set<Product>()
                 .Include(a => a.Name)
                 .Include(a => a.Description)
                 .Include(a => a.Price)
                 .Include(a => a.StockQuantity)
                 .Include(a => a.ImageUrl)
                 .Include(a => a.Reviews)
                 .Include(a => a.Category)
                 .Include(a => a.SubCategory)
                 .FirstOrDefaultAsync(predicate);
        }
    }
}
