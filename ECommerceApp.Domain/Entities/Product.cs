using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities
{
    // Base Entity for common properties
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    // SEO Entity for reusable SEO fields  
    public abstract class SEOEntity : BaseEntity
    {
        [MaxLength(200)]
        public string MetaTitle { get; set; } = "";
        
        [MaxLength(500)]
        public string MetaDescription { get; set; } = "";
        
        [MaxLength(200)]
        public string Slug { get; set; } = "";
    }

    // Category Entity
    public class Category : SEOEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        
        [MaxLength(500)]
        public string Description { get; set; } = "";
        
        [MaxLength(500)]
        public string ImageUrl { get; set; } = "";
        
        public int? ParentId { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        
        // Navigation properties
        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    // Brand Entity
    public class Brand : SEOEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        
        [MaxLength(500)]
        public string Description { get; set; } = "";
        
        [MaxLength(500)]
        public string LogoUrl { get; set; } = "";
        
        [MaxLength(200)]
        public string Website { get; set; } = "";
        
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        
        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    // Tag Entity for product labels (15% OFF, best seller, new, top rated, etc.)
    public class Tag : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "";
        
        [MaxLength(50)]
        public string DisplayName { get; set; } = "";
        
        [MaxLength(50)]
        public string CssClass { get; set; } = ""; // bg-red1, bg-blue1, bg-green1, etc.
        
        public TagType Type { get; set; } = TagType.Label;
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        
        // Navigation properties
        public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }

    public enum TagType
    {
        Discount,  // 15% OFF, 20% OFF etc.
        Status,    // best seller, new, top rated etc.
        Label,     // general labels
        Feature    // special features
    }

    // Product Entity (main entity with Index.cshtml requirements)
    public class Product : SEOEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = "";
        
        [MaxLength(2000)]
        public string Description { get; set; } = "";
        
        [MaxLength(500)]
        public string ShortDescription { get; set; } = "";
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? ComparePrice { get; set; } // Old Price for discount display
        
        public int Stock { get; set; } = 0;
        public bool TrackQuantity { get; set; } = true;
        
        [MaxLength(100)]
        public string SKU { get; set; } = "";
        
        [Required]
        public int CategoryId { get; set; }
        
        public int? BrandId { get; set; }
        
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public bool HasInstallment { get; set; } = false; // Index.cshtml requirement
        
        public int SortOrder { get; set; } = 0;
        public int ViewCount { get; set; } = 0;
        public int SalesCount { get; set; } = 0;
        
        // Rating & Reviews (Index.cshtml requirement)
        public decimal AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        
        public DateTime? PublishedAt { get; set; }
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual Brand? Brand { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }

    // Product Images
    public class ProductImage : BaseEntity
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = "";
        
        [MaxLength(200)]
        public string AltText { get; set; } = "";
        
        public bool IsMain { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        
        // Navigation property
        public virtual Product Product { get; set; } = null!;
    }

    // Many-to-Many relationship between Product and Tag
    public class ProductTag : BaseEntity
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int TagId { get; set; }
        
        // Navigation properties
        public virtual Product Product { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
} 