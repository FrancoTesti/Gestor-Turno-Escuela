using GTE.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTE.Data
{
    public interface IAlumnoRepository
    {
        Task AddAsync(Alumno alumno);
        Task<bool> DeleteAsync(int id);
        Task<Alumno?> GetAsync(int id);
        Task<IEnumerable<Alumno>> GetAllAsync();
        Task<bool> UpdateAsync(Alumno alumno);
        Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria);
    }
}