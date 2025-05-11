using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View("About");
        }

        public IActionResult About()
        {
            return View();
        }
    }
} 