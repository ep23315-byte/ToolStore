using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolStore.Data;
using ToolStore.Models;

namespace ToolStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Неверный логин или пароль";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string name, string login, string password, string role)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (existingUser != null)
            {
                ViewBag.Error = "Пользователь с таким логином уже существует";
                return View();
            }

            var newUser = new User
            {
                Name = name,
                Login = login,
                Password = password,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UserRole", newUser.Role);
            HttpContext.Session.SetString("UserName", newUser.Name);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}