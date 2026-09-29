using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IReporteService
    {
        /// <summary>Alumnos agrupados por curso escolar, incluyendo los cursos sin alumnos.</summary>
        Task<IEnumerable<AlumnosPorCursoDTO>> GetAlumnosPorCursoAsync();

        /// <summary>Retiros comprendidos entre dos fechas. Las fechas son opcionales.</summary>
        Task<IEnumerable<RetiroDTO>> GetRetirosAsync(DateTime? desde, DateTime? hasta);
    }
}
