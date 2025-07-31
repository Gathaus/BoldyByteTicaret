using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerceApp.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, bool resetData = false)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Reset data if requested
                if (resetData)
                {
                    logger.LogInformation("Resetting database data...");
                    await ResetDatabaseAsync(context);
                }

                // Check if data already exists
                if (await context.Categories.AnyAsync() || await context.Products.AnyAsync())
                {
                    logger.LogInformation("Database already contains data. Skipping seed.");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // Seed in order of dependencies
                await SeedTagsAsync(context);
                await SeedBrandsAsync(context);
                await SeedCategoriesAsync(context);
                await SeedProductsAsync(context);
                await SeedProductTagsAsync(context);

                await context.SaveChangesAsync();
                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task ResetDatabaseAsync(ApplicationDbContext context)
        {
            // Remove all data in dependency order
            context.ProductTags.RemoveRange(context.ProductTags);
            context.ProductImages.RemoveRange(context.ProductImages);
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            context.Brands.RemoveRange(context.Brands);
            context.Tags.RemoveRange(context.Tags);
            
            await context.SaveChangesAsync();
        }

        private static async Task SeedTagsAsync(ApplicationDbContext context)
        {
            var tags = new List<Tag>
            {
                // Discount tags
                new Tag { Name = "5% OFF", DisplayName = "5% OFF", CssClass = "bg-red1", Type = TagType.Discount },
                new Tag { Name = "10% OFF", DisplayName = "10% OFF", CssClass = "bg-red1", Type = TagType.Discount },
                new Tag { Name = "15% OFF", DisplayName = "15% OFF", CssClass = "bg-red1", Type = TagType.Discount },
                new Tag { Name = "20% OFF", DisplayName = "20% OFF", CssClass = "bg-red1", Type = TagType.Discount },

                // Status tags
                new Tag { Name = "best seller", DisplayName = "Best Seller", CssClass = "bg-blue1", Type = TagType.Status },
                new Tag { Name = "new", DisplayName = "New", CssClass = "bg-cyan1", Type = TagType.Status },
                new Tag { Name = "top rated", DisplayName = "Top Rated", CssClass = "bg-green1", Type = TagType.Status },
                new Tag { Name = "out of stock", DisplayName = "Out of Stock", CssClass = "bg-dark", Type = TagType.Status },

                // Feature tags
                new Tag { Name = "0% installment", DisplayName = "0% Installment", CssClass = "bg-orange", Type = TagType.Feature },

                // Seasonal tags
                new Tag { Name = "pre-order", DisplayName = "Pre-Order", CssClass = "bg-purple", Type = TagType.Seasonal },
                new Tag { Name = "limited time", DisplayName = "Limited Time", CssClass = "bg-red1", Type = TagType.Seasonal }
            };

            await context.Tags.AddRangeAsync(tags);
        }

        private static async Task SeedBrandsAsync(ApplicationDbContext context)
        {
            var brands = new List<Brand>
            {
                new Brand 
                { 
                    Name = "Samsung", 
                    Description = "Leading technology company", 
                    Slug = "samsung",
                    LogoUrl = "/images/brands/samsung.png",
                    Website = "https://samsung.com",
                    IsActive = true,
                    SortOrder = 1
                },
                new Brand 
                { 
                    Name = "Apple", 
                    Description = "Innovation in technology", 
                    Slug = "apple",
                    LogoUrl = "/images/brands/apple.png",
                    Website = "https://apple.com",
                    IsActive = true,
                    SortOrder = 2
                },
                new Brand 
                { 
                    Name = "Sony", 
                    Description = "Electronics and entertainment", 
                    Slug = "sony",
                    LogoUrl = "/images/brands/sony.png",
                    Website = "https://sony.com",
                    IsActive = true,
                    SortOrder = 3
                },
                new Brand 
                { 
                    Name = "LG", 
                    Description = "Life's Good with innovative technology", 
                    Slug = "lg",
                    LogoUrl = "/images/brands/lg.png",
                    Website = "https://lg.com",
                    IsActive = true,
                    SortOrder = 4
                },
                new Brand 
                { 
                    Name = "Canon", 
                    Description = "Imaging and optical solutions", 
                    Slug = "canon",
                    LogoUrl = "/images/brands/canon.png",
                    Website = "https://canon.com",
                    IsActive = true,
                    SortOrder = 5
                },
                new Brand 
                { 
                    Name = "Gigabyte", 
                    Description = "Computer hardware manufacturer", 
                    Slug = "gigabyte",
                    LogoUrl = "/images/brands/gigabyte.png",
                    Website = "https://gigabyte.com",
                    IsActive = true,
                    SortOrder = 6
                },
                new Brand 
                { 
                    Name = "Marshall", 
                    Description = "Audio equipment and speakers", 
                    Slug = "marshall",
                    LogoUrl = "/images/brands/marshall.png",
                    Website = "https://marshallheadphones.com",
                    IsActive = true,
                    SortOrder = 7
                },
                new Brand 
                { 
                    Name = "Bose", 
                    Description = "Premium audio systems", 
                    Slug = "bose",
                    LogoUrl = "/images/brands/bose.png",
                    Website = "https://bose.com",
                    IsActive = true,
                    SortOrder = 8
                }
            };

            await context.Brands.AddRangeAsync(brands);
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            var categories = new List<Category>
            {
                // Main categories from Index.cshtml
                new Category 
                { 
                    Name = "Televisions", 
                    Description = "Smart TVs, LED, OLED and more", 
                    Slug = "televisions",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat1.png",
                    IsActive = true,
                    SortOrder = 1
                },
                new Category 
                { 
                    Name = "PC Gaming", 
                    Description = "Gaming computers and accessories", 
                    Slug = "pc-gaming",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat2.png",
                    IsActive = true,
                    SortOrder = 2
                },
                new Category 
                { 
                    Name = "Computers", 
                    Description = "Desktop and laptop computers", 
                    Slug = "computers",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat3.png",
                    IsActive = true,
                    SortOrder = 3
                },
                new Category 
                { 
                    Name = "Cameras", 
                    Description = "DSLR, mirrorless and digital cameras", 
                    Slug = "cameras",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat4.png",
                    IsActive = true,
                    SortOrder = 4
                },
                new Category 
                { 
                    Name = "Gadgets", 
                    Description = "Electronic gadgets and accessories", 
                    Slug = "gadgets",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat5.png",
                    IsActive = true,
                    SortOrder = 5
                },
                new Category 
                { 
                    Name = "Smart Home", 
                    Description = "Smart home devices and automation", 
                    Slug = "smart-home",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat6.png",
                    IsActive = true,
                    SortOrder = 6
                },
                new Category 
                { 
                    Name = "Sport Equipments", 
                    Description = "Sports and fitness equipment", 
                    Slug = "sport-equipments",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat7.png",
                    IsActive = true,
                    SortOrder = 7
                },
                new Category 
                { 
                    Name = "Speakers", 
                    Description = "Audio speakers and sound systems", 
                    Slug = "speakers",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat8.png",
                    IsActive = true,
                    SortOrder = 8
                },
                new Category 
                { 
                    Name = "Laptops", 
                    Description = "Portable computers and notebooks", 
                    Slug = "laptops",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat9.png",
                    IsActive = true,
                    SortOrder = 9
                },
                new Category 
                { 
                    Name = "Mobiles & Tablets", 
                    Description = "Smartphones and tablet devices", 
                    Slug = "mobiles-tablets",
                    ImageUrl = "~/swoo/home_electronic/assets/img/cat/cat10.png",
                    IsActive = true,
                    SortOrder = 10
                }
            };

            await context.Categories.AddRangeAsync(categories);
        }

        private static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            // Get categories and brands for foreign keys
            var tvCategory = await context.Categories.FirstAsync(c => c.Name == "Televisions");
            var gamingCategory = await context.Categories.FirstAsync(c => c.Name == "PC Gaming");
            var computerCategory = await context.Categories.FirstAsync(c => c.Name == "Computers");
            var cameraCategory = await context.Categories.FirstAsync(c => c.Name == "Cameras");
            var smartHomeCategory = await context.Categories.FirstAsync(c => c.Name == "Smart Home");
            var speakerCategory = await context.Categories.FirstAsync(c => c.Name == "Speakers");
            var sportCategory = await context.Categories.FirstAsync(c => c.Name == "Sport Equipments");
            var gadgetCategory = await context.Categories.FirstAsync(c => c.Name == "Gadgets");

            var samsungBrand = await context.Brands.FirstAsync(b => b.Name == "Samsung");
            var appleBrand = await context.Brands.FirstAsync(b => b.Name == "Apple");
            var canonBrand = await context.Brands.FirstAsync(b => b.Name == "Canon");
            var gigabyteBrand = await context.Brands.FirstAsync(b => b.Name == "Gigabyte");
            var marshallBrand = await context.Brands.FirstAsync(b => b.Name == "Marshall");
            var boseBrand = await context.Brands.FirstAsync(b => b.Name == "Bose");

            var products = new List<Product>
            {
                // Products from Index.cshtml
                new Product
                {
                    Name = "Durotan Manual Espresso Machine, Coffee Maker",
                    Description = "Professional espresso machine for home use",
                    ShortDescription = "Manual espresso machine with premium features",
                    Price = 489.00m,
                    ComparePrice = 619.00m,
                    Stock = 50,
                    CategoryId = smartHomeCategory.Id,
                    BrandId = null, // Generic brand
                    HasInstallment = true,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 34,
                    SKU = "ESP-001",
                    Slug = "durotan-espresso-machine",
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Canon DSLR Camera EOS II, Only Body",
                    Description = "Professional DSLR camera for photography enthusiasts",
                    ShortDescription = "High-quality DSLR camera body",
                    Price = 579.00m,
                    ComparePrice = 819.00m,
                    Stock = 25,
                    CategoryId = cameraCategory.Id,
                    BrandId = canonBrand.Id,
                    HasInstallment = false,
                    Status = ProductStatus.Active,
                    AverageRating = 4,
                    ReviewCount = 5,
                    SKU = "CAM-001",
                    Slug = "canon-dslr-camera-eos-ii",
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Samsung 49\" Class FHD (1080p) Android LED TV",
                    Description = "Smart Full HD Android TV with Google Assistant",
                    ShortDescription = "49-inch Smart Android TV",
                    Price = 3029.50m,
                    Stock = 15,
                    CategoryId = tvCategory.Id,
                    BrandId = samsungBrand.Id,
                    HasInstallment = false,
                    Status = ProductStatus.Active,
                    AverageRating = 0,
                    ReviewCount = 0,
                    SKU = "TV-001",
                    Slug = "samsung-49-android-led-tv",
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Gigabyte PC Gaming Case, Core i7, 32GB Ram",
                    Description = "High-performance gaming computer with latest specs",
                    ShortDescription = "Gaming PC with Core i7 and 32GB RAM",
                    Price = 1279.00m,
                    Stock = 8,
                    CategoryId = gamingCategory.Id,
                    BrandId = gigabyteBrand.Id,
                    HasInstallment = true,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 2,
                    SKU = "PC-001",
                    Slug = "gigabyte-gaming-pc-i7-32gb",
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Sceptre 32\" Internet TV",
                    Description = "Smart Internet TV with streaming capabilities",
                    ShortDescription = "32-inch Smart Internet TV",
                    Price = 610.00m,
                    Stock = 20,
                    CategoryId = tvCategory.Id,
                    BrandId = null,
                    HasInstallment = true,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 12,
                    SKU = "TV-002",
                    Slug = "sceptre-32-internet-tv",
                    IsActive = true,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Air Purifier with True HEPA H14 Filter",
                    Description = "Advanced air purification system for cleaner air",
                    ShortDescription = "HEPA H14 air purifier",
                    Price = 489.00m,
                    ComparePrice = 619.00m,
                    Stock = 30,
                    CategoryId = smartHomeCategory.Id,
                    BrandId = null,
                    HasInstallment = true,
                    Status = ProductStatus.Active,
                    AverageRating = 4,
                    ReviewCount = 5,
                    SKU = "AIR-001",
                    Slug = "air-purifier-hepa-h14",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Apple iMac All-in-one Desktop Computer with M1",
                    Description = "Powerful all-in-one computer with Apple M1 chip",
                    ShortDescription = "iMac with M1 chip",
                    Price = 2725.00m,
                    Stock = 12,
                    CategoryId = computerCategory.Id,
                    BrandId = appleBrand.Id,
                    HasInstallment = false,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 2,
                    SKU = "IMAC-001",
                    Slug = "apple-imac-m1-desktop",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "aPod LTE+GPS Silver Grey",
                    Description = "Tablet with LTE connectivity and GPS",
                    ShortDescription = "LTE tablet with GPS",
                    Price = 524.00m,
                    Stock = 18,
                    CategoryId = gadgetCategory.Id,
                    BrandId = null,
                    HasInstallment = false,
                    Status = ProductStatus.Active,
                    AverageRating = 3,
                    ReviewCount = 1,
                    SKU = "TAB-001",
                    Slug = "apod-lte-gps-tablet",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "TORO Smart Self Balancing Transporter Scooter",
                    Description = "Electric self-balancing scooter for personal transport",
                    ShortDescription = "Smart self-balancing scooter",
                    Price = 1512.90m,
                    Stock = 5,
                    CategoryId = sportCategory.Id,
                    BrandId = null,
                    HasInstallment = false,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 2,
                    SKU = "SCOOT-001",
                    Slug = "toro-self-balancing-scooter",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Marshall Stanmore II Wireless Bluetooth Speaker",
                    Description = "Premium wireless speaker with classic Marshall design",
                    ShortDescription = "Wireless Bluetooth speaker",
                    Price = 325.00m,
                    Stock = 22,
                    CategoryId = speakerCategory.Id,
                    BrandId = marshallBrand.Id,
                    HasInstallment = true,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 2,
                    SKU = "SPEAK-001",
                    Slug = "marshall-stanmore-ii-speaker",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Bose SoundLink III Speaker",
                    Description = "Portable wireless speaker with superior sound quality",
                    ShortDescription = "Portable Bluetooth speaker",
                    Price = 149.00m,
                    Stock = 35,
                    CategoryId = speakerCategory.Id,
                    BrandId = boseBrand.Id,
                    HasInstallment = false,
                    Status = ProductStatus.Active,
                    AverageRating = 5,
                    ReviewCount = 12,
                    SKU = "SPEAK-002",
                    Slug = "bose-soundlink-iii-speaker",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "TCL OLED 4K Ultra HD, 47\" Smart TV",
                    Description = "Premium OLED TV with 4K Ultra HD resolution",
                    ShortDescription = "47-inch OLED 4K Smart TV",
                    Price = 1205.00m,
                    Stock = 0, // Out of stock
                    CategoryId = tvCategory.Id,
                    BrandId = null,
                    HasInstallment = true,
                    Status = ProductStatus.OutOfStock,
                    AverageRating = 0,
                    ReviewCount = 0,
                    SKU = "TV-003",
                    Slug = "tcl-oled-4k-47-smart-tv",
                    IsActive = true,
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                }
            };

            await context.Products.AddRangeAsync(products);
        }

        private static async Task SeedProductTagsAsync(ApplicationDbContext context)
        {
            // Get all products and tags
            var products = await context.Products.ToListAsync();
            var tags = await context.Tags.ToListAsync();

            var productTags = new List<ProductTag>();

            // Assign tags based on product characteristics
            foreach (var product in products)
            {
                // Add discount tags if product has compare price
                if (product.ComparePrice.HasValue && product.ComparePrice > product.Price)
                {
                    var discountPercent = (int)Math.Round(((product.ComparePrice.Value - product.Price) / product.ComparePrice.Value) * 100);
                    var discountTag = tags.FirstOrDefault(t => t.Name == $"{discountPercent}% OFF" || 
                                                              (discountPercent >= 15 && t.Name == "15% OFF") ||
                                                              (discountPercent >= 10 && discountPercent < 15 && t.Name == "10% OFF") ||
                                                              (discountPercent >= 5 && discountPercent < 10 && t.Name == "5% OFF"));
                    if (discountTag != null)
                    {
                        productTags.Add(new ProductTag { ProductId = product.Id, TagId = discountTag.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    }
                }

                // Add best seller tag for featured products with good ratings
                if (product.IsFeatured && product.AverageRating >= 4)
                {
                    var bestSellerTag = tags.First(t => t.Name == "best seller");
                    productTags.Add(new ProductTag { ProductId = product.Id, TagId = bestSellerTag.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }

                // Add top rated tag for products with 5-star rating
                if (product.AverageRating == 5 && product.ReviewCount > 10)
                {
                    var topRatedTag = tags.First(t => t.Name == "top rated");
                    productTags.Add(new ProductTag { ProductId = product.Id, TagId = topRatedTag.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }

                // Add new tag for recently created products
                if (product.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                {
                    var newTag = tags.First(t => t.Name == "new");
                    productTags.Add(new ProductTag { ProductId = product.Id, TagId = newTag.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }

                // Add out of stock tag
                if (product.Status == ProductStatus.OutOfStock)
                {
                    var outOfStockTag = tags.First(t => t.Name == "out of stock");
                    productTags.Add(new ProductTag { ProductId = product.Id, TagId = outOfStockTag.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }

                // Add installment tag for products with installment support
                if (product.HasInstallment)
                {
                    var installmentTag = tags.First(t => t.Name == "0% installment");
                    productTags.Add(new ProductTag { ProductId = product.Id, TagId = installmentTag.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
            }

            await context.ProductTags.AddRangeAsync(productTags);
        }
    }
} 