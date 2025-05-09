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
            
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;
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
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }
    }
} 