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
    public class AccountController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            ICategoryService categoryService,
            IProductService productService,
            IBrandService brandService,
            ILogger<AccountController> logger)
        {
            _categoryService = categoryService;
            _productService = productService;
            _brandService = brandService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Account dashboard sections
                ViewBag.DashboardSections = new List<dynamic>
                {
                    new { 
                        Title = "My Orders", 
                        Icon = "las la-shopping-bag", 
                        Count = 12, 
                        Url = "/Account/Orders",
                        Description = "View your order history"
                    },
                    new { 
                        Title = "Wishlist", 
                        Icon = "las la-heart", 
                        Count = 8, 
                        Url = "/Account/Wishlist",
                        Description = "Your favorite items"
                    },
                    new { 
                        Title = "Address Book", 
                        Icon = "las la-map-marker", 
                        Count = 2, 
                        Url = "/Account/Addresses",
                        Description = "Manage shipping addresses"
                    },
                    new { 
                        Title = "Payment Methods", 
                        Icon = "las la-credit-card", 
                        Count = 3, 
                        Url = "/Account/PaymentMethods",
                        Description = "Manage payment options"
                    }
                };

                // Recent activity
                ViewBag.RecentActivity = new List<dynamic>
                {
                    new { 
                        Date = DateTime.Now.AddDays(-1), 
                        Action = "Order Placed", 
                        Description = "Order #ORD-2024-001 for $129.99",
                        Icon = "las la-shopping-cart"
                    },
                    new { 
                        Date = DateTime.Now.AddDays(-3), 
                        Action = "Product Reviewed", 
                        Description = "Reviewed 'Wireless Headphones'",
                        Icon = "las la-star"
                    },
                    new { 
                        Date = DateTime.Now.AddDays(-7), 
                        Action = "Wishlist Updated", 
                        Description = "Added 'Smartphone' to wishlist",
                        Icon = "las la-heart"
                    }
                };

                // Account overview stats
                ViewBag.AccountStats = new
                {
                    TotalOrders = 15,
                    TotalSpent = 1450.75m,
                    SavedItems = 8,
                    RewardPoints = 2850,
                    NextRewardLevel = 5000,
                    MemberSince = new DateTime(2023, 1, 15)
                };

                // Recommended products based on purchase history
                var featuredProducts = await _productService.GetFeaturedProductsAsync(6);
                ViewBag.RecommendedProducts = featuredProducts?.Where(p => p.IsActive).Take(4).ToList() ?? new List<Product>();

                // Quick actions
                ViewBag.QuickActions = new List<dynamic>
                {
                    new { Title = "Track Order", Icon = "las la-search", Url = "/Account/TrackOrder" },
                    new { Title = "Reorder", Icon = "las la-redo", Url = "/Account/Reorder" },
                    new { Title = "Return Item", Icon = "las la-undo", Url = "/Account/Returns" },
                    new { Title = "Update Profile", Icon = "las la-user-edit", Url = "/Profile" }
                };

                // Special offers for logged-in users
                ViewBag.PersonalOffers = new List<dynamic>
                {
                    new { 
                        Title = "Member Exclusive: 20% Off Electronics", 
                        Description = "Special discount for premium members",
                        ExpiryDate = DateTime.Now.AddDays(7),
                        Code = "MEMBER20"
                    },
                    new { 
                        Title = "Free Shipping on Next Order", 
                        Description = "No minimum purchase required",
                        ExpiryDate = DateTime.Now.AddDays(14),
                        Code = "FREESHIP"
                    }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading account dashboard");
                ViewBag.Categories = new List<Category>();
                ViewBag.RecommendedProducts = new List<Product>();
                return View();
            }
        }

        public async Task<IActionResult> Orders()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Sample order history
                ViewBag.Orders = new List<dynamic>
                {
                    new { 
                        OrderId = "ORD-2024-001", 
                        Date = DateTime.Now.AddDays(-2), 
                        Status = "Shipped", 
                        Total = 129.99m,
                        Items = new List<dynamic>
                        {
                            new { Name = "Wireless Headphones", Quantity = 1, Price = 99.99m },
                            new { Name = "Phone Case", Quantity = 1, Price = 19.99m },
                            new { Name = "Shipping", Quantity = 1, Price = 10.01m }
                        },
                        TrackingNumber = "1Z999AA1234567890"
                    },
                    new { 
                        OrderId = "ORD-2024-002", 
                        Date = DateTime.Now.AddDays(-15), 
                        Status = "Delivered", 
                        Total = 89.50m,
                        Items = new List<dynamic>
                        {
                            new { Name = "Smart Watch", Quantity = 1, Price = 79.99m },
                            new { Name = "Shipping", Quantity = 1, Price = 9.51m }
                        },
                        TrackingNumber = "1Z999AA1234567891"
                    }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders page");
                ViewBag.Categories = new List<Category>();
                ViewBag.Orders = new List<dynamic>();
                return View();
            }
        }

        public async Task<IActionResult> Wishlist()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Wishlist products
                var featuredProducts = await _productService.GetFeaturedProductsAsync(8);
                ViewBag.WishlistProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();

                // Wishlist stats
                ViewBag.WishlistStats = new
                {
                    TotalItems = 8,
                    TotalValue = 850.45m,
                    OnSaleItems = 2,
                    AvailableItems = 7
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading wishlist page");
                ViewBag.Categories = new List<Category>();
                ViewBag.WishlistProducts = new List<Product>();
                return View();
            }
        }
    }
} 