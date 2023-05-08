using System;

namespace Tomates.Models
{
    public class TemporadaCelebridad
    {
        public int Id { get; set; }
        public int TemporadaId { get; set; }
        public Temporada? Temporada { get; set; }

        public int CelebridadId { get; set; }
        public Celebridad? Celebridad { get; set; }

        public string Rol { get;set; }

        public void ConstruirSeriePlataforma(string rol, int temporadaId, int celebridadId)
        {
            TemporadaId= temporadaId;
            CelebridadId= celebridadId;
            Rol = rol;


        }
    }
}
