using GTE.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTE.Data
{
    public interface IHorarioEspecialRepository
    {
        Task<List<HorarioEspecial>> ObtenerTodosAsync();
        Task<HorarioEspecial?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(HorarioEspecial horario);
        Task ActualizarAsync(HorarioEspecial horario);
        Task EliminarAsync(int id);
    }
}
