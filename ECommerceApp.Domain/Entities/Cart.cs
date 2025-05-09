using System;
using System.Collections.Generic;

namespace ECommerceApp.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }
    }
    
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
} 