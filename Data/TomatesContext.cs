using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tomates.Models;

namespace Tomates.Data
{
    public class TomatesContext : DbContext
    {
        public TomatesContext (DbContextOptions<TomatesContext> options)
            : base(options)
        {
        }
        public DbSet<Tomates.Models.Pelicula>? Pelicula { get; set; }
        public DbSet<Tomates.Models.Serie>? Serie { get; set; }
        public DbSet<Tomates.Models.Plataforma>? Plataforma { get; set; }
        public DbSet<Tomates.Models.Celebridad>? Celebridad { get; set; }
        public DbSet<Tomates.Models.Temporada>? Temporada { get; set; }
        public DbSet<Tomates.Models.SeriePlataforma>? SeriePlataforma { get; set; }
        public DbSet<Tomates.Models.TemporadaCelebridad>? TemporadaCelebridad { get; set; }
        public DbSet<Tomates.Models.PeliculaCelebridad>? PeliculaCelebridad { get; set; }
        public DbSet<Tomates.Models.PeliculaPlataforma>? PeliculaPlataforma { get; set; }

    }
}
