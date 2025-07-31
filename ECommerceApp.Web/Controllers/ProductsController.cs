using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using ECommerceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ITagService _tagService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            ITagService tagService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(
            string category = null, 
            string brand = null, 
            decimal? minPrice = null, 
            decimal? maxPrice = null,
            string sortBy = "name",
            int page = 1,
            int pageSize = 12)
        {
            try
            {
                // 1. Get all categories for sidebar filter
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // 2. Get all brands for filter
                var brands = await _brandService.GetActiveBrandsAsync();
                ViewBag.Brands = brands?.ToList() ?? new List<Brand>();

                // 3. Get all products and apply filters
                var allProducts = await _productService.GetAllProductsAsync();
                var filteredProducts = allProducts.Where(p => p.IsActive);

                // Apply category filter
                if (!string.IsNullOrEmpty(category))
                {
                    var selectedCategory = categories?.FirstOrDefault(c => c.Slug == category);
                    if (selectedCategory != null)
                    {
                        filteredProducts = filteredProducts.Where(p => p.CategoryId == selectedCategory.Id);
                        ViewBag.SelectedCategory = selectedCategory;
                    }
                }

                // Apply brand filter
                if (!string.IsNullOrEmpty(brand))
                {
                    var selectedBrand = brands?.FirstOrDefault(b => b.Slug == brand);
                    if (selectedBrand != null)
                    {
                        filteredProducts = filteredProducts.Where(p => p.BrandId == selectedBrand.Id);
                        ViewBag.SelectedBrand = selectedBrand;
                    }
                }

                // Apply price filters
                if (minPrice.HasValue)
                {
                    filteredProducts = filteredProducts.Where(p => p.Price >= minPrice.Value);
                    ViewBag.MinPrice = minPrice.Value;
                }

                if (maxPrice.HasValue)
                {
                    filteredProducts = filteredProducts.Where(p => p.Price <= maxPrice.Value);
                    ViewBag.MaxPrice = maxPrice.Value;
                }

                // Apply sorting
                switch (sortBy.ToLower())
                {
                    case "price_asc":
                        filteredProducts = filteredProducts.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        filteredProducts = filteredProducts.OrderByDescending(p => p.Price);
                        break;
                    case "newest":
                        filteredProducts = filteredProducts.OrderByDescending(p => p.PublishedAt);
                        break;
                    case "rating":
                        filteredProducts = filteredProducts.OrderByDescending(p => p.AverageRating);
                        break;
                    case "bestseller":
                        filteredProducts = filteredProducts.OrderByDescending(p => p.SalesCount);
                        break;
                    default: // name
                        filteredProducts = filteredProducts.OrderBy(p => p.Name);
                        break;
                }

                var productsList = filteredProducts.ToList();

                // 4. Pagination
                var totalProducts = productsList.Count;
                var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                var pagedProducts = productsList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.Products = pagedProducts;
                ViewBag.TotalProducts = totalProducts;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;
                ViewBag.SortBy = sortBy;

                // 5. Best sellers in selected category
                var bestSellerProducts = productsList.OrderByDescending(p => p.SalesCount).Take(6).ToList();
                ViewBag.BestSellerProducts = bestSellerProducts;

                // 6. Featured/Popular products
                var featuredProducts = await _productService.GetFeaturedProductsAsync(6);
                ViewBag.FeaturedProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();

                // 7. Popular categories (categories with most products)
                var popularCategories = categories?.Where(c => c.IsActive && !c.ParentId.HasValue).Take(10).ToList() ?? new List<Category>();
                ViewBag.PopularCategories = popularCategories;

                // 8. Tags for filters
                var tags = await _tagService.GetActiveTagsAsync();
                ViewBag.Tags = tags?.ToList() ?? new List<Tag>();

                // 9. Price range info
                if (productsList.Any())
                {
                    ViewBag.MinPriceAvailable = productsList.Min(p => p.Price);
                    ViewBag.MaxPriceAvailable = productsList.Max(p => p.Price);
                }
                else
                {
                    ViewBag.MinPriceAvailable = 0;
                    ViewBag.MaxPriceAvailable = 1000;
                }

                _logger.LogInformation("Products page loaded successfully with {ProductCount} products", totalProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products page");
                
                // Provide empty data to prevent view errors
                ViewBag.Categories = new List<Category>();
                ViewBag.Brands = new List<Brand>();
                ViewBag.Products = new List<Product>();
                ViewBag.BestSellerProducts = new List<Product>();
                ViewBag.FeaturedProducts = new List<Product>();
                ViewBag.PopularCategories = new List<Category>();
                ViewBag.Tags = new List<Tag>();
                ViewBag.TotalProducts = 0;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 1;
                ViewBag.MinPriceAvailable = 0;
                ViewBag.MaxPriceAvailable = 1000;
            }

            return View();
        }

        // Single product detail page
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null || !product.IsActive)
                {
                    return NotFound();
                }

                // Related products from same category
                var relatedProducts = await _productService.GetProductsByCategoryAsync(product.CategoryId);
                ViewBag.RelatedProducts = relatedProducts?.Where(p => p.Id != id && p.IsActive).Take(4).ToList() ?? new List<Product>();

                // Categories for breadcrumb
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).ToList() ?? new List<Category>();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID {ProductId}", id);
                return NotFound();
            }
        }
    }
} 