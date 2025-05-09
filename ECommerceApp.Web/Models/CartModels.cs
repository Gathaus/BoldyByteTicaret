using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Web.Models
{
    public class AddToCartModel
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
    
    public class UpdateCartItemModel
    {
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
} 