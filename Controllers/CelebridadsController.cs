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
    public class CelebridadsController : Controller
    {
        private readonly TomatesContext _context;

        public CelebridadsController(TomatesContext context)
        {
            _context = context;
        }

        // GET: Celebridads
        public async Task<IActionResult> Index()
        {
              return _context.Celebridad != null ? 
                          View(await _context.Celebridad.ToListAsync()) :
                          Problem("Entity set 'TomatesContext.Celebridad'  is null.");
        }

        // GET: Celebridads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Celebridad == null)
            {
                return NotFound();
            }

            var celebridad = await _context.Celebridad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (celebridad == null)
            {
                return NotFound();
            }

            return View(celebridad);
        }

        // GET: Celebridads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Celebridads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagenSrc,Nombre")] Celebridad celebridad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(celebridad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(celebridad);
        }

        // GET: Celebridads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Celebridad == null)
            {
                return NotFound();
            }

            var celebridad = await _context.Celebridad.FindAsync(id);
            if (celebridad == null)
            {
                return NotFound();
            }
            return View(celebridad);
        }

        // POST: Celebridads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImagenSrc,Nombre")] Celebridad celebridad)
        {
            if (id != celebridad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(celebridad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CelebridadExists(celebridad.Id))
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
            return View(celebridad);
        }

        // GET: Celebridads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Celebridad == null)
            {
                return NotFound();
            }

            var celebridad = await _context.Celebridad
                .FirstOrDefaultAsync(m => m.Id == id);
            if (celebridad == null)
            {
                return NotFound();
            }

            return View(celebridad);
        }

        // POST: Celebridads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Celebridad == null)
            {
                return Problem("Entity set 'TomatesContext.Celebridad'  is null.");
            }
            var celebridad = await _context.Celebridad.FindAsync(id);
            if (celebridad != null)
            {
                _context.Celebridad.Remove(celebridad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CelebridadExists(int id)
        {
          return (_context.Celebridad?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
