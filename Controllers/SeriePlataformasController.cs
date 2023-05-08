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
    public class SeriePlataformasController : Controller
    {
        private readonly TomatesContext _context;

        public SeriePlataformasController(TomatesContext context)
        {
            _context = context;
        }

        // GET: SeriePlataformas
        public async Task<IActionResult> Index()
        {
            var tomatesContext = _context.SeriePlataforma.Include(s => s.Plataforma).Include(s => s.Serie);
            return View(await tomatesContext.ToListAsync());
        }

        // GET: SeriePlataformas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SeriePlataforma == null)
            {
                return NotFound();
            }

            var seriePlataforma = await _context.SeriePlataforma
                .Include(s => s.Plataforma)
                .Include(s => s.Serie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seriePlataforma == null)
            {
                return NotFound();
            }

            return View(seriePlataforma);
        }

        // GET: SeriePlataformas/Create
        public IActionResult Create()
        {
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id");
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id");
            return View();
        }

        // POST: SeriePlataformas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SerieId,PlataformaId")] SeriePlataforma seriePlataforma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seriePlataforma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id", seriePlataforma.PlataformaId);
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", seriePlataforma.SerieId);
            return View(seriePlataforma);
        }

        // GET: SeriePlataformas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SeriePlataforma == null)
            {
                return NotFound();
            }

            var seriePlataforma = await _context.SeriePlataforma.FindAsync(id);
            if (seriePlataforma == null)
            {
                return NotFound();
            }
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id", seriePlataforma.PlataformaId);
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", seriePlataforma.SerieId);
            return View(seriePlataforma);
        }

        // POST: SeriePlataformas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SerieId,PlataformaId")] SeriePlataforma seriePlataforma)
        {
            if (id != seriePlataforma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seriePlataforma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriePlataformaExists(seriePlataforma.Id))
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
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id", seriePlataforma.PlataformaId);
            ViewData["SerieId"] = new SelectList(_context.Serie, "Id", "Id", seriePlataforma.SerieId);
            return View(seriePlataforma);
        }

        // GET: SeriePlataformas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SeriePlataforma == null)
            {
                return NotFound();
            }

            var seriePlataforma = await _context.SeriePlataforma
                .Include(s => s.Plataforma)
                .Include(s => s.Serie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seriePlataforma == null)
            {
                return NotFound();
            }

            return View(seriePlataforma);
        }

        // POST: SeriePlataformas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SeriePlataforma == null)
            {
                return Problem("Entity set 'TomatesContext.SeriePlataforma'  is null.");
            }
            var seriePlataforma = await _context.SeriePlataforma.FindAsync(id);
            if (seriePlataforma != null)
            {
                _context.SeriePlataforma.Remove(seriePlataforma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriePlataformaExists(int id)
        {
          return (_context.SeriePlataforma?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
