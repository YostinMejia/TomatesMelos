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
    public class PeliculaPlataformasController : Controller
    {
        private readonly TomatesContext _context;

        public PeliculaPlataformasController(TomatesContext context)
        {
            _context = context;
        }

        // GET: PeliculaPlataformas
        public async Task<IActionResult> Index()
        {
            var tomatesContext = _context.PeliculaPlataforma.Include(p => p.Pelicula).Include(p => p.Plataforma);
            return View(await tomatesContext.ToListAsync());
        }

        // GET: PeliculaPlataformas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PeliculaPlataforma == null)
            {
                return NotFound();
            }

            var peliculaPlataforma = await _context.PeliculaPlataforma
                .Include(p => p.Pelicula)
                .Include(p => p.Plataforma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaPlataforma == null)
            {
                return NotFound();
            }

            return View(peliculaPlataforma);
        }

        // GET: PeliculaPlataformas/Create
        public IActionResult Create()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id");
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id");
            return View();
        }

        // POST: PeliculaPlataformas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PeliculaId,PlataformaId")] PeliculaPlataforma peliculaPlataforma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peliculaPlataforma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id", peliculaPlataforma.PeliculaId);
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id", peliculaPlataforma.PlataformaId);
            return View(peliculaPlataforma);
        }

        // GET: PeliculaPlataformas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PeliculaPlataforma == null)
            {
                return NotFound();
            }

            var peliculaPlataforma = await _context.PeliculaPlataforma.FindAsync(id);
            if (peliculaPlataforma == null)
            {
                return NotFound();
            }
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id", peliculaPlataforma.PeliculaId);
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id", peliculaPlataforma.PlataformaId);
            return View(peliculaPlataforma);
        }

        // POST: PeliculaPlataformas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PeliculaId,PlataformaId")] PeliculaPlataforma peliculaPlataforma)
        {
            if (id != peliculaPlataforma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peliculaPlataforma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaPlataformaExists(peliculaPlataforma.Id))
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
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id", peliculaPlataforma.PeliculaId);
            ViewData["PlataformaId"] = new SelectList(_context.Plataforma, "Id", "Id", peliculaPlataforma.PlataformaId);
            return View(peliculaPlataforma);
        }

        // GET: PeliculaPlataformas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PeliculaPlataforma == null)
            {
                return NotFound();
            }

            var peliculaPlataforma = await _context.PeliculaPlataforma
                .Include(p => p.Pelicula)
                .Include(p => p.Plataforma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaPlataforma == null)
            {
                return NotFound();
            }

            return View(peliculaPlataforma);
        }

        // POST: PeliculaPlataformas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PeliculaPlataforma == null)
            {
                return Problem("Entity set 'TomatesContext.PeliculaPlataforma'  is null.");
            }
            var peliculaPlataforma = await _context.PeliculaPlataforma.FindAsync(id);
            if (peliculaPlataforma != null)
            {
                _context.PeliculaPlataforma.Remove(peliculaPlataforma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaPlataformaExists(int id)
        {
          return (_context.PeliculaPlataforma?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
