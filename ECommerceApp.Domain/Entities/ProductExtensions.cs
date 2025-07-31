using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [MaxLength(100)]
        public string SKU { get; set; }
        
        [MaxLength(100)]
        public string Barcode { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? ComparePrice { get; set; }
        
        public decimal? CostPrice { get; set; }
        
        [Required]
        public int Stock { get; set; }
        
        public double? Weight { get; set; }
        
        public bool IsDefault { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
        
        [MaxLength(200)]
        public string Title { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public virtual ICollection<ProductVariantImage> ProductVariantImages { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        
        public ProductVariant()
        {
            ProductVariantAttributes = new HashSet<ProductVariantAttribute>();
            ProductVariantImages = new HashSet<ProductVariantImage>();
            OrderItems = new HashSet<OrderItem>();
            CartItems = new HashSet<CartItem>();
        }
    }
    
    public class ProductVariantAttribute
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductVariantId { get; set; }
        
        [Required]
        public int AttributeId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Value { get; set; }
        
        // Navigation properties
        public virtual ProductVariant ProductVariant { get; set; }
        public virtual ProductAttributeDefinition Attribute { get; set; }
    }
    
    public class ProductVariantImage
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductVariantId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        
        [MaxLength(200)]
        public string AltText { get; set; }
        
        public bool IsMain { get; set; } = false;
        
        public int SortOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual ProductVariant ProductVariant { get; set; }
    }
    
    // Product Labels/Tags for "15% OFF", "best seller", "new", "top rated", etc.
    public class Tag : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string DisplayName { get; set; }
        
        [MaxLength(50)]
        public string CssClass { get; set; } // bg-red1, bg-blue1, bg-green1, etc.
        
        public TagType Type { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
        
        // Navigation properties
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        
        public Tag()
        {
            ProductTags = new HashSet<ProductTag>();
        }
    }

    public enum TagType
    {
        Discount = 1,      // "15% OFF", "10% OFF"
        Status = 2,        // "best seller", "top rated", "new", "out of stock"
        Feature = 3,       // "0% installment"
        Seasonal = 4       // "pre-order", "limited time"
    }

    public class ProductTag : BaseEntity
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int TagId { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Tag Tag { get; set; }
    }
    
    public class Discount
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // percentage, fixed_amount, buy_x_get_y
        
        [Required]
        public decimal Value { get; set; }
        
        public decimal? MinimumAmount { get; set; } // Minimum sepet tutarı
        
        public decimal? MaximumDiscount { get; set; } // Maksimum indirim tutarı
        
        public int? UsageLimit { get; set; } // Toplam kullanım limiti
        
        public int? UsageLimitPerCustomer { get; set; } // Müşteri başına kullanım limiti
        
        public int UsageCount { get; set; } = 0; // Kullanılma sayısı
        
        public bool IsActive { get; set; } = true;
        
        public DateTime? StartsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public virtual ICollection<DiscountUsage> DiscountUsages { get; set; }
        
        public Discount()
        {
            ProductDiscounts = new HashSet<ProductDiscount>();
            DiscountUsages = new HashSet<DiscountUsage>();
        }
    }
    
    public class ProductDiscount
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int DiscountId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual Discount Discount { get; set; }
    }
    
    public class DiscountUsage
    {
        public int Id { get; set; }
        
        [Required]
        public int DiscountId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public int? OrderId { get; set; }
        
        public decimal DiscountAmount { get; set; }
        
        public DateTime UsedAt { get; set; }
        
        // Navigation properties
        public virtual Discount Discount { get; set; }
        public virtual User User { get; set; }
        public virtual Order Order { get; set; }
    }
    
    public class ProductReview
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [MaxLength(100)]
        public string Title { get; set; }
        
        [MaxLength(2000)]
        public string Comment { get; set; }
        
        public bool IsVerified { get; set; } = false; // Ürünü satın almış mı?
        
        public bool IsApproved { get; set; } = false;
        
        public bool IsHelpful { get; set; } = false;
        
        public int HelpfulCount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ReviewImage> ReviewImages { get; set; }
        public virtual ICollection<ReviewHelpful> ReviewHelpfuls { get; set; }
        
        public ProductReview()
        {
            ReviewImages = new HashSet<ReviewImage>();
            ReviewHelpfuls = new HashSet<ReviewHelpful>();
        }
    }
    
    public class ReviewImage
    {
        public int Id { get; set; }
        
        [Required]
        public int ReviewId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        
        [MaxLength(200)]
        public string AltText { get; set; }
        
        public int SortOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual ProductReview Review { get; set; }
    }
    
    public class ReviewHelpful
    {
        public int Id { get; set; }
        
        [Required]
        public int ReviewId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public bool IsHelpful { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual ProductReview Review { get; set; }
        public virtual User User { get; set; }
    }
    
    public class Wishlist
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } = "Varsayılan Liste";
        
        public bool IsDefault { get; set; } = true;
        
        public bool IsPublic { get; set; } = false;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
        
        public Wishlist()
        {
            WishlistItems = new HashSet<WishlistItem>();
        }
    }
    
    public class WishlistItem
    {
        public int Id { get; set; }
        
        [Required]
        public int WishlistId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        public int? ProductVariantId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual Wishlist Wishlist { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
    
    public class ProductView
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        public string UserId { get; set; } // Null olabilir (anonim kullanıcı)
        
        [MaxLength(45)]
        public string IpAddress { get; set; }
        
        [MaxLength(500)]
        public string UserAgent { get; set; }
        
        [MaxLength(500)]
        public string Referrer { get; set; }
        
        public DateTime ViewedAt { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
} 