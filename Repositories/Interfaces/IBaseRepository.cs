using System.Linq.Expressions;

namespace E_commerce.Repositories.Interfaces
{
    public interface IBaseRepository <T>
    {
        //Task<T?> GetAsync(Guid id); 
        Task CreateAsync (T entity);
        Task Update (T entity);
        Task SoftDeleteAsync (T entity);
        Task DeleteAsync(T entity);
        Task<bool> CheckAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync (Expression<Func<T,bool>>predicate);
    }
}
