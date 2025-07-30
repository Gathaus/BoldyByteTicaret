using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveTopLevelCategoriesAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesWithProductCountAsync();
        Task<Category> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId);
        Task<IEnumerable<Category>> GetCategoryHierarchyAsync();
        Task<bool> HasChildCategoriesAsync(int categoryId);
        Task<bool> HasProductsAsync(int categoryId);
    }
} 