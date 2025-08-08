using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Web.Models
{
    public class HomeIndexViewModel
    {
        // Popular Categories section
        public List<Category> Categories { get; set; } = new List<Category>();

        // Featured Products for Best Seller section
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();

        // Best Seller Products (products with highest sales)
        public List<Product> BestSellerProducts { get; set; } = new List<Product>();

        // Popular Brands
        public List<Brand> PopularBrands { get; set; } = new List<Brand>();

        // Suggest Today Products (featured + top rated)
        public List<Product> SuggestTodayProducts { get; set; } = new List<Product>();

        // Top Rated Products
        public List<Product> TopRatedProducts { get; set; } = new List<Product>();

        // Discount Products (products with compare price)
        public List<Product> DiscountProducts { get; set; } = new List<Product>();

        // New Products (recently published)
        public List<Product> NewProducts { get; set; } = new List<Product>();

        // Tags for labels
        public List<Tag> Tags { get; set; } = new List<Tag>();

        // Categories for Best Seller tabs (limit to main categories)
        public List<Category> MainCategories { get; set; } = new List<Category>();

        // Trending Search Keywords
        public List<string> TrendingSearches { get; set; } = new List<string>();

        // Best Weekly Deals Products (products with highest discount or special weekly offers)
        public List<Product> BestWeeklyDeals { get; set; } = new List<Product>();

        // Helper methods for view logic
        public string GetDiscountPercentage(Product product)
        {
            if (product.ComparePrice.HasValue && product.ComparePrice > product.Price)
            {
                var discount = (product.ComparePrice.Value - product.Price) / product.ComparePrice.Value * 100;
                return $"{Math.Round(discount)}% OFF";
            }
            return string.Empty;
        }

        public bool HasDiscount(Product product)
        {
            return product.ComparePrice.HasValue && product.ComparePrice > product.Price;
        }

        public string GetRatingStars(double rating)
        {
            var fullStars = (int)Math.Floor(rating);
            var hasHalfStar = rating - fullStars >= 0.5;
            var emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

            var stars = string.Empty;
            
            // Full stars
            for (int i = 0; i < fullStars; i++)
            {
                stars += "<i class='fa fa-star'></i>";
            }

            // Half star
            if (hasHalfStar)
            {
                stars += "<i class='fa fa-star-half-o'></i>";
            }

            // Empty stars
            for (int i = 0; i < emptyStars; i++)
            {
                stars += "<i class='fa fa-star-o'></i>";
            }

            return stars;
        }

        public string GetProductLabels(Product product)
        {
            var labels = new List<string>();

            if (HasDiscount(product))
            {
                labels.Add($"<span class='badge bg-red1'>{GetDiscountPercentage(product)}</span>");
            }

            if (product.IsFeatured)
            {
                labels.Add("<span class='badge bg-blue1'>featured</span>");
            }

            if (product.PublishedAt.HasValue && product.PublishedAt.Value >= DateTime.UtcNow.AddDays(-30))
            {
                labels.Add("<span class='badge bg-cyan1'>new</span>");
            }

            if (product.SalesCount > 100) // Assuming best seller criteria
            {
                labels.Add("<span class='badge bg-blue1'>best seller</span>");
            }

            return string.Join(" ", labels);
        }

        public string FormatPrice(decimal price)
        {
            return price.ToString("C");
        }

        public bool HasReviews(Product product)
        {
            return product.ReviewCount > 0;
        }

        public string GetInstallmentText(Product product)
        {
            // Basic installment calculation - can be customized
            var installmentPrice = product.Price / 12;
            return $"or 12 installments of {installmentPrice:C}";
        }

        public bool HasInstallment(Product product)
        {
            // You can add logic here to determine if product supports installment
            return product.Price > 100; // Example: only products over $100 support installment
        }

        // Best Weekly Deals specific helper methods
        public bool IsWeeklyDeal(Product product)
        {
            return HasDiscount(product) || product.IsFeatured;
        }

        public string GetWeeklyDealBadge(Product product)
        {
            if (HasDiscount(product))
            {
                return $"<span class='weekly-deal-badge discount'>{GetDiscountPercentage(product)}</span>";
            }
            else if (product.IsFeatured)
            {
                return "<span class='weekly-deal-badge featured'>Weekly Deal</span>";
            }
            return string.Empty;
        }

        public decimal GetWeeklyDealSavings(Product product)
        {
            if (HasDiscount(product))
            {
                return product.ComparePrice!.Value - product.Price;
            }
            return 0;
        }

        public string GetWeeklyDealSavingsText(Product product)
        {
            var savings = GetWeeklyDealSavings(product);
            if (savings > 0)
            {
                return $"Save {savings:C}";
            }
            return string.Empty;
        }
    }
}