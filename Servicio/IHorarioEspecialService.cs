using GTE.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicio
{
    public interface IHorarioEspecialService
    {
        Task<List<HorarioEspecialDTO>> ObtenerTodosAsync();
        Task<HorarioEspecialDTO?> ObtenerPorIdAsync(int id);
        Task<HorarioEspecialDTO> AgregarAsync(HorarioEspecialDTO dto);
        Task ActualizarAsync(HorarioEspecialDTO dto);
        Task EliminarAsync(int id);
    }
}
