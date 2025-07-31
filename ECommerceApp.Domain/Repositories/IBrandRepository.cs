using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand> GetByIdAsync(int id);
        Task<Brand> GetBySlugAsync(string slug);
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<IEnumerable<Brand>> GetActiveAsync();
        Task<Brand> CreateAsync(Brand brand);
        Task<Brand> UpdateAsync(Brand brand);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> SlugExistsAsync(string slug);
    }
} 