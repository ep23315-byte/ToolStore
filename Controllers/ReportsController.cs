using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolStore.Attributes;
using ToolStore.Data;

namespace ToolStore.Controllers
{
    [RoleAuthorize("Менеджер")]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue) startDate = DateTime.Now.AddDays(-30);
            if (!endDate.HasValue) endDate = DateTime.Now;

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Tool)
                .Where(o => o.Date >= startDate && o.Date <= endDate.Value.AddDays(1))
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            ViewBag.TotalSum = orders.SelectMany(o => o.OrderItems)
                                     .Sum(oi => oi.Tool.Price * oi.Quantity);

            return View(orders);
        }
    }
}