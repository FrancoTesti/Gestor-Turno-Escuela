using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface ITutorService
    {
        Task<IEnumerable<TutorDTO>> GetAllAsync();
        Task<TutorDTO?> GetAsync(int id);

        /// <summary>Da de alta un tutor junto con su usuario. Valida el DNI y el nombre de usuario.</summary>
        Task<TutorDTO> AddAsync(TutorDTO dto);

        Task<bool> UpdateAsync(TutorDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
