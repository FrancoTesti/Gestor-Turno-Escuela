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
            // Id en 0: el repositorio le asigna el número al agregar
            CursoEscolar curso = new CursoEscolar(0, dto.Grado, dto.Curso, dto.HorarioSalida);
            await cursoRepository.AddAsync(curso);

            dto.IdCurso = curso.IdCurso;
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
                HorarioSalida = curso.HorarioSalida
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
                HorarioSalida = curso.HorarioSalida
            }).ToList();
        }

        public async Task<bool> UpdateAsync(CursoEscolarDTO dto)
        {
            var existing = await cursoRepository.GetAsync(dto.IdCurso);
            if (existing == null)
                return false;

            // CursoEscolar usa propiedades públicas (no tiene métodos Set)
            existing.Grado = dto.Grado;
            existing.Curso = dto.Curso;
            existing.HorarioSalida = dto.HorarioSalida;

            return await cursoRepository.UpdateAsync(existing);
        }
    }
}