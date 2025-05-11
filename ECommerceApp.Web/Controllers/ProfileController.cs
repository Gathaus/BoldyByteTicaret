using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Profile");
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
} 