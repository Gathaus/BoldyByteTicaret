using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Web.Models;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Web.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;

        public NavbarViewComponent(ICategoryService categoryService, ICartService cartService)
        {
            _categoryService = categoryService;
            _cartService = cartService;
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
                
                // Sepet sayısını dinamik olarak al
                var sessionId = HttpContext.Session.Id;
                navbarViewModel.CartItemCount = await _cartService.GetCartItemCountAsync(sessionId);
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