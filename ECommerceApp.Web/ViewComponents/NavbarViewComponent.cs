using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Web.Models;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Web.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public NavbarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var navbarViewModel = new NavbarViewModel();
            
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                navbarViewModel.MainCategories = categories?.Where(c => c.IsActive && !c.ParentId.HasValue)
                    .OrderBy(c => c.SortOrder)
                    .ToList() ?? new List<ECommerceApp.Domain.Entities.Category>();
                
                // Sepet sayısı ve kullanıcı bilgileri burada eklenebilir
                navbarViewModel.CartItemCount = 2; // Örnek değer
                navbarViewModel.IsUserLoggedIn = User.Identity?.IsAuthenticated ?? false;
                navbarViewModel.UserName = User.Identity?.Name;
            }
            catch (Exception ex)
            {
                // Log error
                navbarViewModel.MainCategories = new List<ECommerceApp.Domain.Entities.Category>();
            }
            
            return View(navbarViewModel);
        }
    }
}