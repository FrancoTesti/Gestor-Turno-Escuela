using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio
{
    public class HorarioEspecialService : IHorarioEspecialService
    {
        private readonly IHorarioEspecialRepository _horarioRepo;
        private readonly IAlumnoRepository _alumnoRepo;
        private readonly ICursoEscolarRepository? _cursoRepo;

        public HorarioEspecialService(IHorarioEspecialRepository horarioRepo, IAlumnoRepository alumnoRepo, ICursoEscolarRepository? cursoRepo = null)
        {
            _horarioRepo = horarioRepo;
            _alumnoRepo = alumnoRepo;
            _cursoRepo = cursoRepo;
        }

        private async Task<string> ObtenerCursoAlumnoAsync(Alumno? alu)
        {
            if (alu == null) return string.Empty;
            if (alu.CursoEscolar != null)
                return $"{alu.CursoEscolar.Grado} {alu.CursoEscolar.Curso}".Trim();

            if (_cursoRepo != null && alu.IdCurso > 0)
            {
                var c = await _cursoRepo.GetAsync(alu.IdCurso);
                if (c != null)
                    return $"{c.Grado} {c.Curso}".Trim();
            }

            return string.Empty;
        }

        public async Task<List<HorarioEspecialDTO>> ObtenerTodosAsync()
        {
            var horarios = await _horarioRepo.ObtenerTodosAsync();
            var dtos = new List<HorarioEspecialDTO>();
            foreach(var h in horarios)
            {
                var alu = await _alumnoRepo.GetAsync(h.IdAlumno);
                string cursoStr = await ObtenerCursoAlumnoAsync(alu);
                dtos.Add(new HorarioEspecialDTO
                {
                    IdHorarioEspecial = h.IdHorarioEspecial,
                    IdAlumno = h.IdAlumno,
                    AlumnoNombreCompleto = alu != null ? $"{alu.Nombre} {alu.Apellido}" : "",
                    Curso = cursoStr,
                    DescripcionActividad = h.DescripcionActividad,
                    HoraSalidaEspecial = h.HoraSalidaEspecial,
                    EsRecurrente = h.EsRecurrente,
                    DiasSemana = h.DiasSemana,
                    FechaEspecifica = h.FechaEspecifica
                });
            }
            return dtos;
        }

        public async Task<HorarioEspecialDTO?> ObtenerPorIdAsync(int id)
        {
            var h = await _horarioRepo.ObtenerPorIdAsync(id);
            if (h == null) return null;
            var alu = await _alumnoRepo.GetAsync(h.IdAlumno);
            string cursoStr = await ObtenerCursoAlumnoAsync(alu);
            return new HorarioEspecialDTO
            {
                IdHorarioEspecial = h.IdHorarioEspecial,
                IdAlumno = h.IdAlumno,
                AlumnoNombreCompleto = alu != null ? $"{alu.Nombre} {alu.Apellido}" : "",
                Curso = cursoStr,
                DescripcionActividad = h.DescripcionActividad,
                HoraSalidaEspecial = h.HoraSalidaEspecial,
                EsRecurrente = h.EsRecurrente,
                DiasSemana = h.DiasSemana,
                FechaEspecifica = h.FechaEspecifica
            };
        }

        public async Task<HorarioEspecialDTO> AgregarAsync(HorarioEspecialDTO dto)
        {
            Validar(dto);
            var alu = await _alumnoRepo.GetAsync(dto.IdAlumno);
            if (alu == null) throw new Exception("El alumno especificado no existe.");

            var entity = new HorarioEspecial(0, dto.IdAlumno, dto.DescripcionActividad, dto.HoraSalidaEspecial,
                dto.EsRecurrente, dto.DiasSemana, dto.FechaEspecifica);
            await _horarioRepo.AgregarAsync(entity);
            dto.IdHorarioEspecial = entity.IdHorarioEspecial;
            return dto;
        }

        public async Task ActualizarAsync(HorarioEspecialDTO dto)
        {
            Validar(dto);
            var h = await _horarioRepo.ObtenerPorIdAsync(dto.IdHorarioEspecial);
            if (h == null) throw new Exception("Horario especial no encontrado.");

            h.IdAlumno = dto.IdAlumno;
            h.DescripcionActividad = dto.DescripcionActividad;
            h.HoraSalidaEspecial = dto.HoraSalidaEspecial;
            h.EsRecurrente = dto.EsRecurrente;
            h.DiasSemana = dto.DiasSemana;
            h.FechaEspecifica = dto.FechaEspecifica;

            await _horarioRepo.ActualizarAsync(h);
        }

        public async Task EliminarAsync(int id)
        {
            await _horarioRepo.EliminarAsync(id);
        }

        private void Validar(HorarioEspecialDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DescripcionActividad))
                throw new Exception("La descripción no puede estar vacía.");
            if (dto.HoraSalidaEspecial == default)
                throw new Exception("Debe especificar una hora de salida válida.");
        }
    }
}
