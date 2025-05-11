using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View("Privacy");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
} 