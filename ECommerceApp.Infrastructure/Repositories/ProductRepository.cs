using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && p.DeletedAt == null)
                .Include(p => p.Category)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetProductsBySearchTermAsync(string searchTerm)
        {
            return await _context.Products
                .Where(p => (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)) && p.DeletedAt == null)
                .Include(p => p.Category)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count)
        {
            return await _context.Products
                .Where(p => p.DeletedAt == null)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
} 