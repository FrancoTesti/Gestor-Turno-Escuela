using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface ITutorService
    {
        Task<IEnumerable<TutorDTO>> GetAllAsync();
        Task<TutorDTO?> GetAsync(int id);

        Task<TutorDTO> AddAsync(TutorDTO dto);

        Task<bool> UpdateAsync(TutorDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
