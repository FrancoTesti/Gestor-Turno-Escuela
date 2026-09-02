using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;


namespace GTE.Application.Services
{
    public class AlumnoService : IAlumnoService
    {
        private readonly IAlumnoRepository alumnoRepository;
        private readonly ICursoEscolarRepository cursoRepository;

        public AlumnoService(IAlumnoRepository alumnoRepository, ICursoEscolarRepository cursoRepository)
        {
            this.alumnoRepository = alumnoRepository;
            this.cursoRepository = cursoRepository;
        }

        public async Task<AlumnoDTO> AddAsync(AlumnoDTO dto)
        {
            var curso = await cursoRepository.GetAsync(dto.IdCurso)
                ?? throw new ArgumentException("El curso seleccionado no existe.", nameof(dto.IdCurso));
            Alumno alumno = new Alumno(dto.IdAlumno, dto.Nombre, dto.Apellido, curso.IdCurso);
            await alumnoRepository.AddAsync(alumno);

            dto.IdAlumno = alumno.IdAlumno;
            dto.Estado = alumno.Estado;
            CompletarDatosCurso(dto, curso);
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await alumnoRepository.DeleteAsync(id);
        }

        public async Task<AlumnoDTO?> GetAsync(int id)
        {
            Alumno? alumno = await alumnoRepository.GetAsync(id);

            if (alumno == null)
                return null;

            return new AlumnoDTO
            {
                IdAlumno = alumno.IdAlumno,
                Nombre = alumno.Nombre,
                Apellido = alumno.Apellido,
                IdCurso = alumno.IdCurso,
                Grado = alumno.CursoEscolar.Grado,
                Curso = alumno.CursoEscolar.Curso,
                Turno = alumno.CursoEscolar.Turno,
                Estado = alumno.Estado
            };
        }

        public async Task<IEnumerable<AlumnoDTO>> GetAllAsync()
        {
            var alumnos = await alumnoRepository.GetAllAsync();

            return alumnos.Select(alumno => new AlumnoDTO
            {
                IdAlumno = alumno.IdAlumno,
                Nombre = alumno.Nombre,
                Apellido = alumno.Apellido,
                IdCurso = alumno.IdCurso,
                Grado = alumno.CursoEscolar.Grado,
                Curso = alumno.CursoEscolar.Curso,
                Turno = alumno.CursoEscolar.Turno,
                Estado = alumno.Estado
            }).ToList();
        }

        public async Task<bool> UpdateAsync(AlumnoDTO dto)
        {
            var existing = await alumnoRepository.GetAsync(dto.IdAlumno);
            if (existing == null)
                return false;

            existing.SetNombre(dto.Nombre);
            existing.SetApellido(dto.Apellido);
            if (await cursoRepository.GetAsync(dto.IdCurso) == null)
                throw new ArgumentException("El curso seleccionado no existe.", nameof(dto.IdCurso));
            existing.SetCurso(dto.IdCurso);

            if (!string.IsNullOrEmpty(dto.Estado))
            {
                existing.SetEstado(dto.Estado);
            }

            return await alumnoRepository.UpdateAsync(existing);
        }

        public async Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(AlumnoCriteriaDTO criteriaDTO)
        {
            var criteria = new AlumnoCriteria(criteriaDTO.Nombre, criteriaDTO.Grado, criteriaDTO.Curso, criteriaDTO.Estado);

            var clientes = await alumnoRepository.GetByCriteriaAsync(criteria);

            return clientes.Select(a => new AlumnoDTO
            {
                IdAlumno = a.IdAlumno,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                IdCurso = a.IdCurso,
                Grado = a.CursoEscolar.Grado,
                Curso = a.CursoEscolar.Curso,
                Turno = a.CursoEscolar.Turno,
                Estado = a.Estado,
            });
        }

        private static void CompletarDatosCurso(AlumnoDTO dto, CursoEscolar curso)
        {
            dto.IdCurso = curso.IdCurso;
            dto.Grado = curso.Grado;
            dto.Curso = curso.Curso;
            dto.Turno = curso.Turno;
        }
    }
}
