using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerceApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.ToList();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<List<Product>> GetFeaturedProductsAsync(int count)
    {
        var products = await _productRepository.GetFeaturedProductsAsync(count);
        return products.ToList();
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
        return products.ToList();
    }

    public async Task<List<Product>> GetProductsByBrandAsync(int brandId)
    {
        var products = await _productRepository.FindAsync(p => p.BrandId == brandId && p.IsActive);
        return products.ToList();
    }

    public async Task<(List<Product> Products, int TotalCount)> GetFilteredProductsAsync(
        string? style = null,
        List<string>? finishes = null,
        List<string>? colors = null,
        List<string>? rooms = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null,
        string? searchTerm = null,
        int page = 1,
        int pageSize = 12)
    {
        // Build filter expression
        Expression<Func<Product, bool>> filterExpression = p => p.IsActive;

        if (!string.IsNullOrEmpty(style))
            filterExpression = p => p.IsActive && p.Style == style;

        if (minPrice.HasValue)
            filterExpression = p => p.IsActive && p.Price >= minPrice.Value;

        if (maxPrice.HasValue)
            filterExpression = p => p.IsActive && p.Price <= maxPrice.Value;

        if (!string.IsNullOrEmpty(searchTerm))
            filterExpression = p => p.IsActive && (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));

        // Get filtered products
        var products = await _productRepository.FindAsync(filterExpression);

        // Apply additional filters that can't be expressed in the expression
        if (finishes != null && finishes.Any())
            products = products.Where(p => finishes.Contains(p.Finish));

        if (colors != null && colors.Any())
            products = products.Where(p => colors.Contains(p.Color));

        if (rooms != null && rooms.Any())
            products = products.Where(p => rooms.Contains(p.Room));

        // Apply sorting
        products = sortBy?.ToLower() switch
        {
            "price_asc" => products.OrderBy(p => p.Price),
            "price_desc" => products.OrderByDescending(p => p.Price),
            "newest" => products.OrderByDescending(p => p.CreatedAt),
            "bestselling" => products.OrderByDescending(p => p.SalesCount),
            "rating" => products.OrderByDescending(p => p.AverageRating),
            _ => products.OrderByDescending(p => p.CreatedAt)
        };

        var totalCount = products.Count();

        // Apply pagination
        products = products.Skip((page - 1) * pageSize).Take(pageSize);

        return (products.ToList(), totalCount);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            _productRepository.Remove(product);
            await _productRepository.SaveChangesAsync();
        }
    }

    public async Task<List<Product>> GetRelatedProductsAsync(int productId, int count = 4)
    {
        var product = await _productRepository.GetProductWithCategoryAsync(productId);
        if (product == null)
            return new List<Product>();

        var relatedProducts = await _productRepository.FindAsync(p => 
            p.IsActive && p.Id != productId &&
            (p.CategoryId == product.CategoryId || p.BrandId == product.BrandId));

        return relatedProducts
            .OrderByDescending(p => p.AverageRating)
            .Take(count)
            .ToList();
    }

    public async Task<List<Product>> GetNewArrivalsAsync(int count = 12)
    {
        var products = await _productRepository.FindAsync(p => 
            p.IsActive && p.PublishedAt.HasValue);

        return products
            .OrderByDescending(p => p.PublishedAt)
            .Take(count)
            .ToList();
    }

    public async Task<List<Product>> GetBestSellersAsync(int count = 12)
    {
        var products = await _productRepository.FindAsync(p => p.IsActive);

        return products
            .OrderByDescending(p => p.SalesCount)
            .Take(count)
            .ToList();
    }

    public async Task<List<Product>> GetDiscountedProductsAsync(int count = 12)
    {
        var products = await _productRepository.FindAsync(p => 
            p.IsActive && p.ComparePrice.HasValue && p.ComparePrice > p.Price);

        return products
            .OrderByDescending(p => (p.ComparePrice!.Value - p.Price) / p.ComparePrice.Value)
            .Take(count)
            .ToList();
    }
}