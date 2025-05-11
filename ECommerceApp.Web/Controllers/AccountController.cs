using Microsoft.AspNetCore.Mvc;
using System;

namespace ECommerceApp.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
} 