namespace Tomates.Models
{
    public class PeliculaPlataforma
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }

        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }

        public void ConstruirPeliculaPlataforma(int plataformaId, int peliculaId)
        {
            PlataformaId = plataformaId;
            PeliculaId = peliculaId;

        }
    }
}
