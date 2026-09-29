using GTE.Dominio;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTE.Data
{
    public class HorarioEspecialRepository : IHorarioEspecialRepository
    {
        private readonly GTEContext _context;

        public HorarioEspecialRepository(GTEContext context)
        {
            _context = context;
        }

        public async Task<List<HorarioEspecial>> ObtenerTodosAsync()
        {
            return await _context.HorariosEspeciales.ToListAsync();
        }

        public async Task<HorarioEspecial?> ObtenerPorIdAsync(int id)
        {
            return await _context.HorariosEspeciales.FindAsync(id);
        }

        public async Task AgregarAsync(HorarioEspecial horario)
        {
            await _context.HorariosEspeciales.AddAsync(horario);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(HorarioEspecial horario)
        {
            _context.HorariosEspeciales.Update(horario);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var horario = await _context.HorariosEspeciales.FindAsync(id);
            if (horario != null)
            {
                _context.HorariosEspeciales.Remove(horario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
