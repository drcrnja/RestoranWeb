using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Entities;

namespace Restoran.DAL.Repositories
{
    public class StoRepository : IStoRepository
    {
        private readonly RestoranContext _ctx;
        public StoRepository(RestoranContext ctx) => _ctx = ctx;

        public Task<List<Sto>> GetAllAsync() => _ctx.Stolovi.ToListAsync();
        public Task<Sto?> GetByIdAsync(int id) => _ctx.Stolovi.FindAsync(id).AsTask();
        public Task<bool> ExistsAsync(int id) => _ctx.Stolovi.AnyAsync(s => s.IDStola == id);
    }
}
