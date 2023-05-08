using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Tomates.Data;
using Tomates.Models;

namespace Tomates.Controllers
{
    public class SeriesController : Controller
    {
        private readonly TomatesContext _context;

        public SeriesController(TomatesContext context)
        {
            _context = context;
        }

        // GET: Series
        public async Task<IActionResult> Index()
        {
            return _context.Serie != null ?
                        View(await _context.Serie.ToListAsync()) :
                        Problem("Entity set 'TomatesContext.Serie'  is null.");
        }

        // GET: Series/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Serie == null)
            {
                return NotFound();
            }

            var serie = await _context.Serie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);
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

                //List<List<string>> descripcion = WebScraper.Descripcion(url, false,tr);

                //List<List<string>> comentarios_criticos = WebScraper.ComentariosCriticos(url);
                //List<List<string>> comentarios_verificados = WebScraper.ComentariosUsuarios(url);
                //List<List<string>> plataformas = WebScraper.PlataformasDisponibles(response);
                //List<List<string>> actores = WebScraper.ActoresPrincipales(response);

                Serie serie = new Serie();
                serie.AgregarSerieBd(url);

                //Se busca una Serie con el mismo nombre 
                Serie validarSerie = await _context.Serie.FirstOrDefaultAsync(e => e.Nombre == serie.Nombre);

                //Si no hay ninguna Serie con el mismo nombre, se crea
                if (validarSerie == null)
                {
                    _context.Serie.Add(serie);
                }

                //Se actualizan los datos
                else
                {

                    _context.Serie.FromSqlRaw($"update Serie set ImagenSrc = '{serie.ImagenSrc}' " +
                    $",Sinopsis = '{serie.Sinopsis}' ,TomatometroCalificacion = '{serie.TomatometroCalificacion}' " +
                    $", AudienciaCalificacion = '{serie.AudienciaCalificacion}' " +
                    $",TomatometroContadorCalificacion = '{serie.TomatometroContadorCalificacion}' " +
                    $",AudienciaContadorCalificacion = '{serie.AudienciaContadorCalificacion}' " +
                    $",Genero = '{serie.Genero}' , FechalanzamientoTransmision = '{serie.FechalanzamientoTransmision}' " +
                    $",Network = '{serie.Network}' " +
                    $"where Id = '{validarSerie.Id}' ");


                }
                await _context.SaveChangesAsync();
                serie = await _context.Serie.FirstOrDefaultAsync(e => e.Nombre == serie.Nombre);

                _context.ChangeTracker.Clear();


                //Busco cuantas temporadas tiene
                for (int i = 1; i < 100; i++)
                {
                    string temp_response;
                    //si no encuentra la pagina es porque no tiene ese número de serie
                    try
                    {

                        if (i < 10)
                        {
                            temp_response = WebScraper.call_url(url + $"/s0{i}").Result;
                        }
                        else
                        {
                            temp_response = WebScraper.call_url(url + $"/s{i}").Result;
                        }
                    }
                    catch
                    {
                        break;
                    }

                    List<List<string>> calificacion = WebScraper.Calificacion(temp_response, true);

                    Temporada temporada = new Temporada();
                    temporada.ConstruirTemporada(i, calificacion, serie.Id);
                    Temporada validarTemporada = _context.Temporada.FirstOrDefault(a => a.SerieId == temporada.SerieId && a.Numtemporada == temporada.Numtemporada);

                    //Se busca una temporada  
                    if (validarTemporada == null)
                    {
                        _context.Temporada.Add(temporada);
                        await _context.SaveChangesAsync();

                        


                    }
                    else
                    {
                       _context.Temporada.FromSqlRaw($"update Temporada set TomatometroCalificacion = '{temporada.TomatometroCalificacion}' " +
                        $",AudienciaCalificacion = '{temporada.AudienciaCalificacion}' " +
                        $",TomatometroContadorCalificacion = '{temporada.TomatometroContadorCalificacion}'" +
                        $", AudienciaCalificacion = '{temporada.AudienciaCalificacion}' " +
                        $",TomatometroContadorCalificacion = '{temporada.TomatometroContadorCalificacion}' " +
                        $",AudienciaContadorCalificacion = '{temporada.AudienciaContadorCalificacion}' where Id = '{validarTemporada.Id}' and SerieId= '{validarTemporada.SerieId}' ");
                       
                    }
                    await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();

                    temporada = await _context.Temporada.FirstOrDefaultAsync(e => e.Numtemporada == i && e.SerieId == serie.Id);


                    //Se guardan los actores de la temporada
                    List<List<string>> elenco = WebScraper.ActoresPrincipales(temp_response);

                    //Agrega la conexion del elenco con la temporada

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
                        TemporadaCelebridad validarTemporadaCelebridad = _context.TemporadaCelebridad.FirstOrDefault(a => a.CelebridadId == celebridad.Id && a.TemporadaId == temporada.Id);

                        if (validarTemporadaCelebridad == null)
                        {
                            //Agrega la conexion del elenco con la temporada
                            TemporadaCelebridad temporadaCelebridad = new TemporadaCelebridad();
                            temporadaCelebridad.ConstruirSeriePlataforma(persona[2], temporada.Id, celebridad.Id);

                            _context.TemporadaCelebridad.Add(temporadaCelebridad);
                        }

                        await _context.SaveChangesAsync();
                        _context.ChangeTracker.Clear();

                    }

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

                    //Se agrega la conexion con la Serie
                    plataforma = await _context.Plataforma.FirstOrDefaultAsync(e => e.Nombre == plataforma.Nombre);
                    SeriePlataforma validarSeriePlataforma = _context.SeriePlataforma.FirstOrDefault(a => a.PlataformaId == plataforma.Id && a.SerieId == serie.Id);

                    if (validarSeriePlataforma == null)
                    {
                        SeriePlataforma seriePlataforma = new SeriePlataforma();
                        seriePlataforma.ConstruirSeriePlataforma(plataforma.Id, serie.Id);

                        _context.SeriePlataforma.Add(seriePlataforma);
                    }

                    await _context.SaveChangesAsync();
                    _context.ChangeTracker.Clear();
                }

                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        // GET: Series/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagenSrc,Nombre,Sinopsis,TomatometroCalificacion,AudienciaCalificacion,TomatometroContadorCalificacion,AudienciaContadorCalificacion,Genero,FechalanzamientoTransmision,Network")] Serie serie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serie);
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Serie == null)
            {
                return NotFound();
            }

            var serie = await _context.Serie.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }
            return View(serie);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImagenSrc,Nombre,Sinopsis,TomatometroCalificacion,AudienciaCalificacion,TomatometroContadorCalificacion,AudienciaContadorCalificacion,Genero,FechalanzamientoTransmision,Network")] Serie serie)
        {
            if (id != serie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerieExists(serie.Id))
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
            return View(serie);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Serie == null)
            {
                return NotFound();
            }

            var serie = await _context.Serie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Serie == null)
            {
                return Problem("Entity set 'TomatesContext.Serie'  is null.");
            }
            var serie = await _context.Serie.FindAsync(id);
            if (serie != null)
            {
                _context.Serie.Remove(serie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerieExists(int id)
        {
            return (_context.Serie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
