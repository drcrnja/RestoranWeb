using Restoran.DAL.Entities;

namespace Restoran.DAL.Repositories
{
    public interface IStoRepository
    {
        Task<List<Sto>> GetAllAsync();
        Task<Sto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
