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
        try
        {
            // 1. Categories for Popular Categories section
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();
            
            // 2. Featured Products for Best Seller section
            var featuredProducts = await _productService.GetFeaturedProductsAsync(20);
            ViewBag.FeaturedProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();
            
            // 3. All Products for different sections
            var allProducts = await _productService.GetAllProductsAsync();
            var activeProducts = allProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();
            
            // 4. Best Seller Products (products with highest sales)
            var bestSellerProducts = activeProducts.OrderByDescending(p => p.SalesCount).Take(10).ToList();
            ViewBag.BestSellerProducts = bestSellerProducts;
            
            // 5. Popular Brands
            var brands = await _brandService.GetPopularBrandsAsync(6);
            ViewBag.PopularBrands = brands?.ToList() ?? new List<Brand>();
            
            // 6. Suggest Today Products (featured + top rated)
            var suggestTodayProducts = activeProducts.Where(p => p.IsFeatured || p.AverageRating >= 4).Take(15).ToList();
            ViewBag.SuggestTodayProducts = suggestTodayProducts;
            
            // 7. Top Rated Products
            var topRatedProducts = activeProducts.Where(p => p.AverageRating >= 4).OrderByDescending(p => p.AverageRating).Take(10).ToList();
            ViewBag.TopRatedProducts = topRatedProducts;
            
            // 8. Discount Products (products with compare price)
            var discountProducts = activeProducts.Where(p => p.ComparePrice > p.Price).Take(10).ToList();
            ViewBag.DiscountProducts = discountProducts;
            
            // 9. New Products (recently published)
            var newProducts = activeProducts.Where(p => p.PublishedAt.HasValue && p.PublishedAt.Value >= DateTime.UtcNow.AddDays(-30))
                                          .OrderByDescending(p => p.PublishedAt).Take(10).ToList();
            ViewBag.NewProducts = newProducts;
            
            // 10. Tags for labels
            var tags = await _tagService.GetActiveTagsAsync();
            ViewBag.Tags = tags?.ToList() ?? new List<Tag>();
            
            // 11. Categories for Best Seller tabs (limit to main categories)
            var mainCategories = categories?.Where(c => c.IsActive && !c.ParentId.HasValue).Take(5).ToList() ?? new List<Category>();
            ViewBag.MainCategories = mainCategories;
            
            // 12. Trending Search Keywords (you can customize these)
            ViewBag.TrendingSearches = new List<string>
            {
                "Vacuum Robot", "Bluetooth Speaker", "Oled TV", "Security Camera", 
                "Macbook M1", "Smart Washing Machine", "iPad Mini 2023", "PS5", 
                "Earbuds", "Air Condition Inverter", "Flycam", "Electric Bike",
                "Gaming Computer", "Smart Air Purifier", "Apple Watch"
            };
            
            _logger.LogInformation("Index page data loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading data for Index page");
            
            // Provide empty data to prevent view errors
            ViewBag.Categories = new List<Category>();
            ViewBag.FeaturedProducts = new List<Product>();
            ViewBag.BestSellerProducts = new List<Product>();
            ViewBag.PopularBrands = new List<Brand>();
            ViewBag.SuggestTodayProducts = new List<Product>();
            ViewBag.TopRatedProducts = new List<Product>();
            ViewBag.DiscountProducts = new List<Product>();
            ViewBag.NewProducts = new List<Product>();
            ViewBag.Tags = new List<Tag>();
            ViewBag.MainCategories = new List<Category>();
            ViewBag.TrendingSearches = new List<string>();
        }
        
        return View();
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
