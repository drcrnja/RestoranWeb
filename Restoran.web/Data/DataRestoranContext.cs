using Microsoft.EntityFrameworkCore;
using Restoran.web.Models;

namespace Restoran.web.Data
{
    public class RestoranContext : DbContext
    {
        public RestoranContext(DbContextOptions<RestoranContext> options)
            : base(options) { }

        public DbSet<Gost> Gosti { get; set; }
        public DbSet<Sto> Stolovi { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // preslikaj klase na tačne nazive tabela koje već postoje
            modelBuilder.Entity<Gost>().ToTable("Gost");
            modelBuilder.Entity<Sto>().ToTable("Sto");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
        }
    }
}