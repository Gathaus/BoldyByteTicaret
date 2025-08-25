using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Web.Models
{
    public class NavbarViewModel
    {
        // Ana kategoriler navbar dropdown için
        public List<Category> MainCategories { get; set; } = new List<Category>();
        
        // Sepetteki ürün sayısı (opsiyonel)
        public int CartItemCount { get; set; } = 0;
        
        // Kullanıcı giriş durumu (opsiyonel)
        public bool IsUserLoggedIn { get; set; } = false;
        
        // Kullanıcı adı (opsiyonel)
        public string? UserName { get; set; }
    }
}