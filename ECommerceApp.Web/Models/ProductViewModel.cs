namespace ECommerceApp.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? ComparePrice { get; set; }
        public int Stock { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<ProductImageViewModel> ProductImages { get; set; }
        public string StockStatus { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool HasFreeGift { get; set; }

        public decimal DiscountAmount => ComparePrice.HasValue && ComparePrice > Price ? ComparePrice.Value - Price : 0;
        public decimal DiscountPercentage => ComparePrice.HasValue && ComparePrice > Price ? ((ComparePrice.Value - Price) / ComparePrice.Value) * 100 : 0;
        public bool HasDiscount => DiscountAmount > 0;
    }

    public class ProductImageViewModel
    {
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}