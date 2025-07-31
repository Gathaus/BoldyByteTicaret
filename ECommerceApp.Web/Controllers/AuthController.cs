using System;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using ECommerceApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(
            IAuthService authService,
            ICategoryService categoryService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _categoryService = categoryService;
            _logger = logger;
        }
        
        // GET: /Auth/Login
        public async Task<IActionResult> Login()
        {
            try
            {
                // Categories for header/footer navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading login page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }
        
        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                var result = await _authService.LoginAsync(model.Email, model.Password);
                if (result.success)
                {
                    // Store JWT token in session or cookie if needed
                    HttpContext.Session.SetString("AuthToken", result.token);
                    
                    // Redirect to home page or returnUrl
                    var returnUrl = Request.Query["returnUrl"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
            }
            
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        // GET: /Auth/Register
        public async Task<IActionResult> Register()
        {
            try
            {
                // Categories for header/footer navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading register page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }
        
        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            
            try
            {
                var result = await _authService.RegisterAsync(user, model.Password);
                if (result)
                {
                    TempData["Success"] = "Registration successful! Please log in.";
                    return RedirectToAction("Login");
                }
                
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
            }
            
            return View(model);
        }
        
        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            try
            {
                // Clear authentication token from session
                HttpContext.Session.Remove("AuthToken");
                
                // If using cookie authentication, sign out
                // await HttpContext.SignOutAsync();
                
                TempData["Success"] = "You have been logged out successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
            }
            
            return RedirectToAction("Index", "Home");
        }
        
        // GET: /Auth/ForgotPassword
        public async Task<IActionResult> ForgotPassword()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading forgot password page");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }
        
        // POST: /Auth/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                // Implement password reset logic here
                // await _authService.SendPasswordResetEmailAsync(model.Email);
                
                TempData["Success"] = "If an account with that email exists, we have sent a password reset link.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset for email {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
            }
            
            return View(model);
        }
    }
} 