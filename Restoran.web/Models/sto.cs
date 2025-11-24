using System.ComponentModel.DataAnnotations;

namespace Restoran.web.Models
{
    public class Sto
    {
        [Key]
        public int IDStola { get; set; }

        public int BrojStola { get; set; }
        public int BrojMesta { get; set; } = 4;
        public string? Lokacija { get; set; } = "Sala";

        public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
    }
}