using Restoran.DAL.Entities;

namespace Restoran.DAL.Repositories
{
    public interface IRezervacijaRepository
    {
        Task<List<Rezervacija>> GetAllAsync();
        Task<Rezervacija?> GetByIdAsync(int id);
        Task AddAsync(Rezervacija rez);
        Task DeleteAsync(int id);
        Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<Rezervacija, bool>> predicate);
    }
}
