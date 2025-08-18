using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Domain.Services;
using ECommerceApp.Web.Models;

namespace ECommerceApp.Web.Controllers;

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

    public async Task<IActionResult> Index(ProductFilterModel filter)
    {
        try
        {
            var viewModel = new ProductsViewModel
            {
                Style = filter.Style,
                SelectedFinishes = filter.Finishes ?? new List<string>(),
                SelectedColors = filter.Colors ?? new List<string>(),
                SelectedRooms = filter.Rooms ?? new List<string>(),
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                SortBy = filter.SortBy,
                Page = Math.Max(1, filter.Page),
                PageSize = Math.Max(1, filter.PageSize)
            };

            // Get filtered products
            var (products, totalCount) = await _productService.GetFilteredProductsAsync(
                filter.Style,
                filter.Finishes,
                filter.Colors,
                filter.Rooms,
                filter.MinPrice,
                filter.MaxPrice,
                filter.SortBy,
                null, // searchTerm
                filter.Page,
                filter.PageSize
            );

            viewModel.Products = products;
            viewModel.TotalProducts = totalCount;

            // Get categories for filter
            var categories = await _categoryService.GetAllCategoriesAsync();
            viewModel.Categories = categories.ToList();

            // Get popular categories for slider
            var popularCategories = categories
                .Where(c => c.IsActive && !c.ParentId.HasValue) // Ana kategoriler
                .OrderByDescending(c => c.Products.Count) // Ürün sayısına göre sırala
                .Take(6) // En popüler 6 kategori
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    ProductCount = c.Products.Count
                })
                .ToList();
            ViewBag.PopularCategories = popularCategories;

            // Get brands for filter
            var brands = await _brandService.GetAllBrandsAsync();
            viewModel.Brands = brands.ToList();

            // Get tags for filter
            var tags = await _tagService.GetActiveTagsAsync();
            viewModel.Tags = tags.ToList();

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading products page");
            TempData["Error"] = "An error occurred while loading the products. Please try again.";
            return View(new ProductsViewModel());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            // Get related products
            var relatedProducts = await _productService.GetRelatedProductsAsync(id);

            // You can create a separate view model for product details if needed
            return View(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading product details");
            TempData["Error"] = "An error occurred while loading the product details. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        try
        {
            var (products, totalCount) = await _productService.GetFilteredProductsAsync(
                searchTerm: query,
                page: 1,
                pageSize: 12
            );

            return Json(new { success = true, products, totalCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching products");
            return Json(new { success = false, message = "An error occurred while searching products." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetNewArrivals()
    {
        try
        {
            var products = await _productService.GetNewArrivalsAsync();
            return Json(new { success = true, products });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting new arrivals");
            return Json(new { success = false, message = "An error occurred while getting new arrivals." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBestSellers()
    {
        try
        {
            var products = await _productService.GetBestSellersAsync();
            return Json(new { success = true, products });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting best sellers");
            return Json(new { success = false, message = "An error occurred while getting best sellers." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetDiscountedProducts()
    {
        try
        {
            var products = await _productService.GetDiscountedProductsAsync();
            return Json(new { success = true, products });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting discounted products");
            return Json(new { success = false, message = "An error occurred while getting discounted products." });
        }
    }
}