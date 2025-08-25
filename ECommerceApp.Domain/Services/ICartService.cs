namespace ECommerceApp.Domain.Services
{
    public interface ICartService
    {
        Task<int> GetCartItemCountAsync(string sessionId);
        Task<decimal> GetCartTotalAsync(string sessionId);
        Task<bool> AddToCartAsync(string sessionId, int productId, int quantity);
        Task<bool> UpdateCartItemAsync(string sessionId, int productId, int quantity);
        Task<bool> RemoveFromCartAsync(string sessionId, int productId);
        Task ClearCartAsync(string sessionId);
    }
}
