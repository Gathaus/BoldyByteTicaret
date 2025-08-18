using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> GetFeaturedProductsAsync(int count);
    Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<List<Product>> GetProductsByBrandAsync(int brandId);
    Task<(List<Product> Products, int TotalCount)> GetFilteredProductsAsync(
        string? style = null,
        List<string>? finishes = null,
        List<string>? colors = null,
        List<string>? rooms = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null,
        string? searchTerm = null,
        int page = 1,
        int pageSize = 12
    );
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<List<Product>> GetRelatedProductsAsync(int productId, int count = 4);
    Task<List<Product>> GetNewArrivalsAsync(int count = 12);
    Task<List<Product>> GetBestSellersAsync(int count = 12);
    Task<List<Product>> GetDiscountedProductsAsync(int count = 12);
}