using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Web.Models
{
    public class CreateOrderModel
    {
        [Required]
        public int AddressId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; }
    }
    
    public class UpdateOrderStatusModel
    {
        [Required]
        [StringLength(20)]
        public string Status { get; set; }
    }
} 