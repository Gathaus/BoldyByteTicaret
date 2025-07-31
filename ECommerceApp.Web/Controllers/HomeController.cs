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
    private readonly IServiceProvider _serviceProvider;

    public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _categoryService = categoryService;
        _serviceProvider = serviceProvider;
    }

    public async Task<IActionResult> Index()
    {
        // Pass categories to the view for the category section
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Categories = categories?.ToList() ?? new List<Category>();
        
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
