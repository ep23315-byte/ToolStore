using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToolStore.Data;
using ToolStore.Models;

namespace ToolStore.Controllers
{
    public class ToolsController : Controller
    {
        private readonly AppDbContext _context;

        public ToolsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TOOLS
        public async Task<IActionResult> Index()
        {
            var tools = await _context.Tools
                .Include(t => t.Manufacturer)
                .Include(t => t.ApplicationArea)
                .ToListAsync();
            return View(tools);
        }

        // GET: TOOLS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tool = await _context.Tools
                .Include(t => t.Manufacturer)
                .Include(t => t.ApplicationArea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tool == null) return NotFound();

            return View(tool);
        }

        // GET: TOOLS/Create
        public IActionResult Create()
        {
            ViewBag.ManufacturerId = new SelectList(_context.Manufacturers, "Id", "Name");
            ViewBag.ApplicationAreaId = new SelectList(_context.ApplicationAreas, "Id", "Name");
            return View();
        }

        // POST: TOOLS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,QuantityInStock,ManufacturerId,ApplicationAreaId")] Tool tool)
        {
            ModelState.Remove("Manufacturer");
            ModelState.Remove("ApplicationArea");

            if (ModelState.IsValid)
            {
                var newTool = new Tool
                {
                    Name = tool.Name,
                    Price = tool.Price,
                    QuantityInStock = tool.QuantityInStock,
                    ManufacturerId = tool.ManufacturerId,
                    ApplicationAreaId = tool.ApplicationAreaId
                };

                _context.Tools.Add(newTool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ManufacturerId = new SelectList(_context.Manufacturers, "Id", "Name", tool.ManufacturerId);
            ViewBag.ApplicationAreaId = new SelectList(_context.ApplicationAreas, "Id", "Name", tool.ApplicationAreaId);
            return View(tool);
        }

        // GET: TOOLS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tool = await _context.Tools.FindAsync(id);
            if (tool == null) return NotFound();

            ViewBag.ManufacturerId = new SelectList(_context.Manufacturers, "Id", "Name", tool.ManufacturerId);
            ViewBag.ApplicationAreaId = new SelectList(_context.ApplicationAreas, "Id", "Name", tool.ApplicationAreaId);
            return View(tool);
        }

        // POST: TOOLS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,QuantityInStock,ManufacturerId,ApplicationAreaId")] Tool tool)
        {
            if (id != tool.Id) return NotFound();

            ModelState.Remove("Manufacturer");
            ModelState.Remove("ApplicationArea");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTool = await _context.Tools.FindAsync(id);
                    if (existingTool == null) return NotFound();

                    existingTool.Name = tool.Name;
                    existingTool.Price = tool.Price;
                    existingTool.QuantityInStock = tool.QuantityInStock;
                    existingTool.ManufacturerId = tool.ManufacturerId;
                    existingTool.ApplicationAreaId = tool.ApplicationAreaId;

                    _context.Update(existingTool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolExists(tool.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ManufacturerId = new SelectList(_context.Manufacturers, "Id", "Name", tool.ManufacturerId);
            ViewBag.ApplicationAreaId = new SelectList(_context.ApplicationAreas, "Id", "Name", tool.ApplicationAreaId);
            return View(tool);
        }

        // GET: TOOLS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tool = await _context.Tools
                .Include(t => t.Manufacturer)
                .Include(t => t.ApplicationArea)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tool == null) return NotFound();

            return View(tool);
        }

        // POST: TOOLS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool != null)
            {
                _context.Tools.Remove(tool);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolExists(int? id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}