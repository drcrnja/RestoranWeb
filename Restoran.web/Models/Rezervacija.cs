using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.web.Models
{
    public class Rezervacija
    {
        [Key]
        public int IDRezervacije { get; set; }

        public DateTime Datum { get; set; }
        public TimeSpan Vreme { get; set; }
        public int BrojOsoba { get; set; }

        public int IDGosta { get; set; }
        [ForeignKey("IDGosta")]
        public Gost Gost { get; set; } = null!;

        public int IDStola { get; set; }
        [ForeignKey("IDStola")]
        public Sto Sto { get; set; } = null!;
    }
}