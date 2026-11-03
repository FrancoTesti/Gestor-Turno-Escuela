using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class CursoEscolarService : ICursoEscolarService
    {
        private readonly ICursoEscolarRepository cursoRepository;

        public CursoEscolarService(ICursoEscolarRepository cursoRepository)
        {
            this.cursoRepository = cursoRepository;
        }

        public async Task<CursoEscolarDTO> AddAsync(CursoEscolarDTO dto)
        {
            var lunes = dto.HorarioSalidaLunes != TimeSpan.Zero ? dto.HorarioSalidaLunes : dto.HorarioSalida;
            var martes = dto.HorarioSalidaMartes != TimeSpan.Zero ? dto.HorarioSalidaMartes : dto.HorarioSalida;
            var miercoles = dto.HorarioSalidaMiercoles != TimeSpan.Zero ? dto.HorarioSalidaMiercoles : dto.HorarioSalida;
            var jueves = dto.HorarioSalidaJueves != TimeSpan.Zero ? dto.HorarioSalidaJueves : dto.HorarioSalida;
            var viernes = dto.HorarioSalidaViernes != TimeSpan.Zero ? dto.HorarioSalidaViernes : dto.HorarioSalida;

            CursoEscolar curso = new CursoEscolar(0, dto.Grado, dto.Curso, dto.Turno, lunes, martes, miercoles, jueves, viernes);
            await cursoRepository.AddAsync(curso);

            dto.IdCurso = curso.IdCurso;
            dto.HorarioSalidaLunes = curso.HorarioLunes;
            dto.HorarioSalidaMartes = curso.HorarioMartes;
            dto.HorarioSalidaMiercoles = curso.HorarioMiercoles;
            dto.HorarioSalidaJueves = curso.HorarioJueves;
            dto.HorarioSalidaViernes = curso.HorarioViernes;
            dto.HorarioSalida = curso.ObtenerHorarioSalida(DateTime.Today.DayOfWeek);
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await cursoRepository.DeleteAsync(id);
        }

        public async Task<CursoEscolarDTO?> GetAsync(int id)
        {
            CursoEscolar? curso = await cursoRepository.GetAsync(id);

            if (curso == null)
                return null;

            return new CursoEscolarDTO
            {
                IdCurso = curso.IdCurso,
                Grado = curso.Grado,
                Curso = curso.Curso,
                Turno = curso.Turno,
                HorarioSalida = curso.ObtenerHorarioSalida(DateTime.Today.DayOfWeek),
                HorarioSalidaLunes = curso.HorarioLunes,
                HorarioSalidaMartes = curso.HorarioMartes,
                HorarioSalidaMiercoles = curso.HorarioMiercoles,
                HorarioSalidaJueves = curso.HorarioJueves,
                HorarioSalidaViernes = curso.HorarioViernes
            };
        }

        public async Task<IEnumerable<CursoEscolarDTO>> GetAllAsync()
        {
            var cursos = await cursoRepository.GetAllAsync();

            return cursos.Select(curso => new CursoEscolarDTO
            {
                IdCurso = curso.IdCurso,
                Grado = curso.Grado,
                Curso = curso.Curso,
                Turno = curso.Turno,
                HorarioSalida = curso.ObtenerHorarioSalida(DateTime.Today.DayOfWeek),
                HorarioSalidaLunes = curso.HorarioLunes,
                HorarioSalidaMartes = curso.HorarioMartes,
                HorarioSalidaMiercoles = curso.HorarioMiercoles,
                HorarioSalidaJueves = curso.HorarioJueves,
                HorarioSalidaViernes = curso.HorarioViernes
            }).ToList();
        }

        public async Task<bool> UpdateAsync(CursoEscolarDTO dto)
        {
            var existing = await cursoRepository.GetAsync(dto.IdCurso);
            if (existing == null)
                return false;

            existing.SetGrado(dto.Grado);
            existing.SetCurso(dto.Curso);
            existing.SetTurno(dto.Turno);

            var lunes = dto.HorarioSalidaLunes != TimeSpan.Zero ? dto.HorarioSalidaLunes : dto.HorarioSalida;
            var martes = dto.HorarioSalidaMartes != TimeSpan.Zero ? dto.HorarioSalidaMartes : dto.HorarioSalida;
            var miercoles = dto.HorarioSalidaMiercoles != TimeSpan.Zero ? dto.HorarioSalidaMiercoles : dto.HorarioSalida;
            var jueves = dto.HorarioSalidaJueves != TimeSpan.Zero ? dto.HorarioSalidaJueves : dto.HorarioSalida;
            var viernes = dto.HorarioSalidaViernes != TimeSpan.Zero ? dto.HorarioSalidaViernes : dto.HorarioSalida;

            existing.SetHorariosSemana(lunes, martes, miercoles, jueves, viernes);

            return await cursoRepository.UpdateAsync(existing);
        }
    }
}
