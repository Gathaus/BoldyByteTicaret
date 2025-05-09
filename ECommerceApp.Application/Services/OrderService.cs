using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        
        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }
        
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
        
        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(id);
        }
        
        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Validation
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new Exception("Order must contain at least one item.");
            }
            
            // Set timestamps
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = "pending";
            order.PaymentStatus = "pending";
            
            // Calculate total amount
            decimal totalAmount = 0;
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with id {item.ProductId} not found.");
                }
                
                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"Not enough stock for product {product.Name}.");
                }
                
                // Update stock
                product.Stock -= item.Quantity;
                _productRepository.Update(product);
                
                // Set price from current product price
                item.Price = product.Price;
                item.CreatedAt = DateTime.UtcNow;
                item.UpdatedAt = DateTime.UtcNow;
                totalAmount += item.Price * item.Quantity;
            }
            
            order.TotalAmount = totalAmount;
            
            // Save the order
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            
            // Clear the user's cart
            var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(order.UserId);
            if (cart != null)
            {
                await _cartRepository.ClearCartAsync(cart.Id);
            }
            
            return order;
        }
        
        public async Task UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception($"Order with id {id} not found.");
            }
            
            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }
        
        public async Task<bool> CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            if (order == null)
            {
                throw new Exception($"Order with id {id} not found.");
            }
            
            // Only pending orders can be cancelled
            if (order.Status != "pending")
            {
                return false;
            }
            
            // Return products to inventory
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    _productRepository.Update(product);
                }
            }
            
            // Update order status
            order.Status = "cancelled";
            order.UpdatedAt = DateTime.UtcNow;
            
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            
            return true;
        }
    }
} 