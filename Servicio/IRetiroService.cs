using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IRetiroService
    {
        Task<IEnumerable<RetiroDTO>> GetAllAsync();
        Task<RetiroDTO?> GetAsync(int id);
        Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> AddAsync(RetiroDTO dto);
        Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> UpdateAsync(RetiroDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AlumnoDTO>> GetAlumnosAutorizadosAsync(int tutorId);
    }
}
