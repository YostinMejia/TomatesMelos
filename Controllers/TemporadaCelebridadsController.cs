using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tomates.Data;
using Tomates.Models;

namespace Tomates.Controllers
{
    public class TemporadaCelebridadsController : Controller
    {
        private readonly TomatesContext _context;

        public TemporadaCelebridadsController(TomatesContext context)
        {
            _context = context;
        }

        // GET: TemporadaCelebridads
        public async Task<IActionResult> Index()
        {
            var tomatesContext = _context.TemporadaCelebridad.Include(t => t.Celebridad).Include(t => t.Temporada);
            return View(await tomatesContext.ToListAsync());
        }

        // GET: TemporadaCelebridads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TemporadaCelebridad == null)
            {
                return NotFound();
            }

            var temporadaCelebridad = await _context.TemporadaCelebridad
                .Include(t => t.Celebridad)
                .Include(t => t.Temporada)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (temporadaCelebridad == null)
            {
                return NotFound();
            }

            return View(temporadaCelebridad);
        }

        // GET: TemporadaCelebridads/Create
        public IActionResult Create()
        {
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id");
            ViewData["TemporadaId"] = new SelectList(_context.Temporada, "Id", "Id");
            return View();
        }

        // POST: TemporadaCelebridads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TemporadaId,CelebridadId,Rol")] TemporadaCelebridad temporadaCelebridad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(temporadaCelebridad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id", temporadaCelebridad.CelebridadId);
            ViewData["TemporadaId"] = new SelectList(_context.Temporada, "Id", "Id", temporadaCelebridad.TemporadaId);
            return View(temporadaCelebridad);
        }

        // GET: TemporadaCelebridads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TemporadaCelebridad == null)
            {
                return NotFound();
            }

            var temporadaCelebridad = await _context.TemporadaCelebridad.FindAsync(id);
            if (temporadaCelebridad == null)
            {
                return NotFound();
            }
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id", temporadaCelebridad.CelebridadId);
            ViewData["TemporadaId"] = new SelectList(_context.Temporada, "Id", "Id", temporadaCelebridad.TemporadaId);
            return View(temporadaCelebridad);
        }

        // POST: TemporadaCelebridads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TemporadaId,CelebridadId,Rol")] TemporadaCelebridad temporadaCelebridad)
        {
            if (id != temporadaCelebridad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(temporadaCelebridad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemporadaCelebridadExists(temporadaCelebridad.Id))
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
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id", temporadaCelebridad.CelebridadId);
            ViewData["TemporadaId"] = new SelectList(_context.Temporada, "Id", "Id", temporadaCelebridad.TemporadaId);
            return View(temporadaCelebridad);
        }

        // GET: TemporadaCelebridads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TemporadaCelebridad == null)
            {
                return NotFound();
            }

            var temporadaCelebridad = await _context.TemporadaCelebridad
                .Include(t => t.Celebridad)
                .Include(t => t.Temporada)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (temporadaCelebridad == null)
            {
                return NotFound();
            }

            return View(temporadaCelebridad);
        }

        // POST: TemporadaCelebridads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TemporadaCelebridad == null)
            {
                return Problem("Entity set 'TomatesContext.TemporadaCelebridad'  is null.");
            }
            var temporadaCelebridad = await _context.TemporadaCelebridad.FindAsync(id);
            if (temporadaCelebridad != null)
            {
                _context.TemporadaCelebridad.Remove(temporadaCelebridad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemporadaCelebridadExists(int id)
        {
          return (_context.TemporadaCelebridad?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
