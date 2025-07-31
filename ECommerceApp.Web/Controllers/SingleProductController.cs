using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class SingleProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ITagService _tagService;
        private readonly ILogger<SingleProductController> _logger;

        public SingleProductController(
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            ITagService tagService,
            ILogger<SingleProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _logger = logger;
        }

        // GET: /SingleProduct/Index/5 or /SingleProduct/5
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null || !product.IsActive)
                {
                    return NotFound();
                }

                await LoadProductViewData(product);
                return View("Single_product", product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID {ProductId}", id);
                return NotFound();
            }
        }

        // GET: /SingleProduct/Single_product/5
        public async Task<IActionResult> Single_product(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null || !product.IsActive)
                {
                    return NotFound();
                }

                await LoadProductViewData(product);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID {ProductId}", id);
                return NotFound();
            }
        }

        // GET: /SingleProduct/Single_product_pay/5
        public async Task<IActionResult> Single_product_pay(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null || !product.IsActive)
                {
                    return NotFound();
                }

                await LoadProductViewData(product);
                
                // Additional data for payment page
                ViewBag.PaymentMethods = new List<string> { "Credit Card", "PayPal", "Bank Transfer", "Cash on Delivery" };
                ViewBag.ShippingMethods = new List<dynamic>
                {
                    new { Name = "Standard Shipping", Price = 5.99m, DeliveryTime = "3-5 business days" },
                    new { Name = "Express Shipping", Price = 12.99m, DeliveryTime = "1-2 business days" },
                    new { Name = "Overnight Shipping", Price = 25.99m, DeliveryTime = "Next business day" }
                };
                
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product payment page for ID {ProductId}", id);
                return NotFound();
            }
        }

        // GET: /SingleProduct/Details/5 (alias for Index)
        public async Task<IActionResult> Details(int id)
        {
            return await Index(id);
        }

        private async Task LoadProductViewData(Product product)
        {
            try
            {
                // 1. Categories for navigation and breadcrumb
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();
                
                // 2. Current product category
                var currentCategory = categories?.FirstOrDefault(c => c.Id == product.CategoryId);
                ViewBag.CurrentCategory = currentCategory;
                
                // 3. Breadcrumb path
                var breadcrumbPath = new List<Category>();
                if (currentCategory != null)
                {
                    breadcrumbPath.Add(currentCategory);
                    // Add parent categories if hierarchical
                    var parent = currentCategory;
                    while (parent?.ParentId.HasValue == true)
                    {
                        parent = categories?.FirstOrDefault(c => c.Id == parent.ParentId.Value);
                        if (parent != null)
                        {
                            breadcrumbPath.Insert(0, parent);
                        }
                    }
                }
                ViewBag.BreadcrumbPath = breadcrumbPath;

                // 4. Related products from same category
                var relatedProducts = await _productService.GetProductsByCategoryAsync(product.CategoryId);
                ViewBag.RelatedProducts = relatedProducts?.Where(p => p.Id != product.Id && p.IsActive).Take(6).ToList() ?? new List<Product>();

                // 5. Similar products from same brand
                var similarProducts = new List<Product>();
                var allProductsForSimilar = await _productService.GetAllProductsAsync();
                if (product.BrandId.HasValue)
                {
                    similarProducts = allProductsForSimilar.Where(p => p.BrandId == product.BrandId && p.Id != product.Id && p.IsActive).Take(4).ToList();
                }
                ViewBag.SimilarProducts = similarProducts;

                // 6. Recently viewed products (you could store this in session/cookies)
                var allProducts = allProductsForSimilar;
                var recentlyViewedProducts = allProducts.Where(p => p.IsActive && p.Id != product.Id).Take(4).ToList();
                ViewBag.RecentlyViewedProducts = recentlyViewedProducts;

                // 7. Featured products for recommendations
                var featuredProducts = await _productService.GetFeaturedProductsAsync(6);
                ViewBag.FeaturedProducts = featuredProducts?.Where(p => p.Id != product.Id && p.IsActive).ToList() ?? new List<Product>();

                // 8. Product brand information
                Brand productBrand = null;
                if (product.BrandId.HasValue)
                {
                    productBrand = await _brandService.GetBrandByIdAsync(product.BrandId.Value);
                }
                ViewBag.ProductBrand = productBrand;

                // 9. Popular brands
                var brands = await _brandService.GetActiveBrandsAsync();
                ViewBag.PopularBrands = brands?.Take(6).ToList() ?? new List<Brand>();

                // 10. Product tags/labels
                var tags = await _tagService.GetActiveTagsAsync();
                ViewBag.Tags = tags?.ToList() ?? new List<Tag>();

                // 11. Product specifications (you can customize this based on category)
                ViewBag.ProductSpecs = GetProductSpecifications(product, currentCategory);

                // 12. Delivery and return information
                ViewBag.DeliveryInfo = new
                {
                    FreeShippingThreshold = 50.00m,
                    StandardDelivery = "3-5 business days",
                    ExpressDelivery = "1-2 business days",
                    ReturnPolicy = "30 days return policy",
                    Warranty = GetProductWarranty(product)
                };

                // 13. Stock and availability
                ViewBag.StockStatus = GetStockStatus(product);
                
                // 14. Price information
                ViewBag.PriceInfo = new
                {
                    CurrentPrice = product.Price,
                    ComparePrice = product.ComparePrice,
                    Discount = product.ComparePrice.HasValue && product.ComparePrice > product.Price ? 
                        Math.Round(((product.ComparePrice.Value - product.Price) / product.ComparePrice.Value) * 100, 0) : 0,
                    HasInstallment = product.HasInstallment,
                    InstallmentOptions = product.HasInstallment ? GetInstallmentOptions(product.Price) : null
                };

                // 15. Customer reviews summary (placeholder - you can implement actual reviews later)
                ViewBag.ReviewsSummary = new
                {
                    AverageRating = product.AverageRating,
                    TotalReviews = product.ReviewCount,
                    RatingDistribution = GetRatingDistribution(product.ReviewCount)
                };

                _logger.LogInformation("Product view data loaded successfully for product {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product view data for product {ProductId}", product.Id);
                
                // Provide fallback data
                ViewBag.Categories = new List<Category>();
                ViewBag.RelatedProducts = new List<Product>();
                ViewBag.SimilarProducts = new List<Product>();
                ViewBag.FeaturedProducts = new List<Product>();
                ViewBag.PopularBrands = new List<Brand>();
                ViewBag.Tags = new List<Tag>();
            }
        }

        private Dictionary<string, object> GetProductSpecifications(Product product, Category category)
        {
            var specs = new Dictionary<string, object>();
            
            // Base specifications
            specs["SKU"] = product.SKU ?? "N/A";
            specs["Brand"] = ViewBag.ProductBrand?.Name ?? "N/A";
            specs["Category"] = category?.Name ?? "N/A";
            
            // Category-specific specifications (you can expand this)
            if (category?.Name.ToLower().Contains("electronics") == true)
            {
                specs["Warranty"] = "1 Year Manufacturer Warranty";
                specs["Power"] = "AC 100-240V";
            }
            else if (category?.Name.ToLower().Contains("computer") == true)
            {
                specs["Processor"] = "Intel Core i5";
                specs["Memory"] = "8GB RAM";
                specs["Storage"] = "256GB SSD";
            }
            
            return specs;
        }

        private string GetProductWarranty(Product product)
        {
            // You can implement warranty logic based on product category or brand
            return "1 Year Manufacturer Warranty";
        }

        private string GetStockStatus(Product product)
        {
            if (product.Stock <= 0)
                return "Out of Stock";
            else if (product.Stock <= 5)
                return $"Only {product.Stock} left in stock!";
            else if (product.Stock <= 20)
                return "Limited Stock";
            else
                return "In Stock";
        }

        private List<dynamic> GetInstallmentOptions(decimal price)
        {
            return new List<dynamic>
            {
                new { Months = 3, MonthlyPayment = Math.Round(price / 3, 2) },
                new { Months = 6, MonthlyPayment = Math.Round(price / 6, 2) },
                new { Months = 12, MonthlyPayment = Math.Round(price / 12, 2) }
            };
        }

        private Dictionary<int, int> GetRatingDistribution(int totalReviews)
        {
            // Placeholder rating distribution - you can implement actual logic
            return new Dictionary<int, int>
            {
                { 5, (int)(totalReviews * 0.6) },
                { 4, (int)(totalReviews * 0.2) },
                { 3, (int)(totalReviews * 0.1) },
                { 2, (int)(totalReviews * 0.05) },
                { 1, (int)(totalReviews * 0.05) }
            };
        }
    }
} 