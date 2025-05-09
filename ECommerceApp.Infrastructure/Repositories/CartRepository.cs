using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<Cart> GetCartWithItemsByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        
        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
            
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
} 