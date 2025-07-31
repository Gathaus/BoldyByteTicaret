using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetActiveTopLevelCategoriesAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesWithProductCountAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId);
        Task<IEnumerable<Category>> GetCategoryHierarchyAsync();
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> HasChildCategoriesAsync(int categoryId);
        Task<bool> HasProductsAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
} 