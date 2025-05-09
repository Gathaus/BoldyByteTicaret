using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderWithItemsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderWithDetailsAsync(int id);
    }
} 