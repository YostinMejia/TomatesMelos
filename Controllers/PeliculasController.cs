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
    public class PeliculasController : Controller
    {
        private readonly TomatesContext _context;

        public PeliculasController(TomatesContext context)
        {
            _context = context;
        }

        // GET: Peliculas
        public async Task<IActionResult> Index()
        {
            return _context.Pelicula != null ?
                        View(await _context.Pelicula.ToListAsync()) :
                        Problem("Entity set 'TomatesContext.Pelicula'  is null.");
        }

        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pelicula == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }



        public ActionResult Scraper()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Scraper(IFormCollection formCollection)
        {

            if (ModelState.IsValid)
            {
                string url = formCollection["url"];
                string response = WebScraper.call_url(url).Result;

                Pelicula pelicula = new Pelicula();
                pelicula.CrearPelicula(url);

                //Se busca una Pelicula con el mismo nombre 
                Pelicula validarPelicula = await _context.Pelicula.FirstOrDefaultAsync(e => e.Nombre == pelicula.Nombre);

                //Si no hay ninguna Pelicula con el mismo nombre, se crea
                if (validarPelicula == null)
                {
                    _context.Pelicula.Add(pelicula);
                }

                //Se actualizan los datos
                else
                {

                    _context.Pelicula.FromSqlRaw($"update Pelicula set ImagenSrc = '{pelicula.ImagenSrc}' " +
                    $",Sinopsis = '{pelicula.Sinopsis}' ,TomatometroCalificacion = '{pelicula.TomatometroCalificacion}' " +
                    $", AudienciaCalificacion = '{pelicula.AudienciaCalificacion}' " +
                    $",TomatometroContadorCalificacion = '{pelicula.TomatometroContadorCalificacion}' " +
                    $",AudienciaContadorCalificacion = '{pelicula.AudienciaContadorCalificacion}' " +
                    $",Genero = '{pelicula.Genero}' , Clasificacion = '{pelicula.Clasificacion}' " +
                    $",LenguajeOriginal = '{pelicula.LenguajeOriginal}',FechaLanzamientoCine = '{pelicula.FechaLanzamientoCine}' " +
                    $",FechaLanzamientoTransmision = '{pelicula.FechaLanzamientoTransmision}',Duracion = '{pelicula.Duracion}'" +
                    $",Distribuidor = '{pelicula.Distribuidor}',CoProduccion = '{pelicula.CoProduccion}'" +
                    $"where Id = '{validarPelicula.Id}' ");


                }
                await _context.SaveChangesAsync();
                pelicula = await _context.Pelicula.FirstOrDefaultAsync(e => e.Nombre == pelicula.Nombre);

                _context.ChangeTracker.Clear();


                //Se guardan los actores de la pelicula
                List<List<string>> elenco = WebScraper.ActoresPrincipales(response);

                //Agrega la conexion del elenco con la pelicula

                foreach (List<string> persona in elenco)
                {

                    Celebridad celebridad = new Celebridad();
                    celebridad.ConstruirCelebridad(persona);

                    Celebridad validarCelebridad = _context.Celebridad.FirstOrDefault(a => a.Nombre == celebridad.Nombre);
                    //Se busca un actor con el mismo nombre 
                    if (validarCelebridad == null)
                    {
                        _context.Celebridad.Add(celebridad);

                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        _context.Celebridad.FromSqlRaw($"update Celebridad set ImagenSrc = '{celebridad.ImagenSrc}' where Id = '{validarCelebridad.Id}' ");
                    }

                    await _context.SaveChangesAsync();

                    celebridad = await _context.Celebridad.FirstOrDefaultAsync(e => e.Nombre == celebridad.Nombre);
                    PeliculaCelebridad validarPeliculaCelebridad = _context.PeliculaCelebridad.FirstOrDefault(a => a.CelebridadId == celebridad.Id && a.PeliculaId == pelicula.Id);

                    if (validarPeliculaCelebridad == null)
                    {
                        //Agrega la conexion del elenco con la pelicula
                        PeliculaCelebridad peliculaCelebridad = new PeliculaCelebridad();
                        peliculaCelebridad.ConstruirPeliculaPlataforma(persona[2], pelicula.Id, celebridad.Id);

                        _context.PeliculaCelebridad.Add(peliculaCelebridad);
                    }

                    await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();

                }

                List<List<string>> plataformas = WebScraper.PlataformasDisponibles(response);

                foreach (List<string> disponible in plataformas)
                {

                    Plataforma plataforma = new Plataforma();
                    plataforma.CrearPlataforma(disponible);

                    Plataforma validarPlataforma = _context.Plataforma.FirstOrDefault(a => a.Nombre == plataforma.Nombre);

                    //Se busca un actor con el mismo nombre 
                    if (validarPlataforma == null)
                    {
                        _context.Plataforma.Add(plataforma);

                    }
                    else
                    {
                        _context.Plataforma.FromSqlRaw($"update Plataforma set Link = '{plataforma.Link}' where Id = '{validarPlataforma.Id}' ");
                    }


                    await _context.SaveChangesAsync();

                    //Se agrega la conexion con la Pelicula
                    plataforma = await _context.Plataforma.FirstOrDefaultAsync(e => e.Nombre == plataforma.Nombre);
                    PeliculaPlataforma validarPeliculaPlataforma = _context.PeliculaPlataforma.FirstOrDefault(a => a.PlataformaId == plataforma.Id && a.PeliculaId == pelicula.Id);

                    if (validarPeliculaPlataforma == null)
                    {
                        PeliculaPlataforma peliculaPlataforma = new PeliculaPlataforma();
                        peliculaPlataforma.ConstruirPeliculaPlataforma(plataforma.Id, pelicula.Id);

                        _context.PeliculaPlataforma.Add(peliculaPlataforma);
                    }

                    await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();
                }

                return RedirectToAction(nameof(Index));

            }
            return View();
        }
    

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagenSrc,Nombre,Sinopsis,TomatometroCalificacion,AudienciaCalificacion,TomatometroContadorCalificacion,AudienciaContadorCalificacion,Clasificacion,Genero,LenguajeOriginal,FechaLanzamientoCine,FechaLanzamientoTransmision,Duracion,Distribuidor,CoProduccion")] Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pelicula == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImagenSrc,Nombre,Sinopsis,TomatometroCalificacion,AudienciaCalificacion,TomatometroContadorCalificacion,AudienciaContadorCalificacion,Clasificacion,Genero,LenguajeOriginal,FechaLanzamientoCine,FechaLanzamientoTransmision,Duracion,Distribuidor,CoProduccion")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
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
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pelicula == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pelicula == null)
            {
                return Problem("Entity set 'TomatesContext.Pelicula'  is null.");
            }
            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula != null)
            {
                _context.Pelicula.Remove(pelicula);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
          return (_context.Pelicula?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
