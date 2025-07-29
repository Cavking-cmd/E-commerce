using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface IProductRepository :IBaseRepository<Product>
    {
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> GetProductAsync(Expression<Func<Product, bool>> predicate);
        Task<ICollection<Product>> GetAllProductsAsync();

        Task<ICollection<Product>> SearchByTagAsync(string tag);
    }
}
