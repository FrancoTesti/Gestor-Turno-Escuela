using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IReporteService
    {
        Task<IEnumerable<AlumnosPorCursoDTO>> GetAlumnosPorCursoAsync();

        Task<IEnumerable<RetiroDTO>> GetRetirosAsync(DateTime? desde, DateTime? hasta);
    }
}
