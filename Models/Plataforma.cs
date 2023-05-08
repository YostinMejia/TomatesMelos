namespace Tomates.Models
{
    public class Plataforma
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Nombre { get; set; }

        public void CrearPlataforma(List<string> disponible)
        {
            Link = disponible[0];
            Nombre = disponible[1].ToLower();
        }
    }
}
