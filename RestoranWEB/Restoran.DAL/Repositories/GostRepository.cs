using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Entities;

namespace Restoran.DAL.Repositories
{
    public class GostRepository : IGostRepository
    {
        private readonly RestoranContext _ctx;
        public GostRepository(RestoranContext ctx) => _ctx = ctx;

        public Task<List<Gost>> GetAllAsync() => _ctx.Gosti.ToListAsync();
        public Task<Gost?> GetByIdAsync(int id) => _ctx.Gosti.FindAsync(id).AsTask();
        public Task<Gost?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<Gost, bool>> predicate) => _ctx.Gosti.FirstOrDefaultAsync(predicate);
        public Task AddAsync(Gost gost) => _ctx.Gosti.AddAsync(gost).AsTask();
    }
}