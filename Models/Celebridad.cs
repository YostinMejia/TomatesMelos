using Microsoft.EntityFrameworkCore;
using Tomates.Data;

namespace Tomates.Models
{
    public class Celebridad
    {
        public int Id { get; set; }
        public string ImagenSrc { get; set; }
        public string Nombre { get; set; }


        public void ConstruirCelebridad(List<string> persona)
        {
            ImagenSrc = persona[0];
            Nombre = persona[1];
        }
    }
}
