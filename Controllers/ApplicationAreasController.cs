using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolStore.Attributes;
using ToolStore.Models;
using ToolStore.Data;

namespace ToolStore.Controllers
{
    [RoleAuthorize("Менеджер")]
    public class ApplicationAreasController : Controller
    {
        private readonly AppDbContext _context;

        public ApplicationAreasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: APPLICATIONAREAS
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationAreas.ToListAsync());
        }

        // GET: APPLICATIONAREAS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationarea = await _context.ApplicationAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationarea == null)
            {
                return NotFound();
            }

            return View(applicationarea);
        }

        // GET: APPLICATIONAREAS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: APPLICATIONAREAS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] ApplicationArea applicationarea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationarea);
        }

        // GET: APPLICATIONAREAS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationarea = await _context.ApplicationAreas.FindAsync(id);
            if (applicationarea == null)
            {
                return NotFound();
            }
            return View(applicationarea);
        }

        // POST: APPLICATIONAREAS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description")] ApplicationArea applicationarea)
        {
            if (id != applicationarea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationAreaExists(applicationarea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationarea);
        }

        // GET: APPLICATIONAREAS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationarea = await _context.ApplicationAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationarea == null)
            {
                return NotFound();
            }

            return View(applicationarea);
        }

        // POST: APPLICATIONAREAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var applicationarea = await _context.ApplicationAreas.FindAsync(id);
            if (applicationarea != null)
            {
                _context.ApplicationAreas.Remove(applicationarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationAreaExists(int? id)
        {
            return _context.ApplicationAreas.Any(e => e.Id == id);
        }
    }
}