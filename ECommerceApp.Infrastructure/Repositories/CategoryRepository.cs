using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveTopLevelCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive && c.ParentId == null)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesWithProductCountAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Products.Where(p => p.IsActive))
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            return await _context.Categories
                .Where(c => c.IsActive && c.Slug == slug)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId)
        {
            return await _context.Categories
                .Where(c => c.IsActive && c.ParentId == parentId)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoryHierarchyAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Children.Where(child => child.IsActive))
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> HasChildCategoriesAsync(int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.ParentId == categoryId && c.IsActive);
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _context.Products
                .AnyAsync(p => p.CategoryId == categoryId && p.IsActive);
        }
    }
} 