using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            ILogger<CheckoutController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Checkout();
        }

        public async Task<IActionResult> Checkout()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Get cart items from session
                var cartItems = GetCartItemsFromSession();
                ViewBag.CartItems = cartItems;

                // Calculate order summary
                var subtotal = cartItems.Sum(item => item.Price * item.Quantity);
                var shipping = CalculateShipping(subtotal);
                var tax = CalculateTax(subtotal);
                var total = subtotal + shipping + tax;

                ViewBag.OrderSummary = new
                {
                    Subtotal = subtotal,
                    Shipping = shipping,
                    Tax = tax,
                    Total = total,
                    ItemCount = cartItems.Sum(item => item.Quantity)
                };

                // Shipping options
                ViewBag.ShippingOptions = new List<dynamic>
                {
                    new { Id = 1, Name = "Standard Shipping", Price = 5.99m, DeliveryTime = "3-5 business days", Description = "Free on orders over $50" },
                    new { Id = 2, Name = "Express Shipping", Price = 12.99m, DeliveryTime = "1-2 business days", Description = "Fast delivery" },
                    new { Id = 3, Name = "Overnight Shipping", Price = 25.99m, DeliveryTime = "Next business day", Description = "Fastest option" }
                };

                // Payment methods
                ViewBag.PaymentMethods = new List<dynamic>
                {
                    new { Id = 1, Name = "Credit Card", Icon = "fas fa-credit-card", Accepted = new[] { "Visa", "MasterCard", "American Express" } },
                    new { Id = 2, Name = "PayPal", Icon = "fab fa-paypal", Description = "Pay securely with PayPal" },
                    new { Id = 3, Name = "Apple Pay", Icon = "fab fa-apple-pay", Description = "Pay with Touch ID or Face ID" },
                    new { Id = 4, Name = "Google Pay", Icon = "fab fa-google-pay", Description = "Pay with Google" },
                    new { Id = 5, Name = "Bank Transfer", Icon = "fas fa-university", Description = "Direct bank transfer" }
                };

                // Countries for shipping address
                ViewBag.Countries = new List<string>
                {
                    "United States", "Canada", "United Kingdom", "Germany", "France", "Italy", "Spain", "Australia", "Japan", "Turkey"
                };

                // Security features
                ViewBag.SecurityFeatures = new List<string>
                {
                    "256-bit SSL Encryption", "PCI DSS Compliant", "Money Back Guarantee", "Secure Payment Processing"
                };

                // Recommended products
                var featuredProducts = await _productService.GetFeaturedProductsAsync(4);
                ViewBag.RecommendedProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading checkout page");
                ViewBag.Categories = new List<Category>();
                ViewBag.CartItems = new List<CartItemModel>();
                ViewBag.OrderSummary = new { Subtotal = 0m, Shipping = 0m, Tax = 0m, Total = 0m, ItemCount = 0 };
                return View();
            }
        }

        private List<CartItemModel> GetCartItemsFromSession()
        {
            var cartItemsJson = HttpContext.Session.GetString("CartItems");
            if (string.IsNullOrEmpty(cartItemsJson))
            {
                return new List<CartItemModel>();
            }

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<CartItemModel>>(cartItemsJson) ?? new List<CartItemModel>();
            }
            catch
            {
                return new List<CartItemModel>();
            }
        }

        private decimal CalculateShipping(decimal subtotal)
        {
            return subtotal >= 50.00m ? 0 : 5.99m;
        }

        private decimal CalculateTax(decimal subtotal)
        {
            return Math.Round(subtotal * 0.08m, 2);
        }
    }
} 