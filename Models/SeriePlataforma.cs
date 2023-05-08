namespace Tomates.Models
{
    public class SeriePlataforma
    {
        public int Id { get; set; }
        public int SerieId { get; set; }
        public Serie? Serie { get; set; }

        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }


        public void ConstruirSeriePlataforma(int plataformaId, int serieId)
        {
            PlataformaId = plataformaId;
            SerieId = serieId;

        }
    }
}
