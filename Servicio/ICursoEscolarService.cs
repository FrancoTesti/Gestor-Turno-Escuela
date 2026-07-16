using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface ICursoEscolarService
    {
        Task<CursoEscolarDTO> AddAsync(CursoEscolarDTO dto);
        Task<CursoEscolarDTO?> GetAsync(int id);
        Task<IEnumerable<CursoEscolarDTO>> GetAllAsync();
        Task<bool> UpdateAsync(CursoEscolarDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}