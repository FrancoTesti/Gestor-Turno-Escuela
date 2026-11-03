using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class SalidaDeCursoService : ISalidaDeCursoService
    {
        private readonly ISalidaDeCursoRepository _salidaRepository;
        private readonly ICursoEscolarRepository _cursoRepository;
        private readonly IAlumnoRepository _alumnoRepository;

        public SalidaDeCursoService(
            ISalidaDeCursoRepository salidaRepository,
            ICursoEscolarRepository cursoRepository,
            IAlumnoRepository alumnoRepository)
        {
            _salidaRepository = salidaRepository;
            _cursoRepository = cursoRepository;
            _alumnoRepository = alumnoRepository;
        }

        public async Task<CarteleraDTO> GetCarteleraAsync(DateTime? momento = null)
        {
            var enCurso = (await _salidaRepository.GetEnCursoAsync()).ToList();
            var alumnos = (await _alumnoRepository.GetAllAsync()).ToList();

            var cartelera = new CarteleraDTO
            {
                EnSalida = enCurso.Select(salida => new CursoEnSalidaDTO
                {
                    Grado = salida.CursoEscolar.Grado,
                    Curso = salida.CursoEscolar.Curso,
                    Turno = salida.CursoEscolar.Turno,
                    Puerta = PuertaDe(salida.Personal),
                    FechaHoraInicio = salida.FechaHoraInicio,
                    CantidadDeAlumnos = alumnos.Count(a => a.IdCurso == salida.IdCurso)
                }).ToList()
            };

            DateTime actual = momento ?? DateTime.Now;
            DayOfWeek diaSemana = actual.DayOfWeek;
            TimeSpan horaActual = actual.TimeOfDay;

            var cursos = await _cursoRepository.GetAllAsync();

            DayOfWeek diaEvaluado = (diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday)
                ? DayOfWeek.Monday
                : diaSemana;

            CursoEscolar? proximo;
            if (diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday)
            {
                proximo = cursos
                    .Where(c => c.ObtenerHorarioSalida(diaEvaluado) >= horaActual)
                    .OrderBy(c => c.ObtenerHorarioSalida(diaEvaluado))
                    .FirstOrDefault()
                    ?? cursos
                        .OrderBy(c => c.ObtenerHorarioSalida(diaEvaluado))
                        .FirstOrDefault();
            }
            else
            {
                proximo = cursos
                    .Where(c => c.ObtenerHorarioSalida(diaEvaluado) >= horaActual)
                    .OrderBy(c => c.ObtenerHorarioSalida(diaEvaluado))
                    .FirstOrDefault();
            }

            if (proximo != null)
            {
                cartelera.Proximo = new CursoEscolarDTO
                {
                    IdCurso = proximo.IdCurso,
                    Grado = proximo.Grado,
                    Curso = proximo.Curso,
                    Turno = proximo.Turno,
                    HorarioSalida = proximo.ObtenerHorarioSalida(diaEvaluado),
                    HorarioSalidaLunes = proximo.HorarioLunes,
                    HorarioSalidaMartes = proximo.HorarioMartes,
                    HorarioSalidaMiercoles = proximo.HorarioMiercoles,
                    HorarioSalidaJueves = proximo.HorarioJueves,
                    HorarioSalidaViernes = proximo.HorarioViernes
                };
            }

            return cartelera;
        }

        public async Task<IEnumerable<SalidaDeCursoDTO>> GetDelDiaAsync(DateTime dia)
        {
            var salidas = await _salidaRepository.GetDelDiaAsync(dia);
            var alumnos = (await _alumnoRepository.GetAllAsync()).ToList();

            return salidas
                .Select(salida => Mapear(salida, alumnos.Count(a => a.IdCurso == salida.IdCurso)))
                .ToList();
        }

        public async Task<(bool Exito, string Mensaje, SalidaDeCursoDTO? Salida)> IniciarAsync(int idCurso, Personal personal)
        {
            var curso = await _cursoRepository.GetAsync(idCurso);
            if (curso == null)
                return (false, "El curso indicado no existe.", null);

            var enCurso = await _salidaRepository.GetEnCursoAsync();
            if (enCurso.Any(s => s.IdCurso == idCurso))
                return (false, $"El curso {curso.MostrarCurso()} ya está saliendo.", null);

            var salida = new SalidaDeCurso(curso, personal, DateTime.Now);
            await _salidaRepository.AddAsync(salida);

            var alumnos = await _alumnoRepository.GetAllAsync();
            int cantidad = alumnos.Count(a => a.IdCurso == idCurso);

            return (true, $"El curso {curso.MostrarCurso()} está saliendo.", Mapear(salida, cantidad));
        }

        public async Task<(bool Exito, string Mensaje)> FinalizarAsync(int idSalida)
        {
            var salida = await _salidaRepository.GetAsync(idSalida);
            if (salida == null)
                return (false, "La salida indicada no existe.");

            if (!salida.EstaEnCurso)
                return (false, "Esa salida ya estaba finalizada.");

            salida.Finalizar(DateTime.Now);
            await _salidaRepository.UpdateAsync(salida);

            return (true, $"El curso {salida.CursoEscolar.MostrarCurso()} terminó de salir.");
        }

        private static string PuertaDe(Personal personal) =>
            personal is Portero portero ? portero.PuertaAsignada : string.Empty;

        private static SalidaDeCursoDTO Mapear(SalidaDeCurso salida, int cantidadDeAlumnos)
        {
            return new SalidaDeCursoDTO
            {
                IdSalidaDeCurso = salida.IdSalidaDeCurso,
                IdCurso = salida.IdCurso,
                Grado = salida.CursoEscolar.Grado,
                Curso = salida.CursoEscolar.Curso,
                Turno = salida.CursoEscolar.Turno,
                HorarioSalida = salida.CursoEscolar.ObtenerHorarioSalida(salida.FechaHoraInicio.DayOfWeek),
                Puerta = PuertaDe(salida.Personal),
                PersonalNombre = salida.Personal.Nombre,
                FechaHoraInicio = salida.FechaHoraInicio,
                FechaHoraFin = salida.FechaHoraFin,
                CantidadDeAlumnos = cantidadDeAlumnos
            };
        }
    }
}
