using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Web.Models
{
    public class CreateOrderModel
    {
        [Required]
        public int ShippingAddressId { get; set; }
        
        [Required]
        public int BillingAddressId { get; set; }
        
        [Required]
        [StringLength(20)]
        public required string PaymentMethod { get; set; }
    }
    
    public class UpdateOrderStatusModel
    {
        [Required]
        [StringLength(20)]
        public required string Status { get; set; }
    }
} 