using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetActiveTopLevelCategoriesAsync()
        {
            return await _categoryRepository.GetActiveTopLevelCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesWithProductCountAsync()
        {
            return await _categoryRepository.GetActiveCategoriesWithProductCountAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug cannot be null or empty", nameof(slug));

            return await _categoryRepository.GetBySlugAsync(slug);
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId)
        {
            return await _categoryRepository.GetChildCategoriesAsync(parentId);
        }

        public async Task<IEnumerable<Category>> GetCategoryHierarchyAsync()
        {
            return await _categoryRepository.GetCategoryHierarchyAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // Set timestamps
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;

            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(category.Slug))
            {
                category.Slug = GenerateSlug(category.Name);
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
            if (existingCategory == null)
                throw new InvalidOperationException($"Category with ID {category.Id} not found");

            // Update properties
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.ImageUrl = category.ImageUrl;
            existingCategory.ParentId = category.ParentId;
            existingCategory.IsActive = category.IsActive;
            existingCategory.SortOrder = category.SortOrder;
            existingCategory.MetaTitle = category.MetaTitle;
            existingCategory.MetaDescription = category.MetaDescription;
            existingCategory.Slug = category.Slug;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(existingCategory);
            await _categoryRepository.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            // Check if category has child categories
            if (await _categoryRepository.HasChildCategoriesAsync(id))
                throw new InvalidOperationException("Cannot delete category that has child categories");

            // Check if category has products
            if (await _categoryRepository.HasProductsAsync(id))
                throw new InvalidOperationException("Cannot delete category that has products");

            // Soft delete
            category.DeletedAt = DateTime.UtcNow;
            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasChildCategoriesAsync(int categoryId)
        {
            return await _categoryRepository.HasChildCategoriesAsync(categoryId);
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _categoryRepository.HasProductsAsync(categoryId);
        }

        private string GenerateSlug(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            return name.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("ı", "i")
                .Replace("ş", "s")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ö", "o")
                .Replace("ç", "c")
                .Replace("İ", "i")
                .Replace("Ş", "s")
                .Replace("Ğ", "g")
                .Replace("Ü", "u")
                .Replace("Ö", "o")
                .Replace("Ç", "c");
        }
    }
} 