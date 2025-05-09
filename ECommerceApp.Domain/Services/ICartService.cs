using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<CartItem> AddItemToCartAsync(string userId, int productId, int quantity);
        Task UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity);
        Task RemoveCartItemAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
    }
} 