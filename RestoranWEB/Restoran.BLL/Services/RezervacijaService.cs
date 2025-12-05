using AutoMapper;
using Restoran.BLL.DTOs;
using Restoran.DAL.Entities;
using Restoran.DAL.UnitOfWork;

namespace Restoran.BLL.Services
{
    public class RezervacijaService : IRezervacijaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public RezervacijaService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<RezervacijaDto>> GetAllAsync() =>
            _mapper.Map<List<RezervacijaDto>>(await _uow.Rezervacije.GetAllAsync() );

        public async Task<RezervacijaDto?> GetByIdAsync(int id) =>
            _mapper.Map<RezervacijaDto>(await _uow.Rezervacije.GetByIdAsync(id));

        public async Task<int> CreateAsync(RezervacijaDto dto)
        {
            bool zauzet = await _uow.Rezervacije
                .AnyAsync(r => r.IDStola == dto.BrojStola && r.Datum == dto.Datum);

            if (zauzet)
                throw new InvalidOperationException($"Sto {dto.BrojStola} je zauzet.");

            var gost = await _uow.Gosti
     .FirstOrDefaultAsync(g => g.ImeGosta == dto.ImeGosta &&
                               g.PrezimeGosta == dto.PrezimeGosta);

            if (gost == null)
            {
                gost = new Gost
                {
                    ImeGosta = dto.ImeGosta,
                    PrezimeGosta = dto.PrezimeGosta
                };
                await _uow.Gosti.AddAsync(gost);
                await _uow.SaveChangesAsync();   // **MORAŠ SAČUVATI DA BI DOBIO ID**
            
        }

            var sto = await _uow.Stolovi.GetByIdAsync(dto.BrojStola);
            if (sto == null) throw new KeyNotFoundException("Sto ne postoji.");

            var rez = new Rezervacija
            {
                Datum = dto.Datum,
                Vreme = dto.Vreme,
                BrojOsoba = dto.BrojOsoba,
                IDGosta = gost.IDGosta,
                IDStola = sto.IDStola
            };

            await _uow.Rezervacije.AddAsync(rez);
            await _uow.SaveChangesAsync();
            return rez.IDRezervacije;
        }

        public async Task UpdateAsync(RezervacijaDto dto)
        {
            var rez = await _uow.Rezervacije.GetByIdAsync(dto.IDRezervacije);
            if (rez == null) throw new KeyNotFoundException("Rezervacija ne postoji.");

            bool zauzet = await _uow.Rezervacije
                .AnyAsync(r => r.IDStola == dto.BrojStola &&
                               r.Datum == dto.Datum &&
                               r.IDRezervacije != dto.IDRezervacije);

            if (zauzet)
                throw new InvalidOperationException($"Sto {dto.BrojStola} je zauzet.");

            _mapper.Map(dto, rez);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Rezervacije.DeleteAsync(id);
            await _uow.SaveChangesAsync();
        }
    }
}