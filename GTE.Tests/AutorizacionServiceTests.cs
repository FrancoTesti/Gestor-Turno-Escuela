using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;
using Xunit;

namespace GTE.Tests
{
    public class AutorizacionServiceTests
    {
        private class AutorizacionRepositoryMemoria : IAutorizacionRepository
        {
            private readonly List<Autorizacion> _autorizaciones = new();
            private readonly List<Alumno> _alumnos;
            private int _nextId = 1;

            public AutorizacionRepositoryMemoria(List<Alumno> alumnos)
            {
                _alumnos = alumnos;
            }

            public Task<IEnumerable<Autorizacion>> GetAllAsync() =>
                Task.FromResult<IEnumerable<Autorizacion>>(_autorizaciones);

            public Task<Autorizacion?> GetByIdAsync(int id) =>
                Task.FromResult(_autorizaciones.FirstOrDefault(a => a.IdAutorizacion == id));

            public Task<IEnumerable<Autorizacion>> GetByTutorIdAsync(int tutorId) =>
                Task.FromResult<IEnumerable<Autorizacion>>(_autorizaciones.Where(a => a.TutorId == tutorId));

            public Task<IEnumerable<Alumno>> GetAlumnosByTutorIdAsync(int tutorId)
            {
                var alumnoIds = _autorizaciones.Where(a => a.TutorId == tutorId).Select(a => a.AlumnoId).ToList();
                return Task.FromResult<IEnumerable<Alumno>>(_alumnos.Where(al => alumnoIds.Contains(al.IdAlumno)));
            }

            public Task<Autorizacion?> GetByTutorAndAlumnoAsync(int tutorId, int alumnoId) =>
                Task.FromResult(_autorizaciones.FirstOrDefault(a => a.TutorId == tutorId && a.AlumnoId == alumnoId));

            public Task<bool> EstaAutorizadoAsync(int tutorId, int alumnoId) =>
                Task.FromResult(_autorizaciones.Any(a => a.TutorId == tutorId && a.AlumnoId == alumnoId));

            public Task AddAsync(Autorizacion autorizacion)
            {
                autorizacion.IdAutorizacion = _nextId++;
                _autorizaciones.Add(autorizacion);
                return Task.CompletedTask;
            }

            public Task<bool> DeleteAsync(int id)
            {
                var item = _autorizaciones.FirstOrDefault(a => a.IdAutorizacion == id);
                if (item == null) return Task.FromResult(false);
                _autorizaciones.Remove(item);
                return Task.FromResult(true);
            }

            public Task<bool> DeleteByTutorAndAlumnoAsync(int tutorId, int alumnoId)
            {
                var item = _autorizaciones.FirstOrDefault(a => a.TutorId == tutorId && a.AlumnoId == alumnoId);
                if (item == null) return Task.FromResult(false);
                _autorizaciones.Remove(item);
                return Task.FromResult(true);
            }
        }

        private class TutorRepositoryMemoria : ITutorRepository
        {
            private readonly List<Tutor> _tutores = new();

            public void Seed(Tutor t) => _tutores.Add(t);

            public Task AddAsync(Tutor tutor) { _tutores.Add(tutor); return Task.CompletedTask; }
            public Task<bool> DeleteAsync(int id) => Task.FromResult(_tutores.RemoveAll(t => t.IdTutor == id) > 0);
            public Task<Tutor?> GetAsync(int id) => Task.FromResult(_tutores.FirstOrDefault(t => t.IdTutor == id));
            public Task<IEnumerable<Tutor>> GetAllAsync() => Task.FromResult<IEnumerable<Tutor>>(_tutores);
            public Task<bool> UpdateAsync(Tutor tutor) => Task.FromResult(true);
            public Task<bool> DniExisteAsync(string dni, int? excludeId = null) => Task.FromResult(_tutores.Any(t => t.Dni == dni && (!excludeId.HasValue || t.IdTutor != excludeId.Value)));
        }

        private class AlumnoRepositoryMemoria : IAlumnoRepository
        {
            private readonly List<Alumno> _alumnos;

            public AlumnoRepositoryMemoria(List<Alumno> alumnos)
            {
                _alumnos = alumnos;
            }

            public Task<Alumno?> GetAsync(int id) => Task.FromResult(_alumnos.FirstOrDefault(a => a.IdAlumno == id));
            public Task<IEnumerable<Alumno>> GetAllAsync() => Task.FromResult<IEnumerable<Alumno>>(_alumnos);
            public Task AddAsync(Alumno alumno) { _alumnos.Add(alumno); return Task.CompletedTask; }
            public Task<bool> UpdateAsync(Alumno alumno) => Task.FromResult(true);
            public Task<bool> DeleteAsync(int id) => Task.FromResult(_alumnos.RemoveAll(a => a.IdAlumno == id) > 0);
            public Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria) => Task.FromResult<IEnumerable<Alumno>>(_alumnos);
        }

        [Fact]
        public async Task NoSePuedeAutorizarDosVecesAlMismoTutorParaElMismoAlumno()
        {
            var curso = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(12, 0, 0));
            var alumno = new Alumno(1, "Juan", "Perez", 1);
            alumno.SetCursoEscolar(curso);
            var tutor = new Tutor("Carlos", "Perez", "30111222", "Padre", "1122334455", new Usuario("cperez", "tutor123", true));
            tutor.AsignarIdGenerado(1);

            var alumnos = new List<Alumno> { alumno };
            var autoRepo = new AutorizacionRepositoryMemoria(alumnos);
            var tutorRepo = new TutorRepositoryMemoria();
            tutorRepo.Seed(tutor);
            var alumnoRepo = new AlumnoRepositoryMemoria(alumnos);

            var service = new AutorizacionService(autoRepo, tutorRepo, alumnoRepo);

            var primera = await service.AddAsync(new AutorizacionDTO { TutorId = 1, AlumnoId = 1 });
            Assert.True(primera.Exito);

            var segunda = await service.AddAsync(new AutorizacionDTO { TutorId = 1, AlumnoId = 1 });

            Assert.False(segunda.Exito);
            Assert.Contains("ya se encuentra autorizado", segunda.Mensaje, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task RechazaAutorizacionSiTutorNoExiste()
        {
            var curso = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(12, 0, 0));
            var alumno = new Alumno(1, "Juan", "Perez", 1);
            alumno.SetCursoEscolar(curso);
            var alumnos = new List<Alumno> { alumno };

            var autoRepo = new AutorizacionRepositoryMemoria(alumnos);
            var tutorRepo = new TutorRepositoryMemoria();
            var alumnoRepo = new AlumnoRepositoryMemoria(alumnos);

            var service = new AutorizacionService(autoRepo, tutorRepo, alumnoRepo);

            var resultado = await service.AddAsync(new AutorizacionDTO { TutorId = 999, AlumnoId = 1 });

            Assert.False(resultado.Exito);
            Assert.Contains("tutor", resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task RechazaAutorizacionSiAlumnoNoExiste()
        {
            var tutor = new Tutor("Carlos", "Perez", "30111222", "Padre", "1122334455", new Usuario("cperez", "tutor123", true));
            tutor.AsignarIdGenerado(1);
            var alumnos = new List<Alumno>();

            var autoRepo = new AutorizacionRepositoryMemoria(alumnos);
            var tutorRepo = new TutorRepositoryMemoria();
            tutorRepo.Seed(tutor);
            var alumnoRepo = new AlumnoRepositoryMemoria(alumnos);

            var service = new AutorizacionService(autoRepo, tutorRepo, alumnoRepo);

            var resultado = await service.AddAsync(new AutorizacionDTO { TutorId = 1, AlumnoId = 999 });

            Assert.False(resultado.Exito);
            Assert.Contains("alumno", resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task EliminarAutorizacionExitosa()
        {
            var curso = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(12, 0, 0));
            var alumno = new Alumno(1, "Juan", "Perez", 1);
            alumno.SetCursoEscolar(curso);
            var tutor = new Tutor("Carlos", "Perez", "30111222", "Padre", "1122334455", new Usuario("cperez", "tutor123", true));
            tutor.AsignarIdGenerado(1);

            var alumnos = new List<Alumno> { alumno };
            var autoRepo = new AutorizacionRepositoryMemoria(alumnos);
            var tutorRepo = new TutorRepositoryMemoria();
            tutorRepo.Seed(tutor);
            var alumnoRepo = new AlumnoRepositoryMemoria(alumnos);

            var service = new AutorizacionService(autoRepo, tutorRepo, alumnoRepo);
            var add = await service.AddAsync(new AutorizacionDTO { TutorId = 1, AlumnoId = 1 });

            bool deleted = await service.DeleteAsync(add.Autorizacion!.IdAutorizacion);
            Assert.True(deleted);

            var lista = await service.GetByTutorIdAsync(1);
            Assert.Empty(lista);
        }
    }
}
