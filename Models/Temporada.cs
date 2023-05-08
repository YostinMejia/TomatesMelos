using Microsoft.EntityFrameworkCore;
using Tomates.Data;


namespace Tomates.Models
{
    public class Temporada
    {
        public int Id { get; set; }
        public int SerieId { get; set; }
        public Serie? Serie { get; set; }
        public int Numtemporada { get; set; }
        public int? TomatometroCalificacion { get; set; }
        public int? AudienciaCalificacion { get; set; }
        public string? TomatometroContadorCalificacion { get; set; }
        public string? AudienciaContadorCalificacion { get; set; }


        public void ConstruirTemporada(int numTemporada, List<List<string>> calificacion, int serieId)
        {

            this.SerieId = serieId;

            this.Numtemporada = numTemporada;
            this.TomatometroCalificacion = Int32.Parse(calificacion[2][1]);
            this.AudienciaCalificacion = Int32.Parse(calificacion[0][1]);
            this.TomatometroContadorCalificacion = calificacion[3][1];
            this.AudienciaContadorCalificacion = calificacion[1][1];

        }

    }
}

