using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        
        public User()
        {
            Orders = new HashSet<Order>();
            Addresses = new HashSet<Address>();
        }
    }
    
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation property
        public virtual User User { get; set; }
    }
} 