using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View("Checkout");
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
} 