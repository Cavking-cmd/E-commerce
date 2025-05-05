using E_commerce.DataContext;

namespace E_commerce.Repositories.Implementattions
{
    public class UnitOfWork
    {
        private readonly E_commerceDbContext _context;
        public UnitOfWork(E_commerceDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
