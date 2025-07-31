using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class AboutController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;
        private readonly ILogger<AboutController> _logger;

        public AboutController(
            ICategoryService categoryService,
            IBrandService brandService,
            IProductService productService,
            ILogger<AboutController> logger)
        {
            _categoryService = categoryService;
            _brandService = brandService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await About();
        }

        public async Task<IActionResult> About()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Company information
                ViewBag.CompanyInfo = new
                {
                    Name = "Swoo E-Commerce",
                    FoundedYear = 2020,
                    Employees = "500+",
                    Customers = "10,000+",
                    Countries = "50+",
                    Mission = "To provide the best online shopping experience with quality products and exceptional service.",
                    Vision = "To be the leading e-commerce platform globally, connecting people with the products they love."
                };

                // Team members
                ViewBag.TeamMembers = new List<dynamic>
                {
                    new { Name = "John Smith", Position = "CEO & Founder", Image = "~/swoo2/assets/img/team/2.jpg", Bio = "10+ years in e-commerce" },
                    new { Name = "Sarah Johnson", Position = "CTO", Image = "~/swoo2/assets/img/team/4.jpg", Bio = "Expert in technology solutions" },
                    new { Name = "Mike Brown", Position = "Head of Sales", Image = "~/swoo2/assets/img/team/2.jpg", Bio = "15+ years in retail" },
                    new { Name = "Lisa Davis", Position = "Marketing Director", Image = "~/swoo2/assets/img/team/4.jpg", Bio = "Digital marketing specialist" }
                };

                // Company statistics
                ViewBag.Statistics = new List<dynamic>
                {
                    new { Icon = "las la-users", Number = "10,000+", Label = "Happy Customers" },
                    new { Icon = "las la-box", Number = "50,000+", Label = "Products Sold" },
                    new { Icon = "las la-globe", Number = "50+", Label = "Countries Served" },
                    new { Icon = "las la-star", Number = "4.8", Label = "Average Rating" }
                };

                // Our values
                ViewBag.Values = new List<dynamic>
                {
                    new { Icon = "las la-heart", Title = "Customer First", Description = "We put our customers at the center of everything we do." },
                    new { Icon = "las la-shield-alt", Title = "Quality Assurance", Description = "We ensure all products meet our high-quality standards." },
                    new { Icon = "las la-shipping-fast", Title = "Fast Delivery", Description = "Quick and reliable shipping to your doorstep." },
                    new { Icon = "las la-headset", Title = "24/7 Support", Description = "Round-the-clock customer support for all your needs." }
                };

                // Partner brands
                var brands = await _brandService.GetActiveBrandsAsync();
                ViewBag.PartnerBrands = brands?.Take(8).ToList() ?? new List<Brand>();

                // Featured products
                var featuredProducts = await _productService.GetFeaturedProductsAsync(6);
                ViewBag.FeaturedProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();

                // Timeline
                ViewBag.Timeline = new List<dynamic>
                {
                    new { Year = "2020", Title = "Company Founded", Description = "Started with a vision to revolutionize online shopping." },
                    new { Year = "2021", Title = "First 1000 Customers", Description = "Reached our first milestone of satisfied customers." },
                    new { Year = "2022", Title = "International Expansion", Description = "Expanded to serve customers in 20+ countries." },
                    new { Year = "2023", Title = "Mobile App Launch", Description = "Launched our mobile application for better accessibility." },
                    new { Year = "2024", Title = "50+ Countries", Description = "Now serving customers in over 50 countries worldwide." }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading about page");
                ViewBag.Categories = new List<Category>();
                ViewBag.PartnerBrands = new List<Brand>();
                ViewBag.FeaturedProducts = new List<Product>();
                return View();
            }
        }
    }
} 