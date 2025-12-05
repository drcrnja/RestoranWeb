using Restoran.BLL.DTOs;

namespace Restoran.BLL.Services
{
    public interface IRezervacijaService
    {
        Task<List<RezervacijaDto>> GetAllAsync();
        Task<RezervacijaDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(RezervacijaDto dto);
        Task UpdateAsync(RezervacijaDto dto);
        Task DeleteAsync(int id);
    }
}
