using Restoran.DAL.Repositories;

namespace Restoran.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStoRepository Stolovi { get; }
        IGostRepository Gosti { get; }
        IRezervacijaRepository Rezervacije { get; }
        Task<int> SaveChangesAsync();
    }
}
