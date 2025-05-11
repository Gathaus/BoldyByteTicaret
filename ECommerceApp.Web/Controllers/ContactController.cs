using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View("Contact");
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
} 