using Microsoft.EntityFrameworkCore;
using Tomates.Data;

namespace Tomates.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string? ImagenSrc { get; set; }
        public string? Nombre { get; set; }
        public string? Sinopsis { get; set; }
        public int? TomatometroCalificacion { get; set; }
        public int? AudienciaCalificacion { get; set; }
        public string? TomatometroContadorCalificacion { get; set; }
        public string? AudienciaContadorCalificacion { get; set; }
        public string? Genero { get; set; }
        public string? FechalanzamientoTransmision { get; set; }
        public string? Network { get; set; }


        public void AgregarSerieBd(string url)
        {

            string response = WebScraper.call_url(url).Result;

            List<List<string>> calificacion = WebScraper.Calificacion(response, false);

            List<List<string>> datos_princi = WebScraper.DatosPrincipales(response, false);

            this.ImagenSrc = datos_princi[0][1];
            this.Nombre = datos_princi[1][1];
            this.Sinopsis = WebScraper.Descripcion(response)[0][1];
            this.TomatometroCalificacion = Int32.Parse(calificacion[2][1]);
            this.AudienciaCalificacion = Int32.Parse(calificacion[0][1]);
            this.TomatometroContadorCalificacion = calificacion[3][1];
            this.AudienciaContadorCalificacion = calificacion[1][1];

            foreach (List<string> dato in datos_princi)
            {
                switch (dato[0])
                {
                    case "Genre:":
                        Genero = dato[1];
                        break;
                    case "TV Network: ":
                        Network = dato[1];
                        break;
                    case "Premiere Date: ":
                        FechalanzamientoTransmision = dato[1];
                        break;


                }



            }

        }

    }
}
