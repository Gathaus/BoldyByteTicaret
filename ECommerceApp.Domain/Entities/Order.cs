using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } // Benzersiz sipariş numarası
        
        [Required]
        public string UserId { get; set; }
        
        public int? ShippingAddressId { get; set; }
        
        public int? BillingAddressId { get; set; }
        
        // Fiyatlandırma
        public decimal SubtotalAmount { get; set; } // Ara toplam
        
        public decimal TaxAmount { get; set; } = 0; // KDV
        
        public decimal ShippingAmount { get; set; } = 0; // Kargo ücreti
        
        public decimal DiscountAmount { get; set; } = 0; // İndirim
        
        [Required]
        public decimal TotalAmount { get; set; } // Genel toplam
        
        // Sipariş Durumu
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Processing, Shipped, Delivered, Cancelled, Returned
        
        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed, Refunded, Partial_Refund
        
        [Required]
        [MaxLength(50)]
        public string FulfillmentStatus { get; set; } = "Unfulfilled"; // Unfulfilled, Partial, Fulfilled
        
        // Ödeme Bilgileri
        [MaxLength(50)]
        public string PaymentMethod { get; set; } // CreditCard, BankTransfer, Cash, PayPal, etc.
        
        [MaxLength(200)]
        public string PaymentReference { get; set; } // Ödeme referans numarası
        
        public DateTime? PaidAt { get; set; }
        
        // Kargo Bilgileri
        [MaxLength(100)]
        public string ShippingMethod { get; set; } // Standard, Express, NextDay
        
        [MaxLength(200)]
        public string TrackingNumber { get; set; }
        
        [MaxLength(100)]
        public string CarrierName { get; set; } // Kargo firması
        
        public DateTime? ShippedAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        public DateTime? EstimatedDeliveryDate { get; set; }
        
        // Notlar
        [MaxLength(1000)]
        public string CustomerNotes { get; set; }
        
        [MaxLength(1000)]
        public string AdminNotes { get; set; }
        
        // İptal/İade
        [MaxLength(500)]
        public string CancellationReason { get; set; }
        
        public DateTime? CancelledAt { get; set; }
        
        public string CancelledByUserId { get; set; }
        
        // Para Birimi
        [MaxLength(10)]
        public string Currency { get; set; } = "TRY";
        
        public decimal? ExchangeRate { get; set; } = 1;
        
        // Fatura Bilgileri
        [MaxLength(100)]
        public string InvoiceNumber { get; set; }
        
        public DateTime? InvoiceDate { get; set; }
        
        [MaxLength(500)]
        public string InvoiceUrl { get; set; }
        
        // Tarihler
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual User CancelledByUser { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }
        public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; }
        public virtual ICollection<OrderShipment> OrderShipments { get; set; }
        public virtual ICollection<OrderRefund> OrderRefunds { get; set; }
        public virtual ICollection<DiscountUsage> DiscountUsages { get; set; }
        public virtual ICollection<CustomerSupport> CustomerSupports { get; set; }
        
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            OrderStatusHistories = new HashSet<OrderStatusHistory>();
            OrderDiscounts = new HashSet<OrderDiscount>();
            OrderShipments = new HashSet<OrderShipment>();
            OrderRefunds = new HashSet<OrderRefund>();
            DiscountUsages = new HashSet<DiscountUsage>();
            CustomerSupports = new HashSet<CustomerSupport>();
        }
        
        // Computed Properties
        public decimal ItemsCount => OrderItems?.Sum(x => x.Quantity) ?? 0;
        
        public bool CanBeCancelled => Status is "Pending" or "Confirmed" && PaymentStatus != "Paid";
        
        public bool IsCompleted => Status == "Delivered";
        
        public bool IsPaid => PaymentStatus == "Paid";
    }
    
    public class OrderItem
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        public int? ProductVariantId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; } // Birim fiyat (sipariş anındaki)
        
        public decimal? ComparePrice { get; set; } // Liste fiyatı
        
        [Required]
        public decimal TotalPrice { get; set; } // Toplam fiyat
        
        // Ürün bilgileri (sipariş anındaki - değişebilir)
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
        
        // Fulfillment
        public int FulfilledQuantity { get; set; } = 0;
        
        public int RefundedQuantity { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
        public virtual ICollection<OrderItemFulfillment> OrderItemFulfillments { get; set; }
        
        public OrderItem()
        {
            OrderItemFulfillments = new HashSet<OrderItemFulfillment>();
        }
        
        // Computed Properties
        public int RemainingQuantity => Quantity - FulfilledQuantity;
        
        public bool IsFullyFulfilled => FulfilledQuantity >= Quantity;
        
        public decimal RefundableAmount => UnitPrice * (Quantity - RefundedQuantity);
    }
    
    public class OrderStatusHistory
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        
        [MaxLength(500)]
        public string Comment { get; set; }
        
        public string ChangedByUserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual User ChangedByUser { get; set; }
    }
    
    public class OrderDiscount
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
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
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Discount Discount { get; set; }
    }
    
    public class OrderShipment
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TrackingNumber { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CarrierName { get; set; }
        
        [MaxLength(100)]
        public string ShippingMethod { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Preparing"; // Preparing, Shipped, InTransit, Delivered, Exception
        
        public DateTime? ShippedAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        public DateTime? EstimatedDeliveryDate { get; set; }
        
        [MaxLength(500)]
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual ICollection<OrderItemFulfillment> OrderItemFulfillments { get; set; }
        public virtual ICollection<ShipmentTracking> ShipmentTrackings { get; set; }
        
        public OrderShipment()
        {
            OrderItemFulfillments = new HashSet<OrderItemFulfillment>();
            ShipmentTrackings = new HashSet<ShipmentTracking>();
        }
    }
    
    public class OrderItemFulfillment
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderItemId { get; set; }
        
        [Required]
        public int OrderShipmentId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual OrderItem OrderItem { get; set; }
        public virtual OrderShipment OrderShipment { get; set; }
    }
    
    public class ShipmentTracking
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderShipmentId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Status { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(200)]
        public string Location { get; set; }
        
        public DateTime EventDate { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual OrderShipment OrderShipment { get; set; }
    }
    
    public class OrderRefund
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string RefundNumber { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Reason { get; set; } // CustomerRequest, Defective, Cancelled, etc.
        
        [MaxLength(1000)]
        public string ReasonDetails { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Processed, Rejected
        
        [MaxLength(500)]
        public string AdminNotes { get; set; }
        
        public string ProcessedByUserId { get; set; }
        
        public DateTime? ProcessedAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual User ProcessedByUser { get; set; }
        public virtual ICollection<OrderRefundItem> OrderRefundItems { get; set; }
        
        public OrderRefund()
        {
            OrderRefundItems = new HashSet<OrderRefundItem>();
        }
    }
    
    public class OrderRefundItem
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderRefundId { get; set; }
        
        [Required]
        public int OrderItemId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal UnitRefundAmount { get; set; }
        
        [Required]
        public decimal TotalRefundAmount { get; set; }
        
        // Navigation properties
        public virtual OrderRefund OrderRefund { get; set; }
        public virtual OrderItem OrderItem { get; set; }
    }
} 