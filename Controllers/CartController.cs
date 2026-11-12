using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ToolStore.Data;
using ToolStore.Models;

namespace ToolStore.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Вспомогательный метод для получения корзины из сессии
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var tools = await _context.Tools.ToListAsync();

            var cartView = cart.Select(c => new
            {
                Tool = tools.FirstOrDefault(t => t.Id == c.ToolId),
                Quantity = c.Quantity
            }).Where(x => x.Tool != null).ToList();

            ViewBag.TotalSum = cartView.Sum(x => x.Tool.Price * x.Quantity);
            return View(cartView);
        }

        // POST: /Cart/Add/5
        [HttpPost]
        public IActionResult Add(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ToolId == id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem { ToolId = id, Quantity = 1 });
            }
            SaveCart(cart);
            return RedirectToAction("Index", "Tools");
        }

        // POST: /Cart/Remove/5
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ToolId == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // POST: /Cart/Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout(string deliveryAddress)
        {
            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction("Index");

            foreach (var item in cart)
            {
                var tool = await _context.Tools.FindAsync(item.ToolId);
                if (tool == null || tool.QuantityInStock < item.Quantity)
                {
                    ViewBag.Error = $"Недостаточно товара '{tool?.Name}' на складе!";
                    return RedirectToAction("Index");
                }
            }

            // Создание заказа
            var order = new Order
            {
                Date = DateTime.Now,
                DeliveryAddress = deliveryAddress,
                Status = "Оформлен",
                UserId = 2
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Создание позиций и списывание остатков
            foreach (var item in cart)
            {
                var tool = await _context.Tools.FindAsync(item.ToolId);
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ToolId = item.ToolId,
                    Quantity = item.Quantity
                });

                tool.QuantityInStock -= item.Quantity; // Списание
            }
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Tools");
        }
    }
}