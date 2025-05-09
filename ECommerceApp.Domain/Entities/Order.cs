using System;
using System.Collections.Generic;

namespace ECommerceApp.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // e.g., "pending", "processing", "shipped", "delivered", "cancelled"
        public string PaymentStatus { get; set; } // e.g., "pending", "paid", "failed"
        public string PaymentMethod { get; set; } // e.g., "credit_card", "paypal"
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
    }
    
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Price at the time of order
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
} 