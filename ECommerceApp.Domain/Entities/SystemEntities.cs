using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities
{
    // Kupon/İndirim Kodu Sistemi
    public class Coupon : Discount
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        
        public bool IsPublic { get; set; } = true; // Herkese açık mı
        
        public bool RequiresMinimumAmount { get; set; } = false;
        
        public bool IsFirstOrderOnly { get; set; } = false; // Sadece ilk sipariş
        
        public bool IsOneTimeUse { get; set; } = false; // Tek kullanımlık
        
        public DateTime? FirstUsedAt { get; set; }
        
        // Navigation Properties (Discount'tan inherit)
    }
    
    // Kargo Yöntemleri
    public class ShippingMethod
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? FreeShippingThreshold { get; set; } // Ücretsiz kargo limiti
        
        public bool IsActive { get; set; } = true;
        
        public int MinDeliveryDays { get; set; } = 1;
        
        public int MaxDeliveryDays { get; set; } = 3;
        
        public double? MaxWeight { get; set; } // Maksimum ağırlık (kg)
        
        public double? MaxSize { get; set; } // Maksimum boyut (cm)
        
        public int SortOrder { get; set; } = 0;
        
        // Bölge kısıtlamaları
        public bool IsRestrictedByRegion { get; set; } = false;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ShippingZone> ShippingZones { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        
        public ShippingMethod()
        {
            ShippingZones = new HashSet<ShippingZone>();
            Orders = new HashSet<Order>();
        }
    }
    
    public class ShippingZone
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        public int ShippingMethodId { get; set; }
        
        public decimal? PriceModifier { get; set; } // Fiyat değiştirici
        
        public int? DeliveryDaysModifier { get; set; } // Teslimat gün değiştirici
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ShippingMethod ShippingMethod { get; set; }
        public virtual ICollection<ShippingZoneRegion> ShippingZoneRegions { get; set; }
        
        public ShippingZone()
        {
            ShippingZoneRegions = new HashSet<ShippingZoneRegion>();
        }
    }
    
    public class ShippingZoneRegion
    {
        public int Id { get; set; }
        
        [Required]
        public int ShippingZoneId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string RegionType { get; set; } // City, State, Country, PostalCode
        
        [Required]
        [MaxLength(200)]
        public string RegionValue { get; set; }
        
        // Navigation properties
        public virtual ShippingZone ShippingZone { get; set; }
    }
    
    // Ödeme Yöntemleri
    public class PaymentMethod
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // CreditCard, BankTransfer, Cash, PayPal, Stripe, etc.
        
        [MaxLength(500)]
        public string IconUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool RequiresOnlinePayment { get; set; } = true;
        
        public decimal? MinAmount { get; set; }
        
        public decimal? MaxAmount { get; set; }
        
        public decimal? Fee { get; set; } // Ödeme ücreti
        
        public decimal? FeePercentage { get; set; } // Ödeme yüzdesi
        
        public int SortOrder { get; set; } = 0;
        
        // API ayarları (JSON olarak saklanabilir)
        [MaxLength(2000)]
        public string Configuration { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        
        public PaymentMethod()
        {
            Orders = new HashSet<Order>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
        }
    }
    
    public class PaymentTransaction
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int PaymentMethodId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TransactionId { get; set; } // Ödeme sağlayıcısından gelen ID
        
        [Required]
        public decimal Amount { get; set; }
        
        [MaxLength(10)]
        public string Currency { get; set; } = "TRY";
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Pending, Completed, Failed, Cancelled, Refunded
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // Payment, Refund, Partial_Refund
        
        [MaxLength(500)]
        public string FailureReason { get; set; }
        
        [MaxLength(200)]
        public string GatewayResponse { get; set; }
        
        [MaxLength(2000)]
        public string RawResponse { get; set; } // Gateway'den gelen ham cevap
        
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
    
    // Site Ayarları
    public class Setting
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Key { get; set; }
        
        [MaxLength(2000)]
        public string Value { get; set; }
        
        [MaxLength(200)]
        public string DisplayName { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string Type { get; set; } = "text"; // text, number, boolean, select, textarea, json
        
        [MaxLength(50)]
        public string Category { get; set; } = "General"; // General, Shipping, Payment, SEO, etc.
        
        public bool IsPublic { get; set; } = false; // Frontend'de kullanılabilir mi
        
        public int SortOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    // Blog/İçerik Yönetimi
    public class BlogPost
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [MaxLength(500)]
        public string Excerpt { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [MaxLength(500)]
        public string FeaturedImageUrl { get; set; }
        
        [Required]
        public string AuthorId { get; set; }
        
        public int CategoryId { get; set; }
        
        public bool IsPublished { get; set; } = false;
        
        public bool IsFeatured { get; set; } = false;
        
        public int ViewCount { get; set; } = 0;
        
        public int CommentCount { get; set; } = 0;
        
        // SEO
        [MaxLength(200)]
        public string MetaTitle { get; set; }
        
        [MaxLength(500)]
        public string MetaDescription { get; set; }
        
        [MaxLength(200)]
        public string Slug { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual User Author { get; set; }
        public virtual BlogCategory Category { get; set; }
        public virtual ICollection<BlogPostTag> BlogPostTags { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        
        public BlogPost()
        {
            BlogPostTags = new HashSet<BlogPostTag>();
            BlogComments = new HashSet<BlogComment>();
        }
    }
    
    public class BlogCategory
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(200)]
        public string Slug { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
        
        public BlogCategory()
        {
            BlogPosts = new HashSet<BlogPost>();
        }
    }
    
    public class BlogPostTag
    {
        public int Id { get; set; }
        
        [Required]
        public int BlogPostId { get; set; }
        
        [Required]
        public int TagId { get; set; }
        
        // Navigation properties
        public virtual BlogPost BlogPost { get; set; }
        public virtual Tag Tag { get; set; }
    }
    
    public class BlogComment
    {
        public int Id { get; set; }
        
        [Required]
        public int BlogPostId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public int? ParentId { get; set; } // Cevap için
        
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        
        public bool IsApproved { get; set; } = false;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual BlogPost BlogPost { get; set; }
        public virtual User User { get; set; }
        public virtual BlogComment Parent { get; set; }
        public virtual ICollection<BlogComment> Replies { get; set; }
        
        public BlogComment()
        {
            Replies = new HashSet<BlogComment>();
        }
    }
    
    // Analitik ve Raporlama
    public class Analytics
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string EventType { get; set; } // page_view, product_view, add_to_cart, purchase, etc.
        
        [Required]
        [MaxLength(500)]
        public string EventData { get; set; } // JSON format
        
        public string UserId { get; set; }
        
        [MaxLength(100)]
        public string SessionId { get; set; }
        
        [MaxLength(45)]
        public string IpAddress { get; set; }
        
        [MaxLength(500)]
        public string UserAgent { get; set; }
        
        [MaxLength(500)]
        public string Referrer { get; set; }
        
        [MaxLength(200)]
        public string PageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
    }
    
    // Newsletter
    public class Newsletter
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsConfirmed { get; set; } = false;
        
        public DateTime? ConfirmedAt { get; set; }
        
        [MaxLength(100)]
        public string Source { get; set; } // Website, Popup, Checkout, etc.
        
        [MaxLength(100)]
        public string Tags { get; set; } // Segmentasyon için
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? UnsubscribedAt { get; set; }
    }
    
    // SEO ve Meta
    public class SeoPage
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string PagePath { get; set; } // /products, /categories/electronics, etc.
        
        [MaxLength(200)]
        public string Title { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(1000)]
        public string Keywords { get; set; }
        
        [MaxLength(200)]
        public string OgTitle { get; set; }
        
        [MaxLength(500)]
        public string OgDescription { get; set; }
        
        [MaxLength(500)]
        public string OgImage { get; set; }
        
        public bool NoIndex { get; set; } = false;
        
        public bool NoFollow { get; set; } = false;
        
        [MaxLength(2000)]
        public string CustomMeta { get; set; } // Özel meta etiketleri
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 