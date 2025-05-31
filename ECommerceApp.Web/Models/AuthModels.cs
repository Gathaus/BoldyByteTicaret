using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }
    }
    
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
    
    public class UpdateProfileModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }
    }
    
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public required string CurrentPassword { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }
        
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
} 