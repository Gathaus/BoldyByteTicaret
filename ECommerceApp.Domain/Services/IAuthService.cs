using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(User user, string password);
        Task<(bool success, string token)> LoginAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
} 