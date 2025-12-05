namespace Restoran.BLL.DTOs
{
    public class RezervacijaDto
    {
        public int IDRezervacije { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Vreme { get; set; }
        public int BrojOsoba { get; set; }

        public string ImeGosta { get; set; } = string.Empty;
        public string PrezimeGosta { get; set; } = string.Empty;
        public int BrojStola { get; set; }
    }
}
