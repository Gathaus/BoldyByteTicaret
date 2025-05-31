using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ECommerceApp.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } // Null olabilir (anonim kullanıcı)
        
        [MaxLength(100)]
        public string SessionId { get; set; } // Anonim kullanıcılar için session ID
        
        [MaxLength(10)]
        public string Currency { get; set; } = "TRY";
        
        public decimal? ExchangeRate { get; set; } = 1;
        
        // Durum
        public bool IsActive { get; set; } = true;
        
        public bool IsAbandoned { get; set; } = false;
        
        public DateTime? AbandonedAt { get; set; }
        
        // İndirimler
        public decimal DiscountAmount { get; set; } = 0;
        
        [MaxLength(100)]
        public string CouponCode { get; set; }
        
        public int? CouponId { get; set; }
        
        // Kargo
        public decimal ShippingAmount { get; set; } = 0;
        
        public int? ShippingAddressId { get; set; }
        
        [MaxLength(100)]
        public string ShippingMethod { get; set; }
        
        // Vergi
        public decimal TaxAmount { get; set; } = 0;
        
        public decimal TaxRate { get; set; } = 0.18m; // KDV oranı
        
        // Notlar
        [MaxLength(1000)]
        public string Notes { get; set; }
        
        // İstatistikler
        public int ViewCount { get; set; } = 0;
        
        public DateTime? LastViewedAt { get; set; }
        
        // Tarihler
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual Discount Coupon { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<CartDiscount> CartDiscounts { get; set; }
        
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
            CartDiscounts = new HashSet<CartDiscount>();
        }
        
        // Computed Properties
        public decimal SubtotalAmount => CartItems?.Sum(x => x.TotalPrice) ?? 0;
        
        public decimal TotalAmount => SubtotalAmount + ShippingAmount + TaxAmount - DiscountAmount;
        
        public int ItemsCount => CartItems?.Sum(x => x.Quantity) ?? 0;
        
        public int UniqueItemsCount => CartItems?.Count ?? 0;
        
        public bool IsEmpty => !CartItems?.Any() ?? true;
        
        public double TotalWeight => CartItems?.Sum(x => (x.Product?.Weight ?? x.ProductVariant?.Weight ?? 0) * x.Quantity) ?? 0;
        
        public bool RequiresShipping => CartItems?.Any(x => x.RequiresShipping) ?? false;
        
        public bool HasDigitalItems => CartItems?.Any(x => x.IsDigital) ?? false;
        
        public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }
    
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public int CartId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        public int? ProductVariantId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; } // Güncel birim fiyat
        
        public decimal? ComparePrice { get; set; } // Liste fiyatı
        
        [Required]
        public decimal TotalPrice { get; set; } // Toplam fiyat
        
        // Ürün snapshot bilgileri (performans için)
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }
        
        [MaxLength(100)]
        public string ProductSKU { get; set; }
        
        [MaxLength(500)]
        public string ProductImageUrl { get; set; }
        
        [MaxLength(200)]
        public string VariantTitle { get; set; }
        
        public double? Weight { get; set; }
        
        public bool RequiresShipping { get; set; } = true;
        
        public bool IsDigital { get; set; } = false;
        
        public bool IsAvailable { get; set; } = true; // Stok durumu
        
        [MaxLength(500)]
        public string UnavailableReason { get; set; } // Stok yoksa sebep
        
        // Özel fiyatlandırma
        public int? DiscountId { get; set; }
        
        public decimal DiscountAmount { get; set; } = 0;
        
        // Tarihler
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public virtual Discount Discount { get; set; }
        
        // Computed Properties
        public decimal OriginalTotalPrice => UnitPrice * Quantity;
        
        public decimal SavedAmount => OriginalTotalPrice - TotalPrice;
        
        public decimal DiscountPercentage => ComparePrice.HasValue && ComparePrice.Value > 0 
            ? Math.Round(((ComparePrice.Value - UnitPrice) / ComparePrice.Value) * 100, 2) 
            : 0;
        
        public bool HasDiscount => DiscountAmount > 0 || (ComparePrice.HasValue && ComparePrice.Value > UnitPrice);
    }
    
    public class CartDiscount
    {
        public int Id { get; set; }
        
        [Required]
        public int CartId { get; set; }
        
        [Required]
        public int DiscountId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string DiscountName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string DiscountType { get; set; }
        
        [Required]
        public decimal DiscountValue { get; set; }
        
        [Required]
        public decimal DiscountAmount { get; set; }
        
        public bool IsApplied { get; set; } = true;
        
        [MaxLength(500)]
        public string ErrorMessage { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Discount Discount { get; set; }
    }
    
    public class AbandonedCart
    {
        public int Id { get; set; }
        
        [Required]
        public int CartId { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }
        
        public decimal CartValue { get; set; }
        
        public int ItemsCount { get; set; }
        
        public bool EmailSent { get; set; } = false;
        
        public DateTime? EmailSentAt { get; set; }
        
        public int EmailCount { get; set; } = 0;
        
        public bool IsRecovered { get; set; } = false;
        
        public DateTime? RecoveredAt { get; set; }
        
        public int? RecoveredOrderId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Order RecoveredOrder { get; set; }
    }
    
    public class SavedForLater
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        public int? ProductVariantId { get; set; }
        
        public int Quantity { get; set; } = 1;
        
        public decimal? SavedPrice { get; set; } // Kaydedildiği andaki fiyat
        
        [MaxLength(500)]
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
} 