
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolStore.Models;
using ToolStore.Data;

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
        return View(await _context.Tools.ToListAsync());
    }

    // GET: TOOLS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tool = await _context.Tools
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tool == null)
        {
            return NotFound();
        }

        return View(tool);
    }

    // GET: TOOLS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TOOLS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,QuantityInStock,ManufacturerId,Manufacturer,ApplicationAreaId,ApplicationArea")] Tool tool)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tool);
    }

    // GET: TOOLS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tool = await _context.Tools.FindAsync(id);
        if (tool == null)
        {
            return NotFound();
        }
        return View(tool);
    }

    // POST: TOOLS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Price,QuantityInStock,ManufacturerId,Manufacturer,ApplicationAreaId,ApplicationArea")] Tool tool)
    {
        if (id != tool.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tool);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToolExists(tool.Id))
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
        return View(tool);
    }

    // GET: TOOLS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tool = await _context.Tools
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tool == null)
        {
            return NotFound();
        }

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
