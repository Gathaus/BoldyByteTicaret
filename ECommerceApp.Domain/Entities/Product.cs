using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(2000)]
        public string Description { get; set; }
        
        [MaxLength(500)]
        public string ShortDescription { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? ComparePrice { get; set; } // Liste fiyatı (indirim göstermek için)
        
        public decimal? CostPrice { get; set; } // Maliyet fiyatı
        
        [Required]
        public int Stock { get; set; }
        
        public int? LowStockThreshold { get; set; } // Düşük stok uyarısı
        
        public bool TrackQuantity { get; set; } = true;
        
        public bool ContinueSelling { get; set; } = false; // Stok bitse de satış devam etsin mi
        
        [MaxLength(100)]
        public string SKU { get; set; } // Stok kodu
        
        [MaxLength(100)]
        public string Barcode { get; set; }
        
        public double? Weight { get; set; } // Ağırlık (gram)
        
        public double? Length { get; set; } // Uzunluk (cm)
        public double? Width { get; set; } // Genişlik (cm)
        public double? Height { get; set; } // Yükseklik (cm)
        
        [Required]
        public int CategoryId { get; set; }
        
        public int? BrandId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsFeatured { get; set; } = false; // Öne çıkan ürün
        
        public bool IsDigital { get; set; } = false; // Dijital ürün
        
        public bool RequiresShipping { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
        
        // SEO Alanları
        [MaxLength(200)]
        public string MetaTitle { get; set; }
        
        [MaxLength(500)]
        public string MetaDescription { get; set; }
        
        [MaxLength(200)]
        public string Slug { get; set; }
        
        // İstatistikler
        public int ViewCount { get; set; } = 0;
        public int SalesCount { get; set; } = 0;
        public decimal AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        
        // Tarihler
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        
        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
        public virtual ICollection<ProductView> ProductViews { get; set; }
        
        public Product()
        {
            ProductImages = new HashSet<ProductImage>();
            ProductAttributes = new HashSet<ProductAttribute>();
            ProductVariants = new HashSet<ProductVariant>();
            ProductTags = new HashSet<ProductTag>();
            ProductDiscounts = new HashSet<ProductDiscount>();
            ProductReviews = new HashSet<ProductReview>();
            OrderItems = new HashSet<OrderItem>();
            WishlistItems = new HashSet<WishlistItem>();
            ProductViews = new HashSet<ProductView>();
        }
    }
    
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(200)]
        public string ImageUrl { get; set; }
        
        public int? ParentId { get; set; } // Hiyerarşik kategori yapısı
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
        
        // SEO Alanları
        [MaxLength(200)]
        public string MetaTitle { get; set; }
        
        [MaxLength(500)]
        public string MetaDescription { get; set; }
        
        [MaxLength(200)]
        public string Slug { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; }
        
        public Category()
        {
            Children = new HashSet<Category>();
            Products = new HashSet<Product>();
            CategoryAttributes = new HashSet<CategoryAttribute>();
        }
    }
    
    public class Brand
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
        
        [MaxLength(200)]
        public string LogoUrl { get; set; }
        
        [MaxLength(200)]
        public string Website { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int SortOrder { get; set; } = 0;
        
        // SEO Alanları
        [MaxLength(200)]
        public string MetaTitle { get; set; }
        
        [MaxLength(500)]
        public string MetaDescription { get; set; }
        
        [MaxLength(200)]
        public string Slug { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }
        
        public Brand()
        {
            Products = new HashSet<Product>();
        }
    }
    
    public class ProductImage
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        
        [MaxLength(200)]
        public string AltText { get; set; }
        
        public bool IsMain { get; set; } = false;
        
        public int SortOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
    }
    
    public class ProductAttribute
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int AttributeId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Value { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; }
        public virtual ProductAttributeDefinition Attribute { get; set; }
    }
    
    public class ProductAttributeDefinition
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string DisplayName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // text, number, boolean, select, multiselect
        
        public bool IsRequired { get; set; } = false;
        
        public bool IsFilterable { get; set; } = false;
        
        public bool IsVariantAttribute { get; set; } = false; // Varyant oluşturmak için kullanılıyor mu
        
        public int SortOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; }
        public virtual ICollection<AttributeValue> AttributeValues { get; set; }
        
        public ProductAttributeDefinition()
        {
            ProductAttributes = new HashSet<ProductAttribute>();
            CategoryAttributes = new HashSet<CategoryAttribute>();
            AttributeValues = new HashSet<AttributeValue>();
        }
    }
    
    public class AttributeValue
    {
        public int Id { get; set; }
        
        [Required]
        public int AttributeId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Value { get; set; }
        
        [MaxLength(200)]
        public string DisplayValue { get; set; }
        
        public int SortOrder { get; set; } = 0;
        
        // Navigation properties
        public virtual ProductAttributeDefinition Attribute { get; set; }
    }
    
    public class CategoryAttribute
    {
        public int Id { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public int AttributeId { get; set; }
        
        public bool IsRequired { get; set; } = false;
        
        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual ProductAttributeDefinition Attribute { get; set; }
    }
} 