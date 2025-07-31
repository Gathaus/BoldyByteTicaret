using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class PrivacyController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<PrivacyController> _logger;

        public PrivacyController(
            ICategoryService categoryService,
            ILogger<PrivacyController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Privacy();
        }

        public async Task<IActionResult> Privacy()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Privacy policy sections
                ViewBag.PrivacySections = new List<dynamic>
                {
                    new { 
                        Id = "information-collection", 
                        Title = "Information We Collect", 
                        Content = "We collect information you provide directly to us, such as when you create an account, make a purchase, or contact us for support."
                    },
                    new { 
                        Id = "information-use", 
                        Title = "How We Use Your Information", 
                        Content = "We use the information we collect to provide, maintain, and improve our services, process transactions, and communicate with you."
                    },
                    new { 
                        Id = "information-sharing", 
                        Title = "Information Sharing", 
                        Content = "We do not sell, trade, or rent your personal information to third parties. We may share information in certain circumstances as outlined in this policy."
                    },
                    new { 
                        Id = "data-security", 
                        Title = "Data Security", 
                        Content = "We implement appropriate security measures to protect your personal information against unauthorized access, alteration, disclosure, or destruction."
                    },
                    new { 
                        Id = "cookies", 
                        Title = "Cookies and Tracking", 
                        Content = "We use cookies and similar technologies to enhance your browsing experience, analyze site traffic, and personalize content."
                    },
                    new { 
                        Id = "your-rights", 
                        Title = "Your Rights", 
                        Content = "You have the right to access, update, or delete your personal information. You may also opt out of certain communications."
                    }
                };

                // Cookie information
                ViewBag.CookieInfo = new
                {
                    Essential = new List<string> { "Session cookies", "Security cookies", "Authentication cookies" },
                    Analytics = new List<string> { "Google Analytics", "Site performance tracking" },
                    Marketing = new List<string> { "Advertising cookies", "Social media integration" },
                    Preferences = new List<string> { "Language settings", "Theme preferences", "Shopping preferences" }
                };

                // Data protection principles
                ViewBag.DataProtectionPrinciples = new List<dynamic>
                {
                    new { Icon = "las la-lock", Title = "Data Minimization", Description = "We only collect data that is necessary for our services." },
                    new { Icon = "las la-shield-alt", Title = "Purpose Limitation", Description = "We use data only for the purposes we've disclosed to you." },
                    new { Icon = "las la-clock", Title = "Storage Limitation", Description = "We retain data only as long as necessary for our services." },
                    new { Icon = "las la-user-check", Title = "Accuracy", Description = "We strive to keep your data accurate and up-to-date." }
                };

                // Contact information for privacy concerns
                ViewBag.PrivacyContact = new
                {
                    Email = "privacy@swoo-ecommerce.com",
                    Phone = "(025) 3886 25 16",
                    Address = "123 E-Commerce Street, Digital City, DC 12345",
                    DataProtectionOfficer = "dpo@swoo-ecommerce.com"
                };

                // GDPR/CCPA compliance info
                ViewBag.ComplianceInfo = new
                {
                    GDPR = new { Applies = true, Region = "European Union", Description = "We comply with GDPR requirements for EU residents." },
                    CCPA = new { Applies = true, Region = "California", Description = "We comply with CCPA requirements for California residents." },
                    LastUpdated = new DateTime(2024, 1, 1),
                    Version = "2.1"
                };

                // Related pages
                ViewBag.RelatedPages = new List<dynamic>
                {
                    new { Title = "Terms of Service", Url = "/Legal/Terms" },
                    new { Title = "Cookie Policy", Url = "/Legal/Cookies" },
                    new { Title = "Return Policy", Url = "/Legal/Returns" },
                    new { Title = "Security Policy", Url = "/Legal/Security" }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading privacy page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }

        public async Task<IActionResult> Terms()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Terms of service sections
                ViewBag.TermsSections = new List<dynamic>
                {
                    new { 
                        Id = "acceptance", 
                        Title = "Acceptance of Terms", 
                        Content = "By using our website and services, you agree to be bound by these Terms of Service."
                    },
                    new { 
                        Id = "services", 
                        Title = "Description of Services", 
                        Content = "We provide an e-commerce platform for buying and selling products online."
                    },
                    new { 
                        Id = "user-accounts", 
                        Title = "User Accounts", 
                        Content = "You are responsible for maintaining the confidentiality of your account credentials."
                    },
                    new { 
                        Id = "payments", 
                        Title = "Payment Terms", 
                        Content = "All purchases are subject to our payment terms and conditions."
                    },
                    new { 
                        Id = "shipping", 
                        Title = "Shipping and Delivery", 
                        Content = "Shipping times and costs vary based on your location and selected shipping method."
                    },
                    new { 
                        Id = "returns", 
                        Title = "Returns and Refunds", 
                        Content = "We offer a 30-day return policy for most items in original condition."
                    }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading terms page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }
    }
} 