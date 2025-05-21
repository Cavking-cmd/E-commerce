using System.Linq.Expressions;
using E_commerce.Core.Entities;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<Customer> 
    {
        Task<Customer> GetAsync(Guid id);
        Task<Customer> GetAsync(Expression<Func<Customer, bool>> predicate);
        Task<ICollection<Customer>>GetAllAsync();
    }
}
