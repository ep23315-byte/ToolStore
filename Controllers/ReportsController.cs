using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolStore.Data;

namespace ToolStore.Controllers
{
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

            var sales = await _context.Sales
                .Include(s => s.Tool)
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .OrderByDescending(s => s.Date)
                .ToListAsync();

            ViewBag.TotalSum = sales.Sum(s => s.Tool.Price * s.Quantity);
            ViewBag.TotalCount = sales.Sum(s => s.Quantity);

            return View(sales);
        }
    }
}