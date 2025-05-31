using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // Product Related
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeDefinition> Attributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public DbSet<ProductVariantImage> ProductVariantImages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ReviewHelpful> ReviewHelpfuls { get; set; }
        public DbSet<ProductView> ProductViews { get; set; }
        
        // Order Related
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }
        public DbSet<OrderShipment> OrderShipments { get; set; }
        public DbSet<OrderItemFulfillment> OrderItemFulfillments { get; set; }
        public DbSet<ShipmentTracking> ShipmentTrackings { get; set; }
        public DbSet<OrderRefund> OrderRefunds { get; set; }
        public DbSet<OrderRefundItem> OrderRefundItems { get; set; }
        
        // Cart Related
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartDiscount> CartDiscounts { get; set; }
        public DbSet<AbandonedCart> AbandonedCarts { get; set; }
        public DbSet<SavedForLater> SavedForLaters { get; set; }
        
        // User Related
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CustomerSupport> CustomerSupports { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        
        // Discount Related
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<DiscountUsage> DiscountUsages { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        
        // System Related
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<ShippingZone> ShippingZones { get; set; }
        public DbSet<ShippingZoneRegion> ShippingZoneRegions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Setting> Settings { get; set; }
        
        // Content Related
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogPostTag> BlogPostTags { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        
        // Analytics & SEO
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<SeoPage> SeoPages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Entity Relationships and Constraints
            ConfigureProductEntities(modelBuilder);
            ConfigureOrderEntities(modelBuilder);
            ConfigureCartEntities(modelBuilder);
            ConfigureUserEntities(modelBuilder);
            ConfigureDiscountEntities(modelBuilder);
            ConfigureSystemEntities(modelBuilder);
            ConfigureContentEntities(modelBuilder);
            ConfigureAnalyticsEntities(modelBuilder);
            
            // Configure Indexes
            ConfigureIndexes(modelBuilder);
            
            // Configure Precision for Decimal Properties
            ConfigurePrecisions(modelBuilder);
        }
        
        private void ConfigureProductEntities(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SKU).IsUnique().HasFilter("[SKU] IS NOT NULL");
                entity.HasIndex(e => e.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");
                entity.HasIndex(e => new { e.CategoryId, e.IsActive });
                entity.HasIndex(e => e.BrandId);
                
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(e => e.BrandId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");
                entity.HasIndex(e => e.ParentId);
                
                entity.HasOne(e => e.Parent)
                    .WithMany(c => c.Children)
                    .HasForeignKey(e => e.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Brand
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");
            });
            
            // ProductImage
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.IsMain });
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // ProductAttribute
            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.AttributeId }).IsUnique();
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductAttributes)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Attribute)
                    .WithMany(a => a.ProductAttributes)
                    .HasForeignKey(e => e.AttributeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Attribute
            modelBuilder.Entity<ProductAttributeDefinition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
            });
            
            // AttributeValue
            modelBuilder.Entity<AttributeValue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.AttributeId, e.Value }).IsUnique();
                
                entity.HasOne(e => e.Attribute)
                    .WithMany(a => a.AttributeValues)
                    .HasForeignKey(e => e.AttributeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // CategoryAttribute
            modelBuilder.Entity<CategoryAttribute>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.CategoryId, e.AttributeId }).IsUnique();
                
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.CategoryAttributes)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Attribute)
                    .WithMany(a => a.CategoryAttributes)
                    .HasForeignKey(e => e.AttributeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            // ProductVariant
            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SKU).IsUnique().HasFilter("[SKU] IS NOT NULL");
                entity.HasIndex(e => new { e.ProductId, e.IsDefault });
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductVariants)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // ProductVariantAttribute
            modelBuilder.Entity<ProductVariantAttribute>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductVariantId, e.AttributeId }).IsUnique();
                
                entity.HasOne(e => e.ProductVariant)
                    .WithMany(pv => pv.ProductVariantAttributes)
                    .HasForeignKey(e => e.ProductVariantId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Attribute)
                    .WithMany()
                    .HasForeignKey(e => e.AttributeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            // ProductVariantImage
            modelBuilder.Entity<ProductVariantImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductVariantId, e.IsMain });
                
                entity.HasOne(e => e.ProductVariant)
                    .WithMany(pv => pv.ProductVariantImages)
                    .HasForeignKey(e => e.ProductVariantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Tag
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");
            });
            
            // ProductTag
            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.TagId }).IsUnique();
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Tag)
                    .WithMany(t => t.ProductTags)
                    .HasForeignKey(e => e.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // ProductReview
            modelBuilder.Entity<ProductReview>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.UserId });
                entity.HasIndex(e => e.IsApproved);
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductReviews)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany(u => u.ProductReviews)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // ReviewImage
            modelBuilder.Entity<ReviewImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Review)
                    .WithMany(r => r.ReviewImages)
                    .HasForeignKey(e => e.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // ReviewHelpful
            modelBuilder.Entity<ReviewHelpful>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ReviewId, e.UserId }).IsUnique();
                
                entity.HasOne(e => e.Review)
                    .WithMany(r => r.ReviewHelpfuls)
                    .HasForeignKey(e => e.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany(u => u.ReviewHelpfuls)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // ProductView
            modelBuilder.Entity<ProductView>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductId, e.ViewedAt });
                entity.HasIndex(e => e.UserId);
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.ProductViews)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany(u => u.ProductViews)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
        
        private void ConfigureOrderEntities(ModelBuilder modelBuilder)
        {
            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => new { e.UserId, e.Status });
                entity.HasIndex(e => e.CreatedAt);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.ShippingAddress)
                    .WithMany(a => a.ShippingOrders)
                    .HasForeignKey(e => e.ShippingAddressId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.BillingAddress)
                    .WithMany(a => a.BillingOrders)
                    .HasForeignKey(e => e.BillingAddressId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.CancelledByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CancelledByUserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.ProductId);
                
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.ProductVariant)
                    .WithMany(pv => pv.OrderItems)
                    .HasForeignKey(e => e.ProductVariantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            // OrderStatusHistory
            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.OrderId, e.CreatedAt });
                
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderStatusHistories)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.ChangedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ChangedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
        
        private void ConfigureCartEntities(ModelBuilder modelBuilder)
        {
            // Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.SessionId);
                entity.HasIndex(e => e.UpdatedAt);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Carts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.ShippingAddress)
                    .WithMany()
                    .HasForeignKey(e => e.ShippingAddressId)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasOne(e => e.Coupon)
                    .WithMany()
                    .HasForeignKey(e => e.CouponId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            // CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.CartId, e.ProductId, e.ProductVariantId }).IsUnique();
                
                entity.HasOne(e => e.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(e => e.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.ProductVariant)
                    .WithMany(pv => pv.CartItems)
                    .HasForeignKey(e => e.ProductVariantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        
        private void ConfigureUserEntities(ModelBuilder modelBuilder)
        {
            // Address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.IsDefault });
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Addresses)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Wishlist
            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.IsDefault });
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Wishlists)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // WishlistItem
            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.WishlistId, e.ProductId, e.ProductVariantId }).IsUnique();
                
                entity.HasOne(e => e.Wishlist)
                    .WithMany(w => w.WishlistItems)
                    .HasForeignKey(e => e.WishlistId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.WishlistItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.ProductVariant)
                    .WithMany()
                    .HasForeignKey(e => e.ProductVariantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        
        private void ConfigureDiscountEntities(ModelBuilder modelBuilder)
        {
            // Discount
            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.IsActive, e.StartsAt, e.EndsAt });
            });
            
            // Coupon (inherits from Discount)
            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasIndex(e => e.Code).IsUnique();
            });
        }
        
        private void ConfigureSystemEntities(ModelBuilder modelBuilder)
        {
            // ShippingMethod
            modelBuilder.Entity<ShippingMethod>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IsActive);
            });
            
            // PaymentMethod
            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IsActive);
            });
            
            // Setting
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Key).IsUnique();
                entity.HasIndex(e => e.Category);
            });
        }
        
        private void ConfigureContentEntities(ModelBuilder modelBuilder)
        {
            // BlogPost
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");
                entity.HasIndex(e => new { e.IsPublished, e.PublishedAt });
                
                entity.HasOne(e => e.Author)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.BlogPosts)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            // BlogCategory
            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Slug).IsUnique().HasFilter("[Slug] IS NOT NULL");
            });
        }
        
        private void ConfigureAnalyticsEntities(ModelBuilder modelBuilder)
        {
            // Analytics
            modelBuilder.Entity<Analytics>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.EventType, e.CreatedAt });
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.SessionId);
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            // Newsletter
            modelBuilder.Entity<Newsletter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.IsActive);
            });
            
            // SeoPage
            modelBuilder.Entity<SeoPage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PagePath).IsUnique();
            });
        }
        
        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Additional performance indexes
            modelBuilder.Entity<Product>()
                .HasIndex(e => new { e.IsActive, e.IsFeatured, e.CreatedAt });
                
            modelBuilder.Entity<Order>()
                .HasIndex(e => new { e.Status, e.PaymentStatus, e.CreatedAt });
                
            modelBuilder.Entity<User>()
                .HasIndex(e => new { e.IsActive, e.LastLoginAt });
        }
        
        private void ConfigurePrecisions(ModelBuilder modelBuilder)
        {
            // Configure decimal precision
            var decimalProperties = new[]
            {
                typeof(Product), typeof(ProductVariant), typeof(Order), typeof(OrderItem),
                typeof(Cart), typeof(CartItem), typeof(Discount), typeof(ShippingMethod),
                typeof(PaymentTransaction)
            };
            
            foreach (var entityType in decimalProperties)
            {
                var entity = modelBuilder.Entity(entityType);
                var properties = entityType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));
                    
                foreach (var property in properties)
                {
                    entity.Property(property.Name).HasPrecision(18, 2);
                }
            }
        }
    }
} 