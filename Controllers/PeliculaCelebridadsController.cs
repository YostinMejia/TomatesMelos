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
    public class PeliculaCelebridadsController : Controller
    {
        private readonly TomatesContext _context;

        public PeliculaCelebridadsController(TomatesContext context)
        {
            _context = context;
        }

        // GET: PeliculaCelebridads
        public async Task<IActionResult> Index()
        {
            var tomatesContext = _context.PeliculaCelebridad.Include(p => p.Celebridad).Include(p => p.Pelicula);
            return View(await tomatesContext.ToListAsync());
        }

        // GET: PeliculaCelebridads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PeliculaCelebridad == null)
            {
                return NotFound();
            }

            var peliculaCelebridad = await _context.PeliculaCelebridad
                .Include(p => p.Celebridad)
                .Include(p => p.Pelicula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaCelebridad == null)
            {
                return NotFound();
            }

            return View(peliculaCelebridad);
        }

        // GET: PeliculaCelebridads/Create
        public IActionResult Create()
        {
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id");
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id");
            return View();
        }

        // POST: PeliculaCelebridads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PeliculaId,CelebridadId,Rol")] PeliculaCelebridad peliculaCelebridad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peliculaCelebridad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id", peliculaCelebridad.CelebridadId);
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id", peliculaCelebridad.PeliculaId);
            return View(peliculaCelebridad);
        }

        // GET: PeliculaCelebridads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PeliculaCelebridad == null)
            {
                return NotFound();
            }

            var peliculaCelebridad = await _context.PeliculaCelebridad.FindAsync(id);
            if (peliculaCelebridad == null)
            {
                return NotFound();
            }
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id", peliculaCelebridad.CelebridadId);
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id", peliculaCelebridad.PeliculaId);
            return View(peliculaCelebridad);
        }

        // POST: PeliculaCelebridads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PeliculaId,CelebridadId,Rol")] PeliculaCelebridad peliculaCelebridad)
        {
            if (id != peliculaCelebridad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peliculaCelebridad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaCelebridadExists(peliculaCelebridad.Id))
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
            ViewData["CelebridadId"] = new SelectList(_context.Celebridad, "Id", "Id", peliculaCelebridad.CelebridadId);
            ViewData["PeliculaId"] = new SelectList(_context.Pelicula, "Id", "Id", peliculaCelebridad.PeliculaId);
            return View(peliculaCelebridad);
        }

        // GET: PeliculaCelebridads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PeliculaCelebridad == null)
            {
                return NotFound();
            }

            var peliculaCelebridad = await _context.PeliculaCelebridad
                .Include(p => p.Celebridad)
                .Include(p => p.Pelicula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaCelebridad == null)
            {
                return NotFound();
            }

            return View(peliculaCelebridad);
        }

        // POST: PeliculaCelebridads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PeliculaCelebridad == null)
            {
                return Problem("Entity set 'TomatesContext.PeliculaCelebridad'  is null.");
            }
            var peliculaCelebridad = await _context.PeliculaCelebridad.FindAsync(id);
            if (peliculaCelebridad != null)
            {
                _context.PeliculaCelebridad.Remove(peliculaCelebridad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaCelebridadExists(int id)
        {
          return (_context.PeliculaCelebridad?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
