using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public interface IAutorizacionRepository
    {
        Task<IEnumerable<Autorizacion>> GetAllAsync();
        Task<IEnumerable<Autorizacion>> GetByTutorIdAsync(int tutorId);
        Task<IEnumerable<Alumno>> GetAlumnosByTutorIdAsync(int tutorId);
        Task<bool> EstaAutorizadoAsync(int tutorId, int alumnoId);
        Task AddAsync(Autorizacion autorizacion);
        Task<bool> DeleteAsync(int id);
    }
}
