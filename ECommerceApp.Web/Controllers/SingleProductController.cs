using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
    public class SingleProductController : Controller
    {
        public IActionResult Index(int id)
        {
            // Here you would fetch the product by id
            return View("Single_product");
        }

        public IActionResult Single_product()
        {
            return View();
        }

        public IActionResult Single_product_pay()
        {
            return View();
        }
    }
} 