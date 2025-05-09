using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        
        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            // Get existing cart or create a new one
            var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }
            
            return cart;
        }
        
        public async Task<CartItem> AddItemToCartAsync(string userId, int productId, int quantity)
        {
            // Validate quantity
            if (quantity <= 0)
            {
                throw new Exception("Quantity must be greater than zero.");
            }
            
            // Verify product exists
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new Exception($"Product with id {productId} not found.");
            }
            
            // Check stock
            if (product.Stock < quantity)
            {
                throw new Exception("Not enough product in stock.");
            }
            
            // Get or create cart
            var cart = await GetCartByUserIdAsync(userId);
            
            // Check if item already exists in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += quantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
                cart.UpdatedAt = DateTime.UtcNow;
                
                _cartRepository.Update(cart);
                await _cartRepository.SaveChangesAsync();
                
                return existingItem;
            }
            
            // Add new item
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            cart.CartItems.Add(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;
            
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();
            
            return cartItem;
        }
        
        public async Task UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            // Validate quantity
            if (quantity <= 0)
            {
                throw new Exception("Quantity must be greater than zero.");
            }
            
            // Get cart
            var cart = await GetCartByUserIdAsync(userId);
            
            // Find item
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                throw new Exception($"Cart item with id {cartItemId} not found.");
            }
            
            // Check stock
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new Exception($"Product with id {cartItem.ProductId} not found.");
            }
            
            if (product.Stock < quantity)
            {
                throw new Exception("Not enough product in stock.");
            }
            
            // Update quantity
            cartItem.Quantity = quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;
            
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();
        }
        
        public async Task RemoveCartItemAsync(string userId, int cartItemId)
        {
            // Get cart
            var cart = await GetCartByUserIdAsync(userId);
            
            // Find item
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
            {
                throw new Exception($"Cart item with id {cartItemId} not found.");
            }
            
            // Remove the item
            cart.CartItems.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;
            
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();
        }
        
        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            await _cartRepository.ClearCartAsync(cart.Id);
        }
    }
} 