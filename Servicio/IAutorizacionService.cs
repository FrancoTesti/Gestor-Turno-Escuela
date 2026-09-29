using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IAutorizacionService
    {
        Task<IEnumerable<AutorizacionDTO>> GetAllAsync();
        Task<AutorizacionDTO?> GetAsync(int id);
        Task<IEnumerable<AutorizacionDTO>> GetByTutorIdAsync(int tutorId);
        Task<IEnumerable<AlumnoDTO>> GetAlumnosAutorizadosAsync(int tutorId);
        Task<(bool Exito, string Mensaje, AutorizacionDTO? Autorizacion)> AddAsync(AutorizacionDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByTutorAndAlumnoAsync(int tutorId, int alumnoId);
    }
}
