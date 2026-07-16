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

        public AlumnoService(IAlumnoRepository alumnoRepository)
        {
            this.alumnoRepository = alumnoRepository;
        }

        public async Task<AlumnoDTO> AddAsync(AlumnoDTO dto)
        {
            // Se utiliza el constructor de tu clase Alumno
            Alumno alumno = new Alumno(dto.IdAlumno, dto.Nombre, dto.Apellido, dto.Grado, dto.Curso);
            await alumnoRepository.AddAsync(alumno);

            dto.IdAlumno = alumno.IdAlumno;
            dto.Estado = alumno.Estado; // El constructor por defecto lo setea en "Presente"
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
                Grado = alumno.Grado,
                Curso = alumno.Curso,
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
                Grado = alumno.Grado,
                Curso = alumno.Curso,
                Estado = alumno.Estado
            }).ToList();
        }

        public async Task<bool> UpdateAsync(AlumnoDTO dto)
        {
            // Obtener el alumno existente
            var existing = await alumnoRepository.GetAsync(dto.IdAlumno);
            if (existing == null)
                return false;

            // Actualizar usando los métodos provistos por tu modelo de Dominio
            existing.SetNombre(dto.Nombre);
            existing.SetApellido(dto.Apellido);
            existing.SetGrado(dto.Grado);
            existing.SetCurso(dto.Curso);

            if (!string.IsNullOrEmpty(dto.Estado))
            {
                existing.SetEstado(dto.Estado);
            }

            return await alumnoRepository.UpdateAsync(existing);
        }

        public async Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(AlumnoCriteriaDTO criteriaDTO)
        {
            // Mapear DTO a Domain Model
            var criteria = new AlumnoCriteria(criteriaDTO.Nombre, criteriaDTO.Grado, criteriaDTO.Curso, criteriaDTO.Estado);

            // Llamar al repositorio
            var clientes = await alumnoRepository.GetByCriteriaAsync(criteria);

            // Mapear Domain Model a DTO
            return clientes.Select(a => new AlumnoDTO
            {
                IdAlumno = a.IdAlumno,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                Grado = a.Grado,
                Curso = a.Curso,
                Estado = a.Estado,
            });
        }
    }
}