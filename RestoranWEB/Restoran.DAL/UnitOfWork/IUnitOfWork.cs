using Restoran.DAL.Repositories;

namespace Restoran.DAL.UnitOfWork
{
    //spaja sve repozitorije u jednu celinj
    public interface IUnitOfWork : IDisposable
    {
        IStoRepository Stolovi { get; }
        IGostRepository Gosti { get; }
        IRezervacijaRepository Rezervacije { get; }
        //cuva promene u bazi
        Task<int> SaveChangesAsync();
    }
}
