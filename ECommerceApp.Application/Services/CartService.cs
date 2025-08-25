using ECommerceApp.Domain.Services;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ECommerceApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;

        public CartService(IHttpContextAccessor httpContextAccessor, IProductService productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public async Task<int> GetCartItemCountAsync(string sessionId)
        {
            var cartItems = GetCartItemsFromSession();
            return cartItems.Sum(x => x.Quantity);
        }

        public async Task<decimal> GetCartTotalAsync(string sessionId)
        {
            var cartItems = GetCartItemsFromSession();
            return cartItems.Sum(x => x.Price * x.Quantity);
        }

        public async Task<bool> AddToCartAsync(string sessionId, int productId, int quantity)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null || !product.IsActive || product.Stock < quantity)
                {
                    return false;
                }

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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCartItemAsync(string sessionId, int productId, int quantity)
        {
            try
            {
                var cartItems = GetCartItemsFromSession();
                var item = cartItems.FirstOrDefault(x => x.ProductId == productId);

                if (item == null) return false;

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
                    return false;
                }

                SaveCartItemsToSession(cartItems);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromCartAsync(string sessionId, int productId)
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

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task ClearCartAsync(string sessionId)
        {
            _httpContextAccessor.HttpContext?.Session.Remove("CartItems");
        }

        private List<CartItemModel> GetCartItemsFromSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return new List<CartItemModel>();

            var cartItemsJson = session.GetString("CartItems");
            if (string.IsNullOrEmpty(cartItemsJson))
            {
                return new List<CartItemModel>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<CartItemModel>>(cartItemsJson) ?? new List<CartItemModel>();
            }
            catch
            {
                return new List<CartItemModel>();
            }
        }

        private void SaveCartItemsToSession(List<CartItemModel> cartItems)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return;

            var cartItemsJson = JsonSerializer.Serialize(cartItems);
            session.SetString("CartItems", cartItemsJson);
        }

        private string GetProductMainImage(Domain.Entities.Product product)
        {
            // Return first product image or default image
            return "~/swoo2/assets/img/products/prod1.png"; // Placeholder
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
