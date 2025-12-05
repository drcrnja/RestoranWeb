using Restoran.DAL.Entities;

namespace Restoran.DAL.Repositories
{
    public interface IGostRepository
    {
        Task<List<Gost>> GetAllAsync();
        Task<Gost?> GetByIdAsync(int id);
        Task<Gost?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<Gost, bool>> predicate);
        Task AddAsync(Gost gost);
    }
}