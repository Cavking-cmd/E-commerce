using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ISubCategoryRepository : IBaseRepository<SubCategory>
    {
        Task<SubCategory> GetSubCategoryAsync(Expression<Func<SubCategory , bool>>predicate);
        Task<SubCategory> GetSubCategoryByIdAsync(Guid id);
        Task<ICollection<SubCategory>> GetAllSubCategoriesAsync();
    }
}
