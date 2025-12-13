using Restoran.DAL.Repositories;

namespace Restoran.DAL.UnitOfWork
{
    //spaja sve u jednu celinu
    public class UnitOfWork : IUnitOfWork
    {
        //konekcija ka bazi
        private readonly RestoranContext _ctx;
        public IStoRepository Stolovi { get; }
        public IGostRepository Gosti { get; }
        public IRezervacijaRepository Rezervacije { get; }
        //pravi sve repozitorija
        public UnitOfWork(RestoranContext ctx)
        {
            _ctx = ctx;
            Stolovi = new StoRepository(_ctx);
            Gosti = new GostRepository(_ctx);
            Rezervacije = new RezervacijaRepository(_ctx);
        }
        //cuva promene u bazi

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
        //oslobadja konekciju kada se vise nekoristi
        public void Dispose() => _ctx?.Dispose();
    }
}
