namespace Tomates.Models
{
    public class PeliculaCelebridad
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }

        public int CelebridadId { get; set; }
        public Celebridad? Celebridad { get; set; }

        public string Rol { get; set; }


        public void ConstruirPeliculaPlataforma(string rol, int peliculaId, int celebridadId)
        {
            PeliculaId = peliculaId;
            CelebridadId = celebridadId;
            Rol = rol;


        }
    }

}
