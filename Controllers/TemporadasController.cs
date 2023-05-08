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
    public class TemporadasController : Controller
    {
        private readonly TomatesContext _context;

        public TemporadasController(TomatesContext context)
        {
            _context = context;
        }

        // GET: Temporadas
        public async Task<IActionResult> Index()
        {
            var tomatesContext = _context.Temporada.Include(t => t.Serie);
            return View(await tomatesContext.ToListAsync());
        }

        // GET: Temporadas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Temporada == null)
            {
                return NotFound();
            }

            var temporada = await _context.Temporada
                .Include(t => t.Serie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (temporada == null)
            {
                return NotFound();
            }

            return View(temporada);
        }

        // GET: Temporadas/Create
        public IActionResult Create()
        {
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id");
            return View();
        }

        // POST: Temporadas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SerieId,Numtemporada,TomatometroCalificacion,AudienciaCalificacion,TomatometroContadorCalificacion,AudienciaContadorCalificacion")] Temporada temporada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(temporada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", temporada.SerieId);
            return View(temporada);
        }

        // GET: Temporadas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Temporada == null)
            {
                return NotFound();
            }

            var temporada = await _context.Temporada.FindAsync(id);
            if (temporada == null)
            {
                return NotFound();
            }
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", temporada.SerieId);
            return View(temporada);
        }

        // POST: Temporadas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SerieId,Numtemporada,TomatometroCalificacion,AudienciaCalificacion,TomatometroContadorCalificacion,AudienciaContadorCalificacion")] Temporada temporada)
        {
            if (id != temporada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(temporada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemporadaExists(temporada.Id))
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
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", temporada.SerieId);
            return View(temporada);
        }

        // GET: Temporadas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Temporada == null)
            {
                return NotFound();
            }

            var temporada = await _context.Temporada
                .Include(t => t.Serie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (temporada == null)
            {
                return NotFound();
            }

            return View(temporada);
        }

        // POST: Temporadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Temporada == null)
            {
                return Problem("Entity set 'TomatesContext.Temporada'  is null.");
            }
            var temporada = await _context.Temporada.FindAsync(id);
            if (temporada != null)
            {
                _context.Temporada.Remove(temporada);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemporadaExists(int id)
        {
          return (_context.Temporada?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
