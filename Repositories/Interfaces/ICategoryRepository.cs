using System.Linq.Expressions;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICategoryRepository: IBaseRepository<Category>
    {
        Task<Category> GetCategoryAsync(Expression<Func<Category, bool>> predicate);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<ICollection<Category>> GetAllCategoriesAsync();
    }
}
