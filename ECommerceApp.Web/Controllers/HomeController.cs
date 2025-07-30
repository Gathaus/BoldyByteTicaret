using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Web.Models;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryService _categoryService;

    public HomeController(ILogger<HomeController> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _categoryService.GetActiveTopLevelCategoriesAsync();
            ViewBag.Categories = categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories for Index page");
            ViewBag.Categories = new List<ECommerceApp.Domain.Entities.Category>();
        }
        
        return View();
    }
    
    public IActionResult About()
    {
        return View();
    }

    public IActionResult SwooHome()
    {
        return View("Index");
    }

    public IActionResult Privacy()
    {
        return View();
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
