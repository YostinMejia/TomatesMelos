namespace Tomates.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string? ImagenSrc { get; set; }
        public string? Nombre { get; set; }
        public string? Sinopsis { get; set; }
        public int? TomatometroCalificacion { get; set; }
        public int? AudienciaCalificacion { get; set; }
        public string? TomatometroContadorCalificacion { get; set; }
        public string? AudienciaContadorCalificacion { get; set; }

        public string? Clasificacion { get; set; }
        public string? Genero { get; set; }
        public string? LenguajeOriginal { get; set; }
        public string? FechaLanzamientoCine { get; set; }
        public string? FechaLanzamientoTransmision { get; set; }
        public string? Duracion { get; set; }
        public string? Distribuidor { get; set; }
        public string? CoProduccion { get; set; }


        public void CrearPelicula(string url)
        {

            string response = WebScraper.call_url(url).Result;

            List<List<string>> calificacion = WebScraper.Calificacion(response);

            List<List<string>> datos_princi = WebScraper.DatosPrincipales(response);

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
                    case "Rating:":
                        Clasificacion = dato[1];
                        break;
                    case "Original Language:":
                        LenguajeOriginal = dato[1];
                        break;
                    case "Release Date (Theaters):":
                        FechaLanzamientoCine = dato[1];
                        break;
                    case "Release Date (Streaming):":
                        FechaLanzamientoTransmision = dato[1];
                        break;
                    case "Runtime:":
                        Duracion = dato[1];
                        break;
                    case "Distributor:":
                        Distribuidor = dato[1];
                        break;
                    case "Production Co:":
                        CoProduccion = dato[1];
                        break;

                }



            }
        }
    }
}