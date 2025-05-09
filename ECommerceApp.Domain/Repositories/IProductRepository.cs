using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsBySearchTermAsync(string searchTerm);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count);
    }
} 