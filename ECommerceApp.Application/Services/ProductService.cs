using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductWithCategoryAsync(id);
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetProductsByCategoryAsync(categoryId);
        }
        
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllProductsAsync();
            }
            
            return await _productRepository.GetProductsBySearchTermAsync(searchTerm);
        }
        
        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count)
        {
            return await _productRepository.GetFeaturedProductsAsync(count);
        }
        
        public async Task<Product> AddProductAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.IsActive = true;
            product.PublishedAt = DateTime.UtcNow;
            
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            
            return product;
        }
        
        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null)
            {
                throw new Exception($"Product with id {product.Id} not found.");
            }
            
            // Update basic properties
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ShortDescription = product.ShortDescription;
            existingProduct.Price = product.Price;
            existingProduct.ComparePrice = product.ComparePrice;
            existingProduct.Stock = product.Stock;
            existingProduct.SKU = product.SKU;
            existingProduct.Barcode = product.Barcode;
            existingProduct.Weight = product.Weight;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.BrandId = product.BrandId;
            existingProduct.IsActive = product.IsActive;
            existingProduct.IsFeatured = product.IsFeatured;
            existingProduct.IsDigital = product.IsDigital;
            existingProduct.RequiresShipping = product.RequiresShipping;
            existingProduct.TrackQuantity = product.TrackQuantity;
            existingProduct.ContinueSelling = product.ContinueSelling;
            existingProduct.LowStockThreshold = product.LowStockThreshold;
            
            // SEO fields
            existingProduct.MetaTitle = product.MetaTitle;
            existingProduct.MetaDescription = product.MetaDescription;
            existingProduct.Slug = product.Slug;
            
            existingProduct.UpdatedAt = DateTime.UtcNow;
            
            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();
        }
        
        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception($"Product with id {id} not found.");
            }
            
            // Soft delete
            product.DeletedAt = DateTime.UtcNow;
            product.IsActive = false;
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }
    }
} 