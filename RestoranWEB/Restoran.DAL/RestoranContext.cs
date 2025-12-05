using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Entities;

namespace Restoran.DAL
{
    public class RestoranContext : DbContext
    {
        public RestoranContext(DbContextOptions<RestoranContext> opts) : base(opts) { }

        public DbSet<Gost> Gosti => Set<Gost>();
        public DbSet<Sto> Stolovi => Set<Sto>();
        public DbSet<Rezervacija> Rezervacije => Set<Rezervacija>();



        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Gost>().HasKey(g => g.IDGosta);
            b.Entity<Sto>().HasKey(s => s.IDStola);
            b.Entity<Rezervacija>().HasKey(r => r.IDRezervacije);

            b.Entity<Gost>().Property(g => g.IDGosta).UseIdentityColumn();
            b.Entity<Sto>().Property(s => s.IDStola).UseIdentityColumn();
            b.Entity<Rezervacija>().Property(r => r.IDRezervacije).UseIdentityColumn();

            b.Entity<Rezervacija>()
                .HasOne(r => r.Gost)
                .WithMany(g => g.Rezervacije)
                .HasForeignKey(r => r.IDGosta)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Rezervacija>()
                .HasOne(r => r.Sto)
                .WithMany(s => s.Rezervacije)
                .HasForeignKey(r => r.IDStola)
                .OnDelete(DeleteBehavior.Cascade);

            // **NAZIVI TABELA – da odgovaraju SQL-u**
            b.Entity<Gost>().ToTable("Gost");
            b.Entity<Sto>().ToTable("Sto");
            b.Entity<Rezervacija>().ToTable("Rezervacija");   // NE Rezervacije
        }
    }
}