﻿using System.Linq.Expressions;
using E_commerce.Core.Entities;
using E_commerce.DataContext;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementattions
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly E_commerceDbContext _context;
        public ProductRepository(E_commerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Product>> GetAllProductsAsync()
        {
            return await _context.Set<Product>()
                .Include(a => a.Reviews)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .ToListAsync();

        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _context.Set<Product>()
                .Include(a => a.Reviews)
                .Include(a => a.Category)
                .Include(a => a.SubCategory)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
        }

        public async Task<Product> GetProductAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Set<Product>()
                 .Include(a => a.Reviews)
                 .Include(a => a.Category)
                 .Include(a => a.SubCategory)
                 .FirstOrDefaultAsync(predicate);
        }

        public async Task<ICollection<Product>> SearchByTagAsync(string tag)
        {
            return await _context.Set<Product>()
                .Where(p => p.Tags != null && p.Tags.ToLower().Contains(tag.ToLower()))
                .ToListAsync();
        }
    }
}
