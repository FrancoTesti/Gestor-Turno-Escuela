using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly IAutorizacionRepository _autorizacionRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IAlumnoRepository _alumnoRepository;

        public AutorizacionService(
            IAutorizacionRepository autorizacionRepository,
            ITutorRepository tutorRepository,
            IAlumnoRepository alumnoRepository)
        {
            _autorizacionRepository = autorizacionRepository;
            _tutorRepository = tutorRepository;
            _alumnoRepository = alumnoRepository;
        }

        public async Task<IEnumerable<AutorizacionDTO>> GetAllAsync()
        {
            var autorizaciones = await _autorizacionRepository.GetAllAsync();
            var dtos = new List<AutorizacionDTO>();

            foreach (var auto in autorizaciones)
            {
                var tutor = await _tutorRepository.GetAsync(auto.TutorId);
                var alumno = await _alumnoRepository.GetAsync(auto.AlumnoId);

                dtos.Add(new AutorizacionDTO
                {
                    IdAutorizacion = auto.IdAutorizacion,
                    TutorId = auto.TutorId,
                    TutorNombreCompleto = tutor != null ? $"{tutor.Nombre} {tutor.Apellido}" : null,
                    AlumnoId = auto.AlumnoId,
                    AlumnoNombreCompleto = alumno != null ? $"{alumno.Nombre} {alumno.Apellido}" : null,
                    Parentesco = auto.Parentesco
                });
            }

            return dtos;
        }

        public async Task<AutorizacionDTO?> GetAsync(int id)
        {
            var auto = await _autorizacionRepository.GetByIdAsync(id);
            if (auto == null) return null;

            var tutor = await _tutorRepository.GetAsync(auto.TutorId);
            var alumno = await _alumnoRepository.GetAsync(auto.AlumnoId);

            return new AutorizacionDTO
            {
                IdAutorizacion = auto.IdAutorizacion,
                TutorId = auto.TutorId,
                TutorNombreCompleto = tutor != null ? $"{tutor.Nombre} {tutor.Apellido}" : null,
                AlumnoId = auto.AlumnoId,
                AlumnoNombreCompleto = alumno != null ? $"{alumno.Nombre} {alumno.Apellido}" : null,
                Parentesco = auto.Parentesco
            };
        }

        public async Task<IEnumerable<AutorizacionDTO>> GetByTutorIdAsync(int tutorId)
        {
            var autorizaciones = await _autorizacionRepository.GetByTutorIdAsync(tutorId);
            var tutor = await _tutorRepository.GetAsync(tutorId);
            string? tutorNombre = tutor != null ? $"{tutor.Nombre} {tutor.Apellido}" : null;

            var dtos = new List<AutorizacionDTO>();
            foreach (var auto in autorizaciones)
            {
                var alumno = await _alumnoRepository.GetAsync(auto.AlumnoId);
                dtos.Add(new AutorizacionDTO
                {
                    IdAutorizacion = auto.IdAutorizacion,
                    TutorId = auto.TutorId,
                    TutorNombreCompleto = tutorNombre,
                    AlumnoId = auto.AlumnoId,
                    AlumnoNombreCompleto = alumno != null ? $"{alumno.Nombre} {alumno.Apellido}" : null,
                    Parentesco = auto.Parentesco
                });
            }

            return dtos;
        }

        public async Task<IEnumerable<AlumnoDTO>> GetAlumnosAutorizadosAsync(int tutorId)
        {
            var alumnos = await _autorizacionRepository.GetAlumnosByTutorIdAsync(tutorId);
            return alumnos.Select(a => new AlumnoDTO
            {
                IdAlumno = a.IdAlumno,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                Grado = a.CursoEscolar?.Grado ?? string.Empty,
                Curso = a.CursoEscolar?.Curso ?? string.Empty,
                Turno = a.CursoEscolar?.Turno ?? string.Empty,
                IdCurso = a.IdCurso,
                Estado = a.Estado
            }).ToList();
        }

        public async Task<(bool Exito, string Mensaje, AutorizacionDTO? Autorizacion)> AddAsync(AutorizacionDTO dto)
        {
            if (dto.TutorId <= 0)
                return (false, "Debe seleccionar un tutor válido.", null);

            if (dto.AlumnoId <= 0)
                return (false, "Debe seleccionar un alumno válido.", null);

            var tutor = await _tutorRepository.GetAsync(dto.TutorId);
            if (tutor == null)
                return (false, "El tutor especificado no existe.", null);

            var alumno = await _alumnoRepository.GetAsync(dto.AlumnoId);
            if (alumno == null)
                return (false, "El alumno especificado no existe.", null);

            bool yaAutorizado = await _autorizacionRepository.EstaAutorizadoAsync(dto.TutorId, dto.AlumnoId);
            if (yaAutorizado)
                return (false, "El tutor ya se encuentra autorizado para retirar a este alumno.", null);

            // Si no se eligió un parentesco se usa el que tiene cargado el tutor.
            string parentesco = string.IsNullOrWhiteSpace(dto.Parentesco) ? tutor.Parentesco : dto.Parentesco;

            var autorizacion = new Autorizacion(dto.AlumnoId, dto.TutorId, parentesco);
            await _autorizacionRepository.AddAsync(autorizacion);

            var resultadoDto = new AutorizacionDTO
            {
                IdAutorizacion = autorizacion.IdAutorizacion,
                TutorId = tutor.IdTutor,
                TutorNombreCompleto = $"{tutor.Nombre} {tutor.Apellido}",
                AlumnoId = alumno.IdAlumno,
                AlumnoNombreCompleto = $"{alumno.Nombre} {alumno.Apellido}",
                Parentesco = autorizacion.Parentesco
            };

            return (true, "Autorización registrada correctamente.", resultadoDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _autorizacionRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByTutorAndAlumnoAsync(int tutorId, int alumnoId)
        {
            return await _autorizacionRepository.DeleteByTutorAndAlumnoAsync(tutorId, alumnoId);
        }
    }
}
