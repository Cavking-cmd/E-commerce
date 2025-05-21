using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ISubCategoryRepository : IBaseRepository<SubCategory>
    {
        Task<SubCategory> Get(Expression<Func<SubCategory , bool>>predicate);
        Task<SubCategory> GetById(Guid id);
        Task<ICollection<SubCategory>> GetAll();
    }
}
