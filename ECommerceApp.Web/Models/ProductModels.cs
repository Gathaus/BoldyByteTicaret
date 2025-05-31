using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Web.Models
{
    public class ProductCreateModel
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        
        [Required]
        [StringLength(1000)]
        public required string Description { get; set; }
        
        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, 1000)]
        public int Stock { get; set; }
        
        [Url]
        public string? ImageUrl { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
    }
    
    public class ProductUpdateModel : ProductCreateModel
    {
        [Required]
        public int Id { get; set; }
    }
} 