using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(
            ICategoryService categoryService,
            ILogger<ContactController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Contact();
        }

        public async Task<IActionResult> Contact()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Contact information
                ViewBag.ContactInfo = new
                {
                    Phone = "(025) 3886 25 16",
                    Email = "info@swoo-ecommerce.com",
                    Address = "123 E-Commerce Street, Digital City, DC 12345",
                    WorkingHours = "Monday - Friday: 9:00 AM - 6:00 PM",
                    SupportHours = "24/7 Customer Support Available"
                };

                // Office locations
                ViewBag.OfficeLocations = new List<dynamic>
                {
                    new { 
                        City = "New York", 
                        Address = "123 Broadway, New York, NY 10001", 
                        Phone = "+1 (555) 123-4567",
                        Email = "ny@swoo-ecommerce.com"
                    },
                    new { 
                        City = "London", 
                        Address = "456 Oxford Street, London, UK W1C 1AP", 
                        Phone = "+44 20 7123 4567",
                        Email = "london@swoo-ecommerce.com"
                    },
                    new { 
                        City = "Tokyo", 
                        Address = "789 Shibuya, Tokyo, Japan 150-0002", 
                        Phone = "+81 3 1234 5678",
                        Email = "tokyo@swoo-ecommerce.com"
                    }
                };

                // FAQ
                ViewBag.FAQ = new List<dynamic>
                {
                    new { Question = "How can I track my order?", Answer = "You can track your order using the tracking number sent to your email after shipment." },
                    new { Question = "What is your return policy?", Answer = "We offer a 30-day return policy for all unused items in original packaging." },
                    new { Question = "Do you ship internationally?", Answer = "Yes, we ship to over 50 countries worldwide." },
                    new { Question = "How can I contact customer support?", Answer = "You can reach us via phone, email, or live chat 24/7." }
                };

                // Contact subjects/categories
                ViewBag.ContactSubjects = new List<string>
                {
                    "General Inquiry",
                    "Order Support", 
                    "Product Information",
                    "Returns & Refunds",
                    "Shipping Information",
                    "Technical Support",
                    "Partnership & Business",
                    "Media & Press"
                };

                // Social media links
                ViewBag.SocialMedia = new List<dynamic>
                {
                    new { Platform = "Facebook", Icon = "fab fa-facebook-f", Url = "https://facebook.com/swoo" },
                    new { Platform = "Twitter", Icon = "fab fa-twitter", Url = "https://twitter.com/swoo" },
                    new { Platform = "Instagram", Icon = "fab fa-instagram", Url = "https://instagram.com/swoo" },
                    new { Platform = "LinkedIn", Icon = "fab fa-linkedin-in", Url = "https://linkedin.com/company/swoo" },
                    new { Platform = "YouTube", Icon = "fab fa-youtube", Url = "https://youtube.com/swoo" }
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading contact page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload page data and redisplay form
                await Contact();
                return View("Contact", model);
            }

            try
            {
                // Here you would typically send the email or save to database
                // For now, we'll just simulate success
                _logger.LogInformation("Contact form submitted by {Email} with subject {Subject}", model.Email, model.Subject);
                
                TempData["Success"] = "Thank you for your message! We'll get back to you within 24 hours.";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contact form submission");
                ModelState.AddModelError(string.Empty, "An error occurred while sending your message. Please try again.");
                
                await Contact();
                return View("Contact", model);
            }
        }
    }

    public class ContactFormModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Subject { get; set; }
        public required string Message { get; set; }
        public string? Phone { get; set; }
    }
} 