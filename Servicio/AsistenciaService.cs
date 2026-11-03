using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaRepository _asistenciaRepository;
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly ICursoEscolarRepository _cursoRepository;
        private readonly IRetiroRepository _retiroRepository;

        public AsistenciaService(
            IAsistenciaRepository asistenciaRepository,
            IAlumnoRepository alumnoRepository,
            ICursoEscolarRepository cursoRepository,
            IRetiroRepository retiroRepository)
        {
            _asistenciaRepository = asistenciaRepository ?? throw new ArgumentNullException(nameof(asistenciaRepository));
            _alumnoRepository = alumnoRepository ?? throw new ArgumentNullException(nameof(alumnoRepository));
            _cursoRepository = cursoRepository ?? throw new ArgumentNullException(nameof(cursoRepository));
            _retiroRepository = retiroRepository ?? throw new ArgumentNullException(nameof(retiroRepository));
        }

        public AsistenciaService()
            : this(new AsistenciaRepository(), new AlumnoRepository(), new CursoEscolarRepository(), new RetiroRepository(new GTEContext()))
        {
        }

        public async Task<List<TipoInasistenciaDTO>> GetTiposInasistenciaAsync()
        {
            var tipos = await _asistenciaRepository.GetTiposInasistenciaAsync(soloActivos: true);
            return (tipos ?? Enumerable.Empty<TipoInasistencia>())
                .Select(t => new TipoInasistenciaDTO
                {
                    IdTipoInasistencia = t.IdTipoInasistencia,
                    Nombre = t.Nombre,
                    ValorDecimal = t.ValorDecimal,
                    CodigoOficial = t.CodigoOficial,
                    EsActivo = t.EsActivo
                })
                .ToList();
        }

        public async Task<PlanillaDiariaDTO> GetPlanillaDiariaAsync(int idCurso, DateTime fecha)
        {
            if (idCurso <= 0)
                throw new ArgumentException("El Id de curso debe ser mayor a 0.", nameof(idCurso));

            var curso = await _cursoRepository.GetAsync(idCurso);
            if (curso == null)
                throw new ArgumentException($"No se encontró el curso con Id {idCurso}.", nameof(idCurso));

            DateTime fechaConsulta = fecha.Date;

            var planilla = await _asistenciaRepository.GetPlanillaDiariaAsync(idCurso, fechaConsulta);
            var tiposDisponibles = await GetTiposInasistenciaAsync();
            var tiposMap = tiposDisponibles.ToDictionary(t => t.IdTipoInasistencia);

            var todosAlumnos = await _alumnoRepository.GetAllAsync();
            var alumnosCurso = (todosAlumnos ?? Enumerable.Empty<Alumno>())
                .Where(a => a.IdCurso == idCurso)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToList();

            var inasistencias = (await _asistenciaRepository.GetInasistenciasPorCursoYFechaAsync(idCurso, fechaConsulta) ?? Enumerable.Empty<InasistenciaAlumno>())
                .ToDictionary(i => i.IdAlumno);

            var retiros = (await _asistenciaRepository.GetRetirosPorCursoYFechaAsync(idCurso, fechaConsulta) ?? Enumerable.Empty<DetalleRetiro>())
                .GroupBy(d => d.IdAlumno)
                .ToDictionary(g => g.Key, g => g.OrderBy(d => d.HoraSalida).First());

            var items = new List<AlumnoAsistenciaItemDTO>();

            foreach (var alumno in alumnosCurso)
            {
                var item = new AlumnoAsistenciaItemDTO
                {
                    IdAlumno = alumno.IdAlumno,
                    Nombre = alumno.Nombre,
                    Apellido = alumno.Apellido
                };

                if (inasistencias.TryGetValue(alumno.IdAlumno, out var inasistencia))
                {
                    item.EstadoGeneral = "Inasistencia";
                    item.IdInasistencia = inasistencia.IdInasistencia;
                    item.IdTipoInasistencia = inasistencia.IdTipoInasistencia;
                    item.CodigoOficial = inasistencia.TipoInasistencia?.CodigoOficial
                        ?? (tiposMap.TryGetValue(inasistencia.IdTipoInasistencia, out var tipoDTO) ? tipoDTO.CodigoOficial : "A");
                    item.Justificada = inasistencia.Justificada;
                    item.Motivo = inasistencia.Motivo;
                    item.NoComputa = inasistencia.NoComputa;
                    item.Observaciones = inasistencia.Observaciones;
                    item.IdRetiroOrigen = inasistencia.IdRetiroOrigen;
                }
                else
                {
                    item.EstadoGeneral = "Presente";
                    item.CodigoOficial = "P";
                    item.Justificada = false;
                    item.Motivo = string.Empty;
                    item.NoComputa = false;
                    item.Observaciones = string.Empty;
                }

                if (retiros.TryGetValue(alumno.IdAlumno, out var detalleRetiro))
                {
                    item.TieneRetiroRegistrado = true;
                    item.DetalleRetiroInfo = $"Retiro registrado a las {detalleRetiro.HoraSalida:hh\\:mm} hs";
                    item.IdRetiroOrigen ??= detalleRetiro.IdRetiro;
                }

                items.Add(item);
            }

            return new PlanillaDiariaDTO
            {
                IdCurso = idCurso,
                CursoNombre = $"{curso.Grado} {curso.Curso}",
                Turno = curso.Turno,
                Fecha = fechaConsulta,
                Confirmada = planilla?.Confirmada ?? false,
                FechaHoraRegistro = planilla?.FechaHoraRegistro,
                Alumnos = items,
                TiposDisponibles = tiposDisponibles
            };
        }

        public async Task<(bool Exito, string Mensaje)> GuardarPlanillaDiariaAsync(GuardarPlanillaAsistenciaRequestDTO request)
        {
            if (request == null || request.IdCurso <= 0)
                return (false, "Los datos de la planilla son inválidos.");

            try
            {
                DateTime fecha = request.Fecha.Date;

                var inasistenciasLimpias = (request.Inasistencias ?? new List<InasistenciaInputDTO>())
                    .GroupBy(x => x.IdAlumno)
                    .Select(g => g.Last())
                    .Where(x => x.IdAlumno > 0 && x.IdTipoInasistencia > 0)
                    .ToList();

                var planilla = new PlanillaAsistenciaDiaria(request.IdCurso, fecha, confirmada: true, idPersonalRegistrador: request.IdPersonal);

                var entities = inasistenciasLimpias.Select(x => new InasistenciaAlumno(
                    x.IdAlumno,
                    fecha,
                    x.IdTipoInasistencia,
                    x.Justificada,
                    x.Motivo ?? string.Empty,
                    x.NoComputa,
                    x.Observaciones ?? string.Empty,
                    x.IdRetiroOrigen
                )).ToList();

                await _asistenciaRepository.GuardarPlanillaConInasistenciasAsync(planilla, entities);

                return (true, "Planilla guardada y confirmada exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar la planilla: {ex.Message}");
            }
        }

        public async Task<List<AlumnoAsistenciaItemDTO>> SugerirInasistenciasDesdeRetirosAsync(int idCurso, DateTime fecha)
        {
            if (idCurso <= 0)
                throw new ArgumentException("El Id de curso debe ser mayor a 0.", nameof(idCurso));

            var curso = await _cursoRepository.GetAsync(idCurso);
            if (curso == null)
                throw new ArgumentException($"No se encontró el curso con Id {idCurso}.", nameof(idCurso));

            DateTime fechaConsulta = fecha.Date;
            TimeSpan horarioSalida = curso.ObtenerHorarioSalida(fechaConsulta);

            var retiros = (await _asistenciaRepository.GetRetirosPorCursoYFechaAsync(idCurso, fechaConsulta) ?? Enumerable.Empty<DetalleRetiro>()).ToList();
            var retirosPorAlumno = retiros
                .GroupBy(d => d.IdAlumno)
                .ToDictionary(g => g.Key, g => g.OrderBy(d => d.HoraSalida).First());

            var tipos = (await _asistenciaRepository.GetTiposInasistenciaAsync(soloActivos: true) ?? Enumerable.Empty<TipoInasistencia>()).ToList();
            var tipoMedia = tipos.FirstOrDefault(t => t.CodigoOficial == "M" || t.ValorDecimal == 0.50m);
            var tipoCuarta = tipos.FirstOrDefault(t => t.CodigoOficial == "C" || t.ValorDecimal == 0.25m);

            int idTipoMedia = tipoMedia?.IdTipoInasistencia ?? 2;
            int idTipoCuarta = tipoCuarta?.IdTipoInasistencia ?? 3;

            var todosAlumnos = await _alumnoRepository.GetAllAsync();
            var alumnosCurso = (todosAlumnos ?? Enumerable.Empty<Alumno>())
                .Where(a => a.IdCurso == idCurso)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToList();

            var resultados = new List<AlumnoAsistenciaItemDTO>();

            foreach (var alumno in alumnosCurso)
            {
                var item = new AlumnoAsistenciaItemDTO
                {
                    IdAlumno = alumno.IdAlumno,
                    Nombre = alumno.Nombre,
                    Apellido = alumno.Apellido
                };

                if (retirosPorAlumno.TryGetValue(alumno.IdAlumno, out var detalleRetiro))
                {
                    TimeSpan horaSalida = detalleRetiro.HoraSalida;
                    TimeSpan delta = horarioSalida - horaSalida;

                    item.TieneRetiroRegistrado = true;
                    item.IdRetiroOrigen = detalleRetiro.IdRetiro;

                    if (delta <= TimeSpan.Zero)
                    {
                        // Se retiró en o después del horario regular de salida -> Presente
                        item.EstadoGeneral = "Presente";
                        item.CodigoOficial = "P";
                        item.DetalleRetiroInfo = $"Retiro registrado a las {horaSalida:hh\\:mm} hs (horario regular)";
                        item.IdTipoInasistencia = null;
                        item.Justificada = false;
                        item.Motivo = string.Empty;
                        item.NoComputa = false;
                    }
                    else if (delta > TimeSpan.FromHours(2))
                    {
                        // Quedan más de 2 horas de clase -> Media Falta (0.50m)
                        item.EstadoGeneral = "Inasistencia";
                        item.IdTipoInasistencia = idTipoMedia;
                        item.CodigoOficial = "M";
                        item.Justificada = true;
                        item.Motivo = $"Retiro anticipado ({horaSalida:hh\\:mm} hs - horario regular {horarioSalida:hh\\:mm} hs)";
                        item.NoComputa = false;
                        item.DetalleRetiroInfo = $"Retiro registrado a las {horaSalida:hh\\:mm} hs";
                    }
                    else
                    {
                        // Quedan 2 horas o menos de clase -> Cuarta Falta (0.25m)
                        item.EstadoGeneral = "Inasistencia";
                        item.IdTipoInasistencia = idTipoCuarta;
                        item.CodigoOficial = "C";
                        item.Justificada = true;
                        item.Motivo = $"Retiro anticipado ({horaSalida:hh\\:mm} hs - horario regular {horarioSalida:hh\\:mm} hs)";
                        item.NoComputa = false;
                        item.DetalleRetiroInfo = $"Retiro registrado a las {horaSalida:hh\\:mm} hs";
                    }
                }
                else
                {
                    item.EstadoGeneral = "Presente";
                    item.CodigoOficial = "P";
                    item.TieneRetiroRegistrado = false;
                    item.DetalleRetiroInfo = null;
                    item.IdRetiroOrigen = null;
                    item.IdTipoInasistencia = null;
                    item.Justificada = false;
                    item.Motivo = string.Empty;
                    item.NoComputa = false;
                }

                resultados.Add(item);
            }

            return resultados;
        }

        public async Task<List<ReporteNovedadItemDTO>> GetReporteNovedadesAsync(int idCurso, DateTime fechaDesde, DateTime fechaHasta)
        {
            if (idCurso <= 0)
                return new List<ReporteNovedadItemDTO>();

            DateTime desde = fechaDesde.Date;
            DateTime hasta = fechaHasta.Date;
            if (desde > hasta)
                (desde, hasta) = (hasta, desde);

            var curso = await _cursoRepository.GetAsync(idCurso);
            string cursoNombre = curso != null ? $"{curso.Grado} {curso.Curso}" : string.Empty;
            string turno = curso?.Turno ?? string.Empty;

            var alumnos = (await _alumnoRepository.GetAllAsync() ?? Enumerable.Empty<Alumno>())
                .Where(a => a.IdCurso == idCurso)
                .ToList();
            var alumnosMap = alumnos.ToDictionary(a => a.IdAlumno);

            var inasistencias = (await _asistenciaRepository.GetInasistenciasPorCursoYRangoAsync(idCurso, desde, hasta) ?? Enumerable.Empty<InasistenciaAlumno>())
                .ToList();

            var allRetiros = (await _retiroRepository.GetAllAsync() ?? Enumerable.Empty<Retiro>()).ToList();

            var items = new List<ReporteNovedadItemDTO>();

            foreach (var inasistencia in inasistencias)
            {
                string nombreAlumno = inasistencia.Alumno != null
                    ? $"{inasistencia.Alumno.Apellido}, {inasistencia.Alumno.Nombre}"
                    : (alumnosMap.TryGetValue(inasistencia.IdAlumno, out var a) ? $"{a.Apellido}, {a.Nombre}" : $"Alumno #{inasistencia.IdAlumno}");

                string diaSemana = inasistencia.Fecha.ToString("dddd", new CultureInfo("es-AR"));
                if (diaSemana.Length > 0)
                    diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

                items.Add(new ReporteNovedadItemDTO
                {
                    Fecha = inasistencia.Fecha,
                    DiaSemana = diaSemana,
                    IdAlumno = inasistencia.IdAlumno,
                    AlumnoNombreCompleto = nombreAlumno,
                    Curso = cursoNombre,
                    Turno = turno,
                    TipoInasistencia = inasistencia.TipoInasistencia?.Nombre ?? "Inasistencia",
                    ValorFalta = inasistencia.TipoInasistencia?.ValorDecimal ?? 1.0m,
                    Justificada = inasistencia.Justificada,
                    Motivo = inasistencia.Motivo,
                    NoComputa = inasistencia.NoComputa,
                    Observaciones = inasistencia.Observaciones
                });
            }

            var idsRetirosConInasistencia = inasistencias
                .Where(i => i.IdRetiroOrigen.HasValue)
                .Select(i => i.IdRetiroOrigen!.Value)
                .ToHashSet();

            foreach (var retiro in allRetiros.Where(r => r.FechaHora.Date >= desde && r.FechaHora.Date <= hasta))
            {
                if (retiro.Detalles == null) continue;

                foreach (var detalle in retiro.Detalles)
                {
                    if (alumnosMap.TryGetValue(detalle.IdAlumno, out var alumno) && !idsRetirosConInasistencia.Contains(retiro.IdRetiro))
                    {
                        bool yaExiste = inasistencias.Any(i => i.IdAlumno == detalle.IdAlumno && i.Fecha.Date == retiro.FechaHora.Date);
                        if (!yaExiste)
                        {
                            string diaSemana = retiro.FechaHora.ToString("dddd", new CultureInfo("es-AR"));
                            if (diaSemana.Length > 0)
                                diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

                            items.Add(new ReporteNovedadItemDTO
                            {
                                Fecha = retiro.FechaHora.Date,
                                DiaSemana = diaSemana,
                                IdAlumno = detalle.IdAlumno,
                                AlumnoNombreCompleto = $"{alumno.Apellido}, {alumno.Nombre}",
                                Curso = cursoNombre,
                                Turno = turno,
                                TipoInasistencia = "Retiro anticipado",
                                ValorFalta = 0m,
                                Justificada = true,
                                Motivo = $"Retiro registrado a las {detalle.HoraSalida:hh\\:mm} hs",
                                NoComputa = true,
                                Observaciones = retiro.Observaciones ?? string.Empty
                            });
                        }
                    }
                }
            }

            return items
                .OrderBy(x => x.Fecha)
                .ThenBy(x => x.AlumnoNombreCompleto)
                .ToList();
        }

        public async Task<ReporteEspejoSemanaDTO> GetReporteEspejoSemanalAsync(int idCurso, DateTime fechaSemana)
        {
            if (idCurso <= 0)
                throw new ArgumentException("El Id de curso debe ser mayor a 0.", nameof(idCurso));

            int diff = (7 + (fechaSemana.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime lunes = fechaSemana.Date.AddDays(-1 * diff);
            DateTime martes = lunes.AddDays(1);
            DateTime miercoles = lunes.AddDays(2);
            DateTime jueves = lunes.AddDays(3);
            DateTime viernes = lunes.AddDays(4);

            var curso = await _cursoRepository.GetAsync(idCurso);
            string cursoNombre = curso != null ? $"{curso.Grado} {curso.Curso}" : string.Empty;
            string turno = curso?.Turno ?? string.Empty;

            var alumnos = (await _alumnoRepository.GetAllAsync() ?? Enumerable.Empty<Alumno>())
                .Where(a => a.IdCurso == idCurso)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToList();

            var inasistenciasSemana = (await _asistenciaRepository.GetInasistenciasPorCursoYRangoAsync(idCurso, lunes, viernes) ?? Enumerable.Empty<InasistenciaAlumno>())
                .ToList();

            var tipos = (await _asistenciaRepository.GetTiposInasistenciaAsync(soloActivos: false) ?? Enumerable.Empty<TipoInasistencia>())
                .ToDictionary(t => t.IdTipoInasistencia);

            var filas = new List<FilaEspejoAlumnoDTO>();

            foreach (var alumno in alumnos)
            {
                var inasistenciasAlumno = inasistenciasSemana.Where(i => i.IdAlumno == alumno.IdAlumno).ToList();
                var inasistenciasPorFecha = inasistenciasAlumno.ToDictionary(i => i.Fecha.Date);

                string codLunes = ObtenerCodigoEspejo(inasistenciasPorFecha.GetValueOrDefault(lunes), tipos);
                string codMartes = ObtenerCodigoEspejo(inasistenciasPorFecha.GetValueOrDefault(martes), tipos);
                string codMiercoles = ObtenerCodigoEspejo(inasistenciasPorFecha.GetValueOrDefault(miercoles), tipos);
                string codJueves = ObtenerCodigoEspejo(inasistenciasPorFecha.GetValueOrDefault(jueves), tipos);
                string codViernes = ObtenerCodigoEspejo(inasistenciasPorFecha.GetValueOrDefault(viernes), tipos);

                decimal totalFaltas = inasistenciasAlumno
                    .Where(i => !i.NoComputa)
                    .Sum(i => i.TipoInasistencia?.ValorDecimal ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t) ? t.ValorDecimal : 1.0m));

                filas.Add(new FilaEspejoAlumnoDTO
                {
                    IdAlumno = alumno.IdAlumno,
                    Alumno = $"{alumno.Apellido}, {alumno.Nombre}",
                    Lunes = codLunes,
                    Martes = codMartes,
                    Miercoles = codMiercoles,
                    Jueves = codJueves,
                    Viernes = codViernes,
                    TotalFaltasPeriodo = totalFaltas
                });
            }

            return new ReporteEspejoSemanaDTO
            {
                IdCurso = idCurso,
                Curso = cursoNombre,
                Turno = turno,
                FechaLunes = lunes,
                FechaViernes = viernes,
                Filas = filas
            };
        }

        public async Task<HistorialAsistenciaAlumnoDTO> GetHistorialAlumnoAsync(int idAlumno)
        {
            if (idAlumno <= 0)
                throw new ArgumentException("El Id de alumno debe ser mayor a 0.", nameof(idAlumno));

            var alumno = await _alumnoRepository.GetAsync(idAlumno);
            string nombreCompleto = alumno != null ? $"{alumno.Apellido}, {alumno.Nombre}" : string.Empty;

            var inasistencias = (await _asistenciaRepository.GetInasistenciasPorAlumnoAsync(idAlumno) ?? Enumerable.Empty<InasistenciaAlumno>())
                .ToList();

            var tipos = (await _asistenciaRepository.GetTiposInasistenciaAsync(soloActivos: false) ?? Enumerable.Empty<TipoInasistencia>())
                .ToDictionary(t => t.IdTipoInasistencia);

            decimal totalComputables = inasistencias
                .Where(i => !i.NoComputa)
                .Sum(i => i.TipoInasistencia?.ValorDecimal ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t) ? t.ValorDecimal : 1.0m));

            decimal totalJustificadas = inasistencias
                .Where(i => !i.NoComputa && i.Justificada)
                .Sum(i => i.TipoInasistencia?.ValorDecimal ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t) ? t.ValorDecimal : 1.0m));

            decimal totalInjustificadas = inasistencias
                .Where(i => !i.NoComputa && !i.Justificada)
                .Sum(i => i.TipoInasistencia?.ValorDecimal ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t) ? t.ValorDecimal : 1.0m));

            var dtos = inasistencias
                .OrderByDescending(i => i.Fecha)
                .Select(i =>
                {
                    decimal valor = i.TipoInasistencia?.ValorDecimal ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t) ? t.ValorDecimal : 1.0m);
                    string nombreTipo = i.TipoInasistencia?.Nombre ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t2) ? t2.Nombre : string.Empty);
                    string codigo = i.TipoInasistencia?.CodigoOficial ?? (tipos.TryGetValue(i.IdTipoInasistencia, out var t3) ? t3.CodigoOficial : string.Empty);

                    return new InasistenciaAlumnoDTO
                    {
                        IdInasistencia = i.IdInasistencia,
                        IdAlumno = i.IdAlumno,
                        NombreAlumno = i.Alumno?.Nombre ?? alumno?.Nombre ?? string.Empty,
                        ApellidoAlumno = i.Alumno?.Apellido ?? alumno?.Apellido ?? string.Empty,
                        Fecha = i.Fecha,
                        IdTipoInasistencia = i.IdTipoInasistencia,
                        NombreTipoInasistencia = nombreTipo,
                        CodigoOficial = codigo,
                        ValorDecimal = valor,
                        Justificada = i.Justificada,
                        Motivo = i.Motivo,
                        NoComputa = i.NoComputa,
                        Observaciones = i.Observaciones,
                        IdRetiroOrigen = i.IdRetiroOrigen
                    };
                })
                .ToList();

            return new HistorialAsistenciaAlumnoDTO
            {
                IdAlumno = idAlumno,
                NombreCompleto = nombreCompleto,
                TotalFaltasComputables = totalComputables,
                TotalFaltasJustificadas = totalJustificadas,
                TotalFaltasInjustificadas = totalInjustificadas,
                Inasistencias = dtos
            };
        }

        private static string ObtenerCodigoEspejo(InasistenciaAlumno? inasistencia, Dictionary<int, TipoInasistencia> tipos)
        {
            if (inasistencia == null || inasistencia.NoComputa)
                return "P";

            decimal valor = inasistencia.TipoInasistencia?.ValorDecimal
                ?? (tipos.TryGetValue(inasistencia.IdTipoInasistencia, out var t) ? t.ValorDecimal : 1.0m);

            string codigo = inasistencia.TipoInasistencia?.CodigoOficial
                ?? (tipos.TryGetValue(inasistencia.IdTipoInasistencia, out var t2) ? t2.CodigoOficial : string.Empty);

            if (valor == 0.50m || codigo == "M")
                return "M";

            if (valor == 0.25m || codigo == "C")
                return "C";

            if (valor == 0.75m || codigo == "TC")
                return "TC";

            // Falta entera (1.0m): if Justificada == true -> "J", else -> "I" (or "A")
            if (inasistencia.Justificada)
                return "J";

            return "I";
        }
    }
}
