using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            ICategoryService categoryService,
            IProductService productService,
            ILogger<ProfileController> logger)
        {
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Profile();
        }

        public async Task<IActionResult> Profile()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // User profile sections
                ViewBag.ProfileSections = new List<dynamic>
                {
                    new { Id = "personal", Title = "Personal Information", Icon = "las la-user", Active = true },
                    new { Id = "orders", Title = "Order History", Icon = "las la-shopping-bag", Active = false },
                    new { Id = "addresses", Title = "Addresses", Icon = "las la-map-marker", Active = false },
                    new { Id = "wishlist", Title = "Wishlist", Icon = "las la-heart", Active = false },
                    new { Id = "reviews", Title = "My Reviews", Icon = "las la-star", Active = false },
                    new { Id = "settings", Title = "Account Settings", Icon = "las la-cog", Active = false }
                };

                // Sample user data (in real app, get from auth context)
                ViewBag.User = new
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Phone = "+1 555 123 4567",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Gender = "Male",
                    JoinDate = new DateTime(2023, 1, 15),
                    ProfileImage = "~/swoo2/assets/img/avatar.jpg"
                };

                // Sample order history
                ViewBag.RecentOrders = new List<dynamic>
                {
                    new { 
                        OrderId = "ORD-2024-001", 
                        Date = DateTime.Now.AddDays(-5), 
                        Status = "Delivered", 
                        Total = 129.99m,
                        Items = 3
                    },
                    new { 
                        OrderId = "ORD-2024-002", 
                        Date = DateTime.Now.AddDays(-15), 
                        Status = "Shipped", 
                        Total = 89.50m,
                        Items = 2
                    },
                    new { 
                        OrderId = "ORD-2024-003", 
                        Date = DateTime.Now.AddDays(-30), 
                        Status = "Processing", 
                        Total = 249.99m,
                        Items = 5
                    }
                };

                // Sample addresses
                ViewBag.Addresses = new List<dynamic>
                {
                    new { 
                        Type = "Home", 
                        Address = "123 Main Street", 
                        City = "New York", 
                        State = "NY", 
                        ZipCode = "10001",
                        IsDefault = true
                    },
                    new { 
                        Type = "Work", 
                        Address = "456 Business Ave", 
                        City = "New York", 
                        State = "NY", 
                        ZipCode = "10002",
                        IsDefault = false
                    }
                };

                // Wishlist products (sample)
                var featuredProducts = await _productService.GetFeaturedProductsAsync(6);
                ViewBag.WishlistProducts = featuredProducts?.Where(p => p.IsActive).Take(4).ToList() ?? new List<Product>();

                // Recently viewed products
                var allProducts = await _productService.GetAllProductsAsync();
                ViewBag.RecentlyViewed = allProducts.Where(p => p.IsActive).Take(6).ToList();

                // Account statistics
                ViewBag.AccountStats = new
                {
                    TotalOrders = 15,
                    TotalSpent = 1250.75m,
                    RewardPoints = 2500,
                    ReviewsWritten = 8,
                    WishlistItems = 12
                };

                // Notification preferences
                ViewBag.NotificationPreferences = new List<dynamic>
                {
                    new { Type = "Order Updates", Enabled = true, Description = "Get notified about order status changes" },
                    new { Type = "Promotions", Enabled = false, Description = "Receive promotional offers and discounts" },
                    new { Type = "New Products", Enabled = true, Description = "Be the first to know about new arrivals" },
                    new { Type = "Price Drops", Enabled = true, Description = "Get alerts when wishlist items go on sale" }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading profile page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }
    }
} 