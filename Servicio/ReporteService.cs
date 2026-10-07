using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly ICursoEscolarRepository _cursoRepository;
        private readonly IRetiroService _retiroService;

        public ReporteService(
            IAlumnoRepository alumnoRepository,
            ICursoEscolarRepository cursoRepository,
            IRetiroService retiroService)
        {
            _alumnoRepository = alumnoRepository;
            _cursoRepository = cursoRepository;
            _retiroService = retiroService;
        }

        public async Task<IEnumerable<AlumnosPorCursoDTO>> GetAlumnosPorCursoAsync()
        {
            var cursos = await _cursoRepository.GetAllAsync();
            var alumnos = await _alumnoRepository.GetAllAsync();

            var cantidades = alumnos
                .GroupBy(alumno => alumno.IdCurso)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());

            return cursos
                .Select(curso => new AlumnosPorCursoDTO
                {
                    IdCurso = curso.IdCurso,
                    Grado = curso.Grado,
                    Curso = curso.Curso,
                    Turno = curso.Turno,
                    Cantidad = cantidades.TryGetValue(curso.IdCurso, out int cantidad) ? cantidad : 0
                })
                .OrderBy(fila => OrdenDelTurno(fila.Turno))
                .ThenBy(fila => fila.Grado)
                .ThenBy(fila => fila.Curso)
                .ToList();
        }

        public async Task<IEnumerable<RetiroDTO>> GetRetirosAsync(DateTime? desde, DateTime? hasta)
        {
            var retiros = await _retiroService.GetAllAsync();

            return retiros
                .Where(retiro => !desde.HasValue || retiro.FechaHora.Date >= desde.Value.Date)
                .Where(retiro => !hasta.HasValue || retiro.FechaHora.Date <= hasta.Value.Date)
                .ToList();
        }

        private static int OrdenDelTurno(string? turno) => turno switch
        {
            "Mañana" => 0,
            "Tarde" => 1,
            "Noche" => 2,
            _ => 3
        };
    }
}
