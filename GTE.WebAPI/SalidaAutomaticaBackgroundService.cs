using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTE.Application.Services;
using GTE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GTE.WebAPI
{
    public class SalidaAutomaticaBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SalidaAutomaticaBackgroundService> _logger;
        private static readonly TimeSpan VentanaSalida = TimeSpan.FromMinutes(20);

        public SalidaAutomaticaBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<SalidaAutomaticaBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de salida de curso automática iniciado (duración de salida: 20 min).");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcesarSalidasAutomaticasAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al procesar salidas automáticas de curso.");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task ProcesarSalidasAutomaticasAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GTEContext>();
            var salidaService = scope.ServiceProvider.GetRequiredService<ISalidaDeCursoService>();

            DateTime ahoraCompleto = DateTime.Now;
            DateTime hoy = ahoraCompleto.Date;
            TimeSpan ahoraHora = ahoraCompleto.TimeOfDay;

            // 1. Finalizar salidas en curso que hayan superado los 20 minutos
            var salidasEnCurso = await db.SalidasDeCurso
                .Include(s => s.CursoEscolar)
                .Where(s => s.FechaHoraFin == null)
                .ToListAsync(stoppingToken);

            foreach (var salida in salidasEnCurso)
            {
                if (ahoraCompleto - salida.FechaHoraInicio >= VentanaSalida)
                {
                    try
                    {
                        salida.Finalizar(ahoraCompleto);
                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Salida automática finalizada tras 20 min para el curso {Curso}.",
                            salida.CursoEscolar?.MostrarCurso() ?? salida.IdCurso.ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "No se pudo finalizar automáticamente la salida {Id}.", salida.IdSalidaDeCurso);
                    }
                }
            }

            // 2. Iniciar salidas de cursos que llegaron a su horario de salida (lunes a viernes)
            if (hoy.DayOfWeek < DayOfWeek.Monday || hoy.DayOfWeek > DayOfWeek.Friday)
                return;

            var cursos = await db.Cursos.ToListAsync(stoppingToken);
            var salidasDeHoyIds = (await db.SalidasDeCurso
                .Where(s => s.FechaHoraInicio.Date == hoy)
                .Select(s => s.IdCurso)
                .ToListAsync(stoppingToken))
                .ToHashSet();

            var personalDefault = await db.Personal.FirstOrDefaultAsync(stoppingToken);
            if (personalDefault is null)
                return;

            foreach (var curso in cursos)
            {
                if (salidasDeHoyIds.Contains(curso.IdCurso))
                    continue;

                TimeSpan horarioSalida = curso.ObtenerHorarioSalida(hoy.DayOfWeek);
                if (horarioSalida <= TimeSpan.Zero)
                    continue;

                // Si ya es la hora de salida y estamos dentro de la ventana de 20 minutos
                if (ahoraHora >= horarioSalida && ahoraHora < horarioSalida.Add(VentanaSalida))
                {
                    var (exito, mensaje, salidaCreada) = await salidaService.IniciarAsync(curso.IdCurso, personalDefault);
                    if (exito)
                    {
                        salidasDeHoyIds.Add(curso.IdCurso);
                        _logger.LogInformation("Salida automática iniciada para el curso {Curso} a las {Hora:hh\\:mm}.",
                            curso.MostrarCurso(), ahoraHora);
                    }
                    else
                    {
                        _logger.LogDebug("No se pudo iniciar salida automática para curso {Curso}: {Mensaje}",
                            curso.MostrarCurso(), mensaje);
                    }
                }
            }
        }
    }
}
