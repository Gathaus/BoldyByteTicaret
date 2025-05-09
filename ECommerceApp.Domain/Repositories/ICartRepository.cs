using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartWithItemsByUserIdAsync(string userId);
        Task ClearCartAsync(int cartId);
    }
} 