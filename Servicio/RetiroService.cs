using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class RetiroService : IRetiroService
    {
        private readonly IRetiroRepository _retiroRepository;
        private readonly IAutorizacionRepository _autorizacionRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IAlumnoRepository _alumnoRepository;

        public RetiroService(
            IRetiroRepository retiroRepository,
            IAutorizacionRepository autorizacionRepository,
            ITutorRepository tutorRepository,
            IAlumnoRepository alumnoRepository)
        {
            _retiroRepository = retiroRepository;
            _autorizacionRepository = autorizacionRepository;
            _tutorRepository = tutorRepository;
            _alumnoRepository = alumnoRepository;
        }

        public async Task<IEnumerable<RetiroDTO>> GetAllAsync()
        {
            var retiros = await _retiroRepository.GetAllAsync();
            return retiros.Select(MapToDTO).ToList();
        }

        public async Task<RetiroDTO?> GetAsync(int id)
        {
            var retiro = await _retiroRepository.GetAsync(id);
            return retiro == null ? null : MapToDTO(retiro);
        }

        public async Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> AddAsync(RetiroDTO dto)
        {
            if (dto.IdTutor <= 0)
                return (false, "Debe seleccionar un tutor válido.", null);

            if (dto.IdPersonal <= 0)
                return (false, "Debe seleccionar un personal válido.", null);

            if (dto.Detalles == null || dto.Detalles.Count == 0)
                return (false, "El retiro debe contener al menos un alumno.", null);

            if (dto.FechaHora > DateTime.Now.AddMinutes(5))
                return (false, "La fecha y hora del retiro no puede ser futura.", null);

            // Validar que no haya alumnos repetidos en el detalle
            if (dto.Detalles.GroupBy(d => d.IdAlumno).Any(g => g.Count() > 1))
                return (false, "No se puede incluir al mismo alumno más de una vez en el retiro.", null);

            // Validar tutor
            var tutor = await _tutorRepository.GetAsync(dto.IdTutor);
            if (tutor == null)
                return (false, "El tutor especificado no existe.", null);

            if (tutor.TieneRestriccion)
                return (false, $"El tutor {tutor.Nombre} {tutor.Apellido} posee restricciones legales para retirar alumnos.", null);

            // Validaciones por cada alumno
            foreach (var det in dto.Detalles)
            {
                var alumno = await _alumnoRepository.GetAsync(det.IdAlumno);
                if (alumno == null)
                    return (false, $"El alumno con ID {det.IdAlumno} no existe.", null);

                if (alumno.Estado != "Presente")
                    return (false, $"El alumno {alumno.Nombre} {alumno.Apellido} no se encuentra presente (Estado actual: '{alumno.Estado}').", null);

                bool autorizado = await _autorizacionRepository.EstaAutorizadoAsync(dto.IdTutor, det.IdAlumno);
                if (!autorizado)
                    return (false, $"El tutor {tutor.Nombre} {tutor.Apellido} no se encuentra autorizado para retirar al alumno {alumno.Nombre} {alumno.Apellido}.", null);
            }

            // Construir entidad de dominio y agregar líneas
            var retiro = new Retiro(
                dto.IdTutor,
                dto.IdPersonal,
                dto.FechaHora == default ? DateTime.Now : dto.FechaHora,
                dto.Observaciones ?? string.Empty
            );

            foreach (var det in dto.Detalles)
            {
                var horaSalida = det.HoraSalida == default ? DateTime.Now.TimeOfDay : det.HoraSalida;
                var detalle = new DetalleRetiro(det.IdAlumno, horaSalida, string.IsNullOrWhiteSpace(det.Estado) ? "Retirado" : det.Estado);
                retiro.AgregarDetalle(detalle);
            }

            retiro.Validar();

            // Guardar retiro
            await _retiroRepository.AddAsync(retiro);

            // Actualizar estado de los alumnos a "Retirado"
            foreach (var det in dto.Detalles)
            {
                var alumno = await _alumnoRepository.GetAsync(det.IdAlumno);
                if (alumno != null)
                {
                    alumno.SetEstado("Retirado");
                    await _alumnoRepository.UpdateAsync(alumno);
                }
            }

            // Recuperar completo con relaciones
            var creado = await _retiroRepository.GetAsync(retiro.IdRetiro);
            return (true, "Retiro registrado correctamente.", creado == null ? null : MapToDTO(creado));
        }

        public async Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> UpdateAsync(RetiroDTO dto)
        {
            if (dto.IdRetiro <= 0)
                return (false, "Identificador de retiro inválido.", null);

            var existing = await _retiroRepository.GetAsync(dto.IdRetiro);
            if (existing == null)
                return (false, "El retiro no existe.", null);

            if (dto.IdTutor <= 0)
                return (false, "Debe seleccionar un tutor válido.", null);

            if (dto.IdPersonal <= 0)
                return (false, "Debe seleccionar un personal válido.", null);

            if (dto.Detalles == null || dto.Detalles.Count == 0)
                return (false, "El retiro debe contener al menos un alumno.", null);

            if (dto.Detalles.GroupBy(d => d.IdAlumno).Any(g => g.Count() > 1))
                return (false, "No se puede incluir al mismo alumno más de una vez en el retiro.", null);

            var tutor = await _tutorRepository.GetAsync(dto.IdTutor);
            if (tutor == null)
                return (false, "El tutor especificado no existe.", null);

            if (tutor.TieneRestriccion)
                return (false, $"El tutor {tutor.Nombre} {tutor.Apellido} posee restricciones legales.", null);

            foreach (var det in dto.Detalles)
            {
                var alumno = await _alumnoRepository.GetAsync(det.IdAlumno);
                if (alumno == null)
                    return (false, $"El alumno con ID {det.IdAlumno} no existe.", null);

                bool autorizado = await _autorizacionRepository.EstaAutorizadoAsync(dto.IdTutor, det.IdAlumno);
                if (!autorizado)
                    return (false, $"El tutor {tutor.Nombre} {tutor.Apellido} no está autorizado para el alumno {alumno.Nombre} {alumno.Apellido}.", null);
            }

            var retiro = new Retiro(
                dto.IdRetiro,
                dto.IdTutor,
                dto.IdPersonal,
                dto.FechaHora,
                dto.Observaciones ?? string.Empty
            );

            foreach (var det in dto.Detalles)
            {
                var detalle = new DetalleRetiro(det.IdDetalleRetiro, dto.IdRetiro, det.IdAlumno, det.HoraSalida, det.Estado);
                retiro.AgregarDetalle(detalle);
            }

            retiro.Validar();

            bool actualizado = await _retiroRepository.UpdateAsync(retiro);
            if (!actualizado)
                return (false, "No se pudo actualizar el retiro.", null);

            var modificado = await _retiroRepository.GetAsync(dto.IdRetiro);
            return (true, "Retiro modificado correctamente.", modificado == null ? null : MapToDTO(modificado));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _retiroRepository.DeleteAsync(id);
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

        private static RetiroDTO MapToDTO(Retiro r)
        {
            return new RetiroDTO
            {
                IdRetiro = r.IdRetiro,
                IdTutor = r.IdTutor,
                TutorNombreCompleto = r.Tutor != null ? $"{r.Tutor.Nombre} {r.Tutor.Apellido}" : null,
                IdPersonal = r.IdPersonal,
                PersonalNombre = r.Personal != null ? r.Personal.Nombre : null,
                FechaHora = r.FechaHora,
                Observaciones = r.Observaciones,
                Detalles = r.Detalles.Select(d => new DetalleRetiroDTO
                {
                    IdDetalleRetiro = d.IdDetalleRetiro,
                    IdRetiro = d.IdRetiro,
                    IdAlumno = d.IdAlumno,
                    AlumnoNombreCompleto = d.Alumno != null ? $"{d.Alumno.Nombre} {d.Alumno.Apellido}" : null,
                    HoraSalida = d.HoraSalida,
                    Estado = d.Estado
                }).ToList()
            };
        }
    }
}
