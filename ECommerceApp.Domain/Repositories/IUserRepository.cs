using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
    }
} 