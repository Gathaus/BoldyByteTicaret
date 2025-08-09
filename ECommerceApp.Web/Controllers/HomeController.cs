using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Web.Models;
using ECommerceApp.Domain.Services;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Infrastructure.Data;

namespace ECommerceApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IBrandService _brandService;
    private readonly ITagService _tagService;
    private readonly IServiceProvider _serviceProvider;

    public HomeController(
        ILogger<HomeController> logger, 
        ICategoryService categoryService,
        IProductService productService,
        IBrandService brandService,
        ITagService tagService,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _categoryService = categoryService;
        _productService = productService;
        _brandService = brandService;
        _tagService = tagService;
        _serviceProvider = serviceProvider;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeIndexViewModel();
        
        try
        {
            // 1. Categories for Popular Categories section
            var categories = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();
            
            // 2. Featured Products for Best Seller section
            var featuredProducts = await _productService.GetFeaturedProductsAsync(20);
            viewModel.FeaturedProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();
            
            // 3. All Products for different sections
            var allProducts = await _productService.GetAllProductsAsync();
            var activeProducts = allProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();
            
            // 4. Best Seller Products (products with highest sales)
            viewModel.BestSellerProducts = activeProducts.OrderByDescending(p => p.SalesCount).Take(10).ToList();
            
            // 5. Popular Brands
            var brands = await _brandService.GetPopularBrandsAsync(6);
            viewModel.PopularBrands = brands?.ToList() ?? new List<Brand>();
            
            // 6. Suggest Today Products (featured + top rated)
            viewModel.SuggestTodayProducts = activeProducts.Where(p => p.IsFeatured || p.AverageRating >= 4).Take(15).ToList();
            
            // 7. Top Rated Products
            viewModel.TopRatedProducts = activeProducts.Where(p => p.AverageRating >= 4).OrderByDescending(p => p.AverageRating).Take(10).ToList();
            
            // 8. Discount Products (products with compare price)
            viewModel.DiscountProducts = activeProducts.Where(p => p.ComparePrice > p.Price).Take(10).ToList();
            
            // 9. New Products (recently published)
            viewModel.NewProducts = activeProducts.Where(p => p.PublishedAt.HasValue && p.PublishedAt.Value >= DateTime.UtcNow.AddDays(-30))
                                          .OrderByDescending(p => p.PublishedAt).Take(10).ToList();
            
            // 10. Tags for labels
            var tags = await _tagService.GetActiveTagsAsync();
            viewModel.Tags = tags?.ToList() ?? new List<Tag>();
            
            // 11. Categories for Best Seller tabs (limit to main categories)
            viewModel.MainCategories = viewModel.Categories.Where(c => c.IsActive && !c.ParentId.HasValue).Take(5).ToList();
            
            // 12. Trending Search Keywords (you can customize these)
            viewModel.TrendingSearches = new List<string>
            {
                "Vacuum Robot", "Bluetooth Speaker", "Oled TV", "Security Camera", 
                "Macbook M1", "Smart Washing Machine", "iPad Mini 2023", "PS5", 
                "Earbuds", "Air Condition Inverter", "Flycam", "Electric Bike",
                "Gaming Computer", "Smart Air Purifier", "Apple Watch"
            };
            
            // 13. Best Weekly Deals (products with highest discount percentages or special weekly offers)
            var weeklyDeals = activeProducts.Where(p => p.ComparePrice.HasValue && p.ComparePrice > p.Price)
                                          .OrderByDescending(p => (p.ComparePrice!.Value - p.Price) / p.ComparePrice.Value * 100) // Order by discount percentage
                                          .Take(12) // Take top 12 deals
                                          .ToList();
            
            // If we don't have enough discounted products, fill with featured products
            if (weeklyDeals.Count < 12)
            {
                var additionalProducts = activeProducts.Where(p => p.IsFeatured && !weeklyDeals.Contains(p))
                                                     .Take(12 - weeklyDeals.Count)
                                                     .ToList();
                weeklyDeals.AddRange(additionalProducts);
            }
            
            viewModel.BestWeeklyDeals = weeklyDeals;

            // 14. Best Seller Products by Categories (for Best Seller section with category tabs)
            var topCategories = viewModel.MainCategories.Take(6).ToList(); // Top 6 categories for tabs
            foreach (var category in topCategories)
            {
                var categoryProducts = activeProducts.Where(p => p.CategoryId == category.Id)
                                                   .OrderByDescending(p => p.SalesCount)
                                                   .Take(12) // 12 products per category for grid
                                                   .ToList();
                viewModel.BestSellersByCategory[category.Name] = categoryProducts;
            }

            // 15. Suggest Today Products by Categories
            foreach (var category in topCategories)
            {
                var categoryProducts = activeProducts.Where(p => p.CategoryId == category.Id && (p.IsFeatured || p.AverageRating >= 4.0m))
                                                   .OrderByDescending(p => p.AverageRating)
                                                   .ThenByDescending(p => p.SalesCount)
                                                   .Take(12)
                                                   .ToList();
                viewModel.SuggestTodayByCategory[category.Name] = categoryProducts;
            }

            // 16. Just Landing Products by Categories (recently published products)
            foreach (var category in topCategories)
            {
                var categoryProducts = activeProducts.Where(p => p.CategoryId == category.Id && 
                                                               p.PublishedAt.HasValue && 
                                                               p.PublishedAt.Value >= DateTime.UtcNow.AddDays(-7)) // Last 7 days
                                                   .OrderByDescending(p => p.PublishedAt)
                                                   .Take(12)
                                                   .ToList();
                
                // If not enough new products, fill with latest products from category
                if (categoryProducts.Count < 12)
                {
                    var additionalCategoryProducts = activeProducts.Where(p => p.CategoryId == category.Id && !categoryProducts.Contains(p))
                                                                  .OrderByDescending(p => p.CreatedAt)
                                                                  .Take(12 - categoryProducts.Count)
                                                                  .ToList();
                    categoryProducts.AddRange(additionalCategoryProducts);
                }
                
                viewModel.JustLandingByCategory[category.Name] = categoryProducts;
            }
            
            _logger.LogInformation("Index page data loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading data for Index page");
            TempData["Error"] = "An error occurred while loading the page. Please try again.";
        }
        
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Resets all database data and re-seeds with fresh sample data
    /// This is a development/demo feature - access via /Home/ResetData
    /// </summary>
    public async Task<IActionResult> ResetData()
    {
        try
        {
            _logger.LogInformation("Database reset requested via web interface");
            
            // Reset and seed data
            await SeedData.SeedAsync(_serviceProvider, resetData: true);
            
            TempData["Success"] = "Database has been reset and seeded with fresh data successfully!";
            _logger.LogInformation("Database reset completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while resetting database data");
            TempData["Error"] = $"Error resetting database: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult SwooHome()
    {
        return View("Index");
    }

    public IActionResult Products()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Single_product()
    {
        return View();
    }

    public IActionResult Single_product_pay()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Checkout()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Cart()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
