using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILogger<CartController> _logger;

        public CartController(
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            ILogger<CartController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _logger = logger;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            return await Cart();
        }

        // GET: /Cart/Cart
        public async Task<IActionResult> Cart()
        {
            try
            {
                await LoadCartViewData();
                
                // Get cart items from session or database
                var cartItems = GetCartItemsFromSession();
                ViewBag.CartItems = cartItems;
                
                // Calculate totals
                var subtotal = cartItems.Sum(item => item.Price * item.Quantity);
                var shipping = CalculateShipping(subtotal);
                var tax = CalculateTax(subtotal);
                var total = subtotal + shipping + tax;
                
                ViewBag.CartSummary = new
                {
                    Subtotal = subtotal,
                    Shipping = shipping,
                    Tax = tax,
                    Total = total,
                    ItemCount = cartItems.Sum(item => item.Quantity),
                    FreeShippingThreshold = 50.00m,
                    FreeShippingRemaining = Math.Max(0, 50.00m - subtotal)
                };

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart page");
                ViewBag.CartItems = new List<CartItemModel>();
                ViewBag.CartSummary = new { Subtotal = 0m, Shipping = 0m, Tax = 0m, Total = 0m, ItemCount = 0 };
                return View();
            }
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null || !product.IsActive)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                if (product.Stock < quantity)
                {
                    return Json(new { success = false, message = "Insufficient stock" });
                }

                // Add to session-based cart
                var cartItems = GetCartItemsFromSession();
                var existingItem = cartItems.FirstOrDefault(x => x.ProductId == productId);
                
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cartItems.Add(new CartItemModel
                    {
                        ProductId = productId,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = quantity,
                        ImageUrl = GetProductMainImage(product),
                        Stock = product.Stock
                    });
                }

                SaveCartItemsToSession(cartItems);

                var cartCount = cartItems.Sum(x => x.Quantity);
                return Json(new { success = true, message = "Product added to cart", cartCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product {ProductId} to cart", productId);
                return Json(new { success = false, message = "Error adding to cart" });
            }
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            try
            {
                var cartItems = GetCartItemsFromSession();
                var item = cartItems.FirstOrDefault(x => x.ProductId == productId);
                
                if (item == null)
                {
                    return Json(new { success = false, message = "Item not found in cart" });
                }

                if (quantity <= 0)
                {
                    cartItems.Remove(item);
                }
                else if (quantity <= item.Stock)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    return Json(new { success = false, message = "Insufficient stock" });
                }

                SaveCartItemsToSession(cartItems);

                var subtotal = cartItems.Sum(x => x.Price * x.Quantity);
                var cartCount = cartItems.Sum(x => x.Quantity);
                
                return Json(new { 
                    success = true, 
                    subtotal, 
                    cartCount,
                    itemTotal = item?.Price * item?.Quantity ?? 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart quantity for product {ProductId}", productId);
                return Json(new { success = false, message = "Error updating quantity" });
            }
        }

        // POST: /Cart/RemoveItem
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            try
            {
                var cartItems = GetCartItemsFromSession();
                var item = cartItems.FirstOrDefault(x => x.ProductId == productId);
                
                if (item != null)
                {
                    cartItems.Remove(item);
                    SaveCartItemsToSession(cartItems);
                }

                var subtotal = cartItems.Sum(x => x.Price * x.Quantity);
                var cartCount = cartItems.Sum(x => x.Quantity);
                
                return Json(new { success = true, subtotal, cartCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product {ProductId} from cart", productId);
                return Json(new { success = false, message = "Error removing item" });
            }
        }

        // POST: /Cart/ClearCart
        [HttpPost]
        public IActionResult ClearCart()
        {
            try
            {
                HttpContext.Session.Remove("CartItems");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return Json(new { success = false, message = "Error clearing cart" });
            }
        }

        private async Task LoadCartViewData()
        {
            try
            {
                // Categories for navigation
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories?.Where(c => c.IsActive).OrderBy(c => c.SortOrder).ToList() ?? new List<Category>();

                // Recommended products
                var featuredProducts = await _productService.GetFeaturedProductsAsync(6);
                ViewBag.RecommendedProducts = featuredProducts?.Where(p => p.IsActive).ToList() ?? new List<Product>();

                // Popular brands
                var brands = await _brandService.GetActiveBrandsAsync();
                ViewBag.PopularBrands = brands?.Take(6).ToList() ?? new List<Brand>();

                // Shipping options
                ViewBag.ShippingOptions = new List<dynamic>
                {
                    new { Name = "Standard Shipping", Price = 5.99m, DeliveryTime = "3-5 business days", Description = "Free on orders over $50" },
                    new { Name = "Express Shipping", Price = 12.99m, DeliveryTime = "1-2 business days", Description = "Fast delivery" },
                    new { Name = "Overnight Shipping", Price = 25.99m, DeliveryTime = "Next business day", Description = "Fastest option" }
                };

                // Payment methods
                ViewBag.PaymentMethods = new List<string>
                {
                    "Credit Card", "PayPal", "Apple Pay", "Google Pay", "Bank Transfer"
                };

                // Security badges
                ViewBag.SecurityBadges = new List<string>
                {
                    "SSL Secure", "Money Back Guarantee", "Fast Shipping", "24/7 Support"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart view data");
                ViewBag.Categories = new List<Category>();
                ViewBag.RecommendedProducts = new List<Product>();
                ViewBag.PopularBrands = new List<Brand>();
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

        private void SaveCartItemsToSession(List<CartItemModel> cartItems)
        {
            var cartItemsJson = System.Text.Json.JsonSerializer.Serialize(cartItems);
            HttpContext.Session.SetString("CartItems", cartItemsJson);
        }

        private string GetProductMainImage(Product product)
        {
            // Return first product image or default image
            return "~/swoo2/assets/img/products/prod1.png"; // Placeholder
        }

        private decimal CalculateShipping(decimal subtotal)
        {
            // Free shipping over $50
            return subtotal >= 50.00m ? 0 : 5.99m;
        }

        private decimal CalculateTax(decimal subtotal)
        {
            // 8% tax rate
            return Math.Round(subtotal * 0.08m, 2);
        }
    }

    // Cart item model for session storage
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
} 