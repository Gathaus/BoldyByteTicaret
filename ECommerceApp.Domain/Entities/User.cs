using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Domain.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [MaxLength(10)]
        public string Gender { get; set; } // Male, Female, Other
        
        [MaxLength(500)]
        public string ProfileImageUrl { get; set; }
        
        [MaxLength(20)]
        public string PreferredLanguage { get; set; } = "tr-TR";
        
        [MaxLength(10)]
        public string PreferredCurrency { get; set; } = "TRY";
        
        public bool NewsletterSubscription { get; set; } = true;
        
        public bool SmsNotifications { get; set; } = true;
        
        public bool EmailNotifications { get; set; } = true;
        
        public bool IsActive { get; set; } = true;
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime? EmailVerifiedAt { get; set; }
        
        public DateTime? PhoneVerifiedAt { get; set; }
        
        // İstatistikler
        public int TotalOrders { get; set; } = 0;
        
        public decimal TotalSpent { get; set; } = 0;
        
        public int LoyaltyPoints { get; set; } = 0;
        
        [MaxLength(50)]
        public string CustomerType { get; set; } = "Regular"; // Regular, VIP, Premium
        
        // Tarihler
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public virtual ICollection<ReviewHelpful> ReviewHelpfuls { get; set; }
        public virtual ICollection<ProductView> ProductViews { get; set; }
        public virtual ICollection<DiscountUsage> DiscountUsages { get; set; }
        public virtual ICollection<CustomerSupport> CustomerSupports { get; set; }
        public virtual ICollection<UserActivity> UserActivities { get; set; }
        
        public User()
        {
            Orders = new HashSet<Order>();
            Addresses = new HashSet<Address>();
            Carts = new HashSet<Cart>();
            Wishlists = new HashSet<Wishlist>();
            ProductReviews = new HashSet<ProductReview>();
            ReviewHelpfuls = new HashSet<ReviewHelpful>();
            ProductViews = new HashSet<ProductView>();
            DiscountUsages = new HashSet<DiscountUsage>();
            CustomerSupports = new HashSet<CustomerSupport>();
            UserActivities = new HashSet<UserActivity>();
        }
        
        // Computed Properties
        public string FullName { get => $"{FirstName} {LastName}"; private set { } }
        
        public int Age { get => DateOfBirth.HasValue ? 
            DateTime.Now.Year - DateOfBirth.Value.Year - 
            (DateTime.Now.DayOfYear < DateOfBirth.Value.DayOfYear ? 1 : 0) : 0; private set { } }
    }
    
    public class Address
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } // Ev, İş, vb.
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [MaxLength(100)]
        public string Company { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string AddressLine1 { get; set; }
        
        [MaxLength(200)]
        public string AddressLine2 { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string District { get; set; }
        
        [MaxLength(100)]
        public string State { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = "Türkiye";
        
        [MaxLength(20)]
        public string Phone { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; } = "Shipping"; // Shipping, Billing, Both
        
        public bool IsDefault { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
        
        // Konum bilgileri (opsiyonel)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Order> ShippingOrders { get; set; }
        public virtual ICollection<Order> BillingOrders { get; set; }
        
        public Address()
        {
            ShippingOrders = new HashSet<Order>();
            BillingOrders = new HashSet<Order>();
        }
        
        public string FullAddress { get => $"{AddressLine1}, {AddressLine2}, {District}, {City}, {PostalCode}, {Country}".Replace(", ,", ",").Trim(',', ' '); private set { } }
        
        public string FullName { get => $"{FirstName} {LastName}"; private set { } }
    }
    
    public class CustomerSupport
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Subject { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } // Order, Product, Payment, Technical, Other
        
        [Required]
        [MaxLength(50)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Urgent
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Open"; // Open, In Progress, Resolved, Closed
        
        public int? OrderId { get; set; }
        
        public int? ProductId { get; set; }
        
        public string AssignedToUserId { get; set; } // Admin/Support user
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }
        public virtual ICollection<SupportMessage> SupportMessages { get; set; }
        
        public CustomerSupport()
        {
            SupportMessages = new HashSet<SupportMessage>();
        }
    }
    
    public class SupportMessage
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerSupportId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
        
        public bool IsFromCustomer { get; set; } = true;
        
        public bool IsRead { get; set; } = false;
        
        [MaxLength(500)]
        public string AttachmentUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual CustomerSupport CustomerSupport { get; set; }
        public virtual User User { get; set; }
    }
    
    public class UserActivity
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Activity { get; set; } // login, logout, order_placed, product_viewed, etc.
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(45)]
        public string IpAddress { get; set; }
        
        [MaxLength(500)]
        public string UserAgent { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
    }
} 