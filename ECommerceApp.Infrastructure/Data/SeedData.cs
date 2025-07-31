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
                if (await context.Categories.AnyAsync())
                {
                    logger.LogInformation("Database already contains data. Skipping seed.");
                    return;
                }

                logger.LogInformation("Seeding database...");

                // Seed in order of dependencies
                await SeedTagsAsync(context);
                await context.SaveChangesAsync();
                
                await SeedBrandsAsync(context);
                await context.SaveChangesAsync();
                
                await SeedCategoriesAsync(context);
                await context.SaveChangesAsync();
                
                await SeedProductsAsync(context);
                await context.SaveChangesAsync();
                
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
            try
            {
                // Execute raw SQL to truncate tables (safer approach)
                await context.Database.ExecuteSqlRawAsync(@"
                    TRUNCATE TABLE ""ProductTags"" RESTART IDENTITY CASCADE;
                    TRUNCATE TABLE ""ProductImages"" RESTART IDENTITY CASCADE;
                    TRUNCATE TABLE ""Products"" RESTART IDENTITY CASCADE;
                    TRUNCATE TABLE ""Categories"" RESTART IDENTITY CASCADE;
                    TRUNCATE TABLE ""Brands"" RESTART IDENTITY CASCADE;
                    TRUNCATE TABLE ""Tags"" RESTART IDENTITY CASCADE;
                ");
                
                Console.WriteLine("Database tables truncated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database reset: {ex.Message}");
                // Continue anyway
            }
        }

        private static async Task SeedTagsAsync(ApplicationDbContext context)
        {
            if (await context.Tags.AnyAsync()) return;

            var tags = new List<Tag>
            {
                new Tag { Name = "15% OFF", DisplayName = "15% OFF", CssClass = "bg-red1", Type = TagType.Discount },
                new Tag { Name = "20% OFF", DisplayName = "20% OFF", CssClass = "bg-red1", Type = TagType.Discount },
                new Tag { Name = "25% OFF", DisplayName = "25% OFF", CssClass = "bg-red1", Type = TagType.Discount },
                new Tag { Name = "best seller", DisplayName = "Best Seller", CssClass = "bg-blue1", Type = TagType.Status },
                new Tag { Name = "new", DisplayName = "New", CssClass = "bg-cyan1", Type = TagType.Status },
                new Tag { Name = "top rated", DisplayName = "Top Rated", CssClass = "bg-green1", Type = TagType.Status },
                new Tag { Name = "featured", DisplayName = "Featured", CssClass = "bg-yellow1", Type = TagType.Feature },
                new Tag { Name = "limited", DisplayName = "Limited Edition", CssClass = "bg-purple1", Type = TagType.Feature }
            };

            await context.Tags.AddRangeAsync(tags);
        }

        private static async Task SeedBrandsAsync(ApplicationDbContext context)
        {
            if (await context.Brands.AnyAsync()) return;

            var brands = new List<Brand>
            {
                new Brand { Name = "Apple", Description = "Technology company", Slug = "apple", LogoUrl = "/images/brands/apple.png" },
                new Brand { Name = "Samsung", Description = "Electronics company", Slug = "samsung", LogoUrl = "/images/brands/samsung.png" },
                new Brand { Name = "Sony", Description = "Electronics and entertainment", Slug = "sony", LogoUrl = "/images/brands/sony.png" },
                new Brand { Name = "LG", Description = "Home appliances and electronics", Slug = "lg", LogoUrl = "/images/brands/lg.png" },
                new Brand { Name = "Canon", Description = "Cameras and printers", Slug = "canon", LogoUrl = "/images/brands/canon.png" },
                new Brand { Name = "HP", Description = "Computers and printers", Slug = "hp", LogoUrl = "/images/brands/hp.png" },
                new Brand { Name = "Gigabyte", Description = "Computer hardware", Slug = "gigabyte", LogoUrl = "/images/brands/gigabyte.png" },
                new Brand { Name = "Durotan", Description = "Coffee machines", Slug = "durotan", LogoUrl = "/images/brands/durotan.png" },
                new Brand { Name = "Sceptre", Description = "TVs and monitors", Slug = "sceptre", LogoUrl = "/images/brands/sceptre.png" },
                new Brand { Name = "Sharp", Description = "Electronics", Slug = "sharp", LogoUrl = "/images/brands/sharp.png" }
            };

            await context.Brands.AddRangeAsync(brands);
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Electronic devices and accessories", Slug = "electronics", ImageUrl = "/images/categories/electronics.png" },
                new Category { Name = "Computers & Gaming", Description = "Computers, laptops and gaming equipment", Slug = "computers-gaming", ImageUrl = "/images/categories/computers.png" },
                new Category { Name = "Home & Kitchen", Description = "Home appliances and kitchen equipment", Slug = "home-kitchen", ImageUrl = "/images/categories/home.png" },
                new Category { Name = "TV & Audio", Description = "Televisions and audio equipment", Slug = "tv-audio", ImageUrl = "/images/categories/tv.png" },
                new Category { Name = "Cameras", Description = "Digital cameras and accessories", Slug = "cameras", ImageUrl = "/images/categories/cameras.png" },
                new Category { Name = "Mobile & Tablets", Description = "Smartphones and tablets", Slug = "mobile-tablets", ImageUrl = "/images/categories/mobile.png" }
            };

            await context.Categories.AddRangeAsync(categories);
        }

        private static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            // Get categories and brands for relationships
            var electronics = await context.Categories.FirstAsync(c => c.Slug == "electronics");
            var computers = await context.Categories.FirstAsync(c => c.Slug == "computers-gaming");
            var tv = await context.Categories.FirstAsync(c => c.Slug == "tv-audio");
            var home = await context.Categories.FirstAsync(c => c.Slug == "home-kitchen");
            var cameras = await context.Categories.FirstAsync(c => c.Slug == "cameras");

            var durotan = await context.Brands.FirstAsync(b => b.Slug == "durotan");
            var canon = await context.Brands.FirstAsync(b => b.Slug == "canon");
            var sharp = await context.Brands.FirstAsync(b => b.Slug == "sharp");
            var gigabyte = await context.Brands.FirstAsync(b => b.Slug == "gigabyte");
            var sceptre = await context.Brands.FirstAsync(b => b.Slug == "sceptre");

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Durotan Manual Espresso Machine, Coffee Maker",
                    Description = "High-quality manual espresso machine for perfect coffee",
                    ShortDescription = "Manual espresso machine with premium build quality",
                    Price = 489.00M,
                    ComparePrice = 619.00M,
                    Stock = 15,
                    SKU = "DUR-ESP-001",
                    CategoryId = home.Id,
                    BrandId = durotan.Id,
                    IsActive = true,
                    IsFeatured = true,
                    HasInstallment = true,
                    AverageRating = 5.0M,
                    ReviewCount = 34,
                    Slug = "durotan-manual-espresso-machine",
                    MetaTitle = "Durotan Manual Espresso Machine - Premium Coffee Maker",
                    SortOrder = 1
                },
                new Product
                {
                    Name = "Canon DSLR Camera EOS II, Only Body",
                    Description = "Professional DSLR camera for photography enthusiasts",
                    ShortDescription = "High-resolution DSLR camera body",
                    Price = 579.00M,
                    ComparePrice = 819.00M,
                    Stock = 8,
                    SKU = "CAN-EOS-002",
                    CategoryId = cameras.Id,
                    BrandId = canon.Id,
                    IsActive = true,
                    IsFeatured = true,
                    HasInstallment = false,
                    AverageRating = 4.0M,
                    ReviewCount = 5,
                    Slug = "canon-dslr-camera-eos-ii",
                    MetaTitle = "Canon DSLR Camera EOS II - Professional Photography",
                    SortOrder = 2
                },
                new Product
                {
                    Name = "Sharp 49\" Class FHD (1080p) Android LED TV",
                    Description = "Large screen Android TV with Full HD resolution",
                    ShortDescription = "49-inch Android LED TV with smart features",
                    Price = 3029.50M,
                    ComparePrice = null,
                    Stock = 5,
                    SKU = "SHA-TV-049",
                    CategoryId = tv.Id,
                    BrandId = sharp.Id,
                    IsActive = true,
                    IsFeatured = true,
                    HasInstallment = false,
                    AverageRating = 0,
                    ReviewCount = 0,
                    Slug = "sharp-49-android-led-tv",
                    MetaTitle = "Sharp 49\" FHD Android LED TV - Smart Entertainment",
                    SortOrder = 3
                },
                new Product
                {
                    Name = "Gigabyte PC Gaming Case, Core i7, 32GB Ram",
                    Description = "High-performance gaming computer with latest specifications",
                    ShortDescription = "Gaming PC with Core i7 processor and 32GB RAM",
                    Price = 1279.00M,
                    ComparePrice = null,
                    Stock = 3,
                    SKU = "GIG-PC-I7",
                    CategoryId = computers.Id,
                    BrandId = gigabyte.Id,
                    IsActive = true,
                    IsFeatured = true,
                    HasInstallment = true,
                    AverageRating = 5.0M,
                    ReviewCount = 2,
                    Slug = "gigabyte-gaming-pc-i7-32gb",
                    MetaTitle = "Gigabyte Gaming PC - Core i7 32GB RAM",
                    SortOrder = 4
                },
                new Product
                {
                    Name = "Sceptre 32\" Internet TV",
                    Description = "Smart TV with internet connectivity and streaming capabilities",
                    ShortDescription = "32-inch smart TV with internet features",
                    Price = 610.00M,
                    ComparePrice = null,
                    Stock = 12,
                    SKU = "SCE-TV-032",
                    CategoryId = tv.Id,
                    BrandId = sceptre.Id,
                    IsActive = true,
                    IsFeatured = true,
                    HasInstallment = true,
                    AverageRating = 5.0M,
                    ReviewCount = 12,
                    Slug = "sceptre-32-internet-tv",
                    MetaTitle = "Sceptre 32\" Smart Internet TV",
                    SortOrder = 5
                }
            };

            await context.Products.AddRangeAsync(products);
        }

        private static async Task SeedProductTagsAsync(ApplicationDbContext context)
        {
            if (await context.ProductTags.AnyAsync()) return;

            // Get products and tags
            var products = await context.Products.ToListAsync();
            var discountTag = await context.Tags.FirstAsync(t => t.Name == "15% OFF");
            var bestSellerTag = await context.Tags.FirstAsync(t => t.Name == "best seller");
            var newTag = await context.Tags.FirstAsync(t => t.Name == "new");
            var topRatedTag = await context.Tags.FirstAsync(t => t.Name == "top rated");

            var productTags = new List<ProductTag>();

            // Durotan Espresso Machine - 15% OFF + best seller
            var espressoMachine = products.First(p => p.SKU == "DUR-ESP-001");
            productTags.Add(new ProductTag { ProductId = espressoMachine.Id, TagId = discountTag.Id });
            productTags.Add(new ProductTag { ProductId = espressoMachine.Id, TagId = bestSellerTag.Id });

            // Canon Camera - 15% OFF + best seller
            var camera = products.First(p => p.SKU == "CAN-EOS-002");
            productTags.Add(new ProductTag { ProductId = camera.Id, TagId = discountTag.Id });
            productTags.Add(new ProductTag { ProductId = camera.Id, TagId = bestSellerTag.Id });

            // Sharp TV - new + best seller
            var sharpTv = products.First(p => p.SKU == "SHA-TV-049");
            productTags.Add(new ProductTag { ProductId = sharpTv.Id, TagId = newTag.Id });
            productTags.Add(new ProductTag { ProductId = sharpTv.Id, TagId = bestSellerTag.Id });

            // Gigabyte PC - best seller
            var gamingPc = products.First(p => p.SKU == "GIG-PC-I7");
            productTags.Add(new ProductTag { ProductId = gamingPc.Id, TagId = bestSellerTag.Id });

            // Sceptre TV - best seller + top rated
            var sceptreTv = products.First(p => p.SKU == "SCE-TV-032");
            productTags.Add(new ProductTag { ProductId = sceptreTv.Id, TagId = bestSellerTag.Id });
            productTags.Add(new ProductTag { ProductId = sceptreTv.Id, TagId = topRatedTag.Id });

            await context.ProductTags.AddRangeAsync(productTags);
        }
    }
} 