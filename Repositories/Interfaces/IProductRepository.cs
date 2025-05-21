using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IProductRepository :IBaseRepository<Product>
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<Product> GetProduct(Expression<Func<Product, bool>> predicate);
        Task<ICollection<Product>> GetAllProducts();
    }
}
