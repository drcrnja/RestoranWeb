using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Entities;

namespace Restoran.DAL.Repositories
{
    //implementacija za rezervaciju
    public class RezervacijaRepository : IRezervacijaRepository
    {
        private readonly RestoranContext _ctx;
        public RezervacijaRepository(RestoranContext ctx) => _ctx = ctx;
        //uzmi sve rezervacije za povezanim sebicnim gostom i stolom
        public Task<List<Rezervacija>> GetAllAsync() => _ctx.Rezervacije
            .Include(r => r.Gost)
            .Include(r => r.Sto)
            .ToListAsync();
        //uzmi rezervaciju po idu sa povezanim sebicnim gostom i stolom
        public Task<Rezervacija?> GetByIdAsync(int id) => _ctx.Rezervacije
            .Include(r => r.Gost)
            .Include(r => r.Sto)
            .FirstOrDefaultAsync(r => r.IDRezervacije == id);
        //dodaj novu rezervaciju
        public Task AddAsync(Rezervacija rez) => _ctx.Rezervacije.AddAsync(rez).AsTask();
        //obrisi rezervaciju po idu
        public Task DeleteAsync(int id) => _ctx.Rezervacije.Where(r => r.IDRezervacije == id).ExecuteDeleteAsync();
        //proveri dali postoji rezervacija po uslovu
        public Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<Rezervacija, bool>> predicate) => _ctx.Rezervacije.AnyAsync(predicate);
    }
}