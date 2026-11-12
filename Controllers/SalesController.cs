using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToolStore.Data;
using ToolStore.Models;

namespace ToolStore.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ToolId = new SelectList(_context.Tools, "Id", "Name");
            return View(await _context.Sales.Include(s => s.Tool).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Sell(int toolId, int quantity)
        {
            var tool = await _context.Tools.FindAsync(toolId);

            if (tool == null || tool.QuantityInStock < quantity)
            {
                ViewBag.Error = $"Ошибка: недостаточно товара '{tool?.Name}' на складе!";
                ViewBag.ToolId = new SelectList(_context.Tools, "Id", "Name");
                return View("Index", await _context.Sales.Include(s => s.Tool).ToListAsync());
            }

            tool.QuantityInStock -= quantity;

            _context.Sales.Add(new Sale
            {
                ToolId = toolId,
                Quantity = quantity,
                Date = DateTime.Now,
                Status = "Активна"
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}