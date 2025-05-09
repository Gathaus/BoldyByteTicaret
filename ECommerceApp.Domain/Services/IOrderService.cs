using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderWithDetailsAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderStatusAsync(int id, string status);
        Task<bool> CancelOrderAsync(int id);
    }
} 