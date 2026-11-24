using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToolStore.Models;

namespace ToolStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                return RedirectToAction("Index", "Tools");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
