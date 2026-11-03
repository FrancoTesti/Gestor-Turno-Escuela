using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;
using Moq;
using Xunit;

namespace GTE.Tests
{
    public class CursoEscolarServiceTests
    {
        private readonly Mock<ICursoEscolarRepository> _mockRepo;
        private readonly CursoEscolarService _service;

        public CursoEscolarServiceTests()
        {
            _mockRepo = new Mock<ICursoEscolarRepository>(MockBehavior.Strict);
            _service = new CursoEscolarService(_mockRepo.Object);
        }

        #region 1. GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_WhenCoursesExist_ReturnsAllMappedDtosWith5DailySchedules()
        {
            // Arrange
            var curso1 = new CursoEscolar(1, "1°", "A", "Mañana",
                new TimeSpan(12, 0, 0),
                new TimeSpan(12, 15, 0),
                new TimeSpan(12, 30, 0),
                new TimeSpan(12, 45, 0),
                new TimeSpan(11, 30, 0));

            var curso2 = new CursoEscolar(2, "2°", "B", "Tarde",
                new TimeSpan(17, 0, 0),
                new TimeSpan(17, 10, 0),
                new TimeSpan(17, 20, 0),
                new TimeSpan(17, 30, 0),
                new TimeSpan(16, 30, 0));

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CursoEscolar> { curso1, curso2 });

            // Act
            var result = (await _service.GetAllAsync()).ToList();

            // Assert
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Equal(2, result.Count);

            var dto1 = result[0];
            Assert.Equal(1, dto1.IdCurso);
            Assert.Equal("1°", dto1.Grado);
            Assert.Equal("A", dto1.Curso);
            Assert.Equal("Mañana", dto1.Turno);
            Assert.Equal(new TimeSpan(12, 0, 0), dto1.HorarioSalidaLunes);
            Assert.Equal(new TimeSpan(12, 15, 0), dto1.HorarioSalidaMartes);
            Assert.Equal(new TimeSpan(12, 30, 0), dto1.HorarioSalidaMiercoles);
            Assert.Equal(new TimeSpan(12, 45, 0), dto1.HorarioSalidaJueves);
            Assert.Equal(new TimeSpan(11, 30, 0), dto1.HorarioSalidaViernes);
            Assert.Equal(curso1.ObtenerHorarioSalida(DateTime.Today.DayOfWeek), dto1.HorarioSalida);

            var dto2 = result[1];
            Assert.Equal(2, dto2.IdCurso);
            Assert.Equal(new TimeSpan(17, 0, 0), dto2.HorarioSalidaLunes);
            Assert.Equal(new TimeSpan(17, 10, 0), dto2.HorarioSalidaMartes);
            Assert.Equal(new TimeSpan(17, 20, 0), dto2.HorarioSalidaMiercoles);
            Assert.Equal(new TimeSpan(17, 30, 0), dto2.HorarioSalidaJueves);
            Assert.Equal(new TimeSpan(16, 30, 0), dto2.HorarioSalidaViernes);
            Assert.Equal(curso2.ObtenerHorarioSalida(DateTime.Today.DayOfWeek), dto2.HorarioSalida);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoCoursesExist_ReturnsEmptyCollection()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<CursoEscolar>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Empty(result);
        }

        #endregion

        #region 2. GetAsync (GetById) Tests

        [Fact]
        public async Task GetAsync_WhenCourseExists_ReturnsMappedDtoWith5DailySchedules()
        {
            // Arrange
            var curso = new CursoEscolar(10, "3°", "C", "Tarde",
                new TimeSpan(17, 0, 0),
                new TimeSpan(17, 15, 0),
                new TimeSpan(17, 30, 0),
                new TimeSpan(17, 45, 0),
                new TimeSpan(16, 0, 0));

            _mockRepo.Setup(r => r.GetAsync(10)).ReturnsAsync(curso);

            // Act
            var dto = await _service.GetAsync(10);

            // Assert
            _mockRepo.Verify(r => r.GetAsync(10), Times.Once);
            Assert.NotNull(dto);
            Assert.Equal(10, dto.IdCurso);
            Assert.Equal("3°", dto.Grado);
            Assert.Equal("C", dto.Curso);
            Assert.Equal("Tarde", dto.Turno);
            Assert.Equal(new TimeSpan(17, 0, 0), dto.HorarioSalidaLunes);
            Assert.Equal(new TimeSpan(17, 15, 0), dto.HorarioSalidaMartes);
            Assert.Equal(new TimeSpan(17, 30, 0), dto.HorarioSalidaMiercoles);
            Assert.Equal(new TimeSpan(17, 45, 0), dto.HorarioSalidaJueves);
            Assert.Equal(new TimeSpan(16, 0, 0), dto.HorarioSalidaViernes);
            Assert.Equal(curso.ObtenerHorarioSalida(DateTime.Today.DayOfWeek), dto.HorarioSalida);
        }

        [Fact]
        public async Task GetAsync_WhenCourseDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAsync(999)).ReturnsAsync((CursoEscolar?)null);

            // Act
            var dto = await _service.GetAsync(999);

            // Assert
            _mockRepo.Verify(r => r.GetAsync(999), Times.Once);
            Assert.Null(dto);
        }

        #endregion

        #region 3. AddAsync (Create) Tests

        [Fact]
        public async Task AddAsync_WithDifferentiated5WeekdaySchedules_CreatesEntityAndCallsRepoAddAsync()
        {
            // Arrange
            var dto = new CursoEscolarDTO
            {
                Grado = "4°",
                Curso = "B",
                Turno = "Mañana",
                HorarioSalidaLunes = new TimeSpan(12, 0, 0),
                HorarioSalidaMartes = new TimeSpan(12, 10, 0),
                HorarioSalidaMiercoles = new TimeSpan(12, 20, 0),
                HorarioSalidaJueves = new TimeSpan(12, 30, 0),
                HorarioSalidaViernes = new TimeSpan(11, 45, 0)
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<CursoEscolar>()))
                     .Callback<CursoEscolar>(c => c.SetIdCurso(55))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.Is<CursoEscolar>(c =>
                c.Grado == "4°" &&
                c.Curso == "B" &&
                c.Turno == "Mañana" &&
                c.HorarioLunes == new TimeSpan(12, 0, 0) &&
                c.HorarioMartes == new TimeSpan(12, 10, 0) &&
                c.HorarioMiercoles == new TimeSpan(12, 20, 0) &&
                c.HorarioJueves == new TimeSpan(12, 30, 0) &&
                c.HorarioViernes == new TimeSpan(11, 45, 0)
            )), Times.Once);

            Assert.Equal(55, result.IdCurso);
            Assert.Equal(new TimeSpan(12, 0, 0), result.HorarioSalidaLunes);
            Assert.Equal(new TimeSpan(12, 10, 0), result.HorarioSalidaMartes);
            Assert.Equal(new TimeSpan(12, 20, 0), result.HorarioSalidaMiercoles);
            Assert.Equal(new TimeSpan(12, 30, 0), result.HorarioSalidaJueves);
            Assert.Equal(new TimeSpan(11, 45, 0), result.HorarioSalidaViernes);
            Assert.Equal(result.ObtenerHorarioPorDia(DateTime.Today.DayOfWeek), result.HorarioSalida);
        }

        [Fact]
        public async Task AddAsync_WhenWeekdaySchedulesAreZero_FallsBackToHorarioSalida()
        {
            // Arrange
            var dto = new CursoEscolarDTO
            {
                Grado = "5°",
                Curso = "A",
                Turno = "Tarde",
                HorarioSalida = new TimeSpan(17, 30, 0),
                HorarioSalidaLunes = TimeSpan.Zero,
                HorarioSalidaMartes = TimeSpan.Zero,
                HorarioSalidaMiercoles = TimeSpan.Zero,
                HorarioSalidaJueves = TimeSpan.Zero,
                HorarioSalidaViernes = TimeSpan.Zero
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<CursoEscolar>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.Is<CursoEscolar>(c =>
                c.HorarioLunes == new TimeSpan(17, 30, 0) &&
                c.HorarioMartes == new TimeSpan(17, 30, 0) &&
                c.HorarioMiercoles == new TimeSpan(17, 30, 0) &&
                c.HorarioJueves == new TimeSpan(17, 30, 0) &&
                c.HorarioViernes == new TimeSpan(17, 30, 0)
            )), Times.Once);

            Assert.Equal(new TimeSpan(17, 30, 0), result.HorarioSalidaLunes);
            Assert.Equal(new TimeSpan(17, 30, 0), result.HorarioSalidaMartes);
            Assert.Equal(new TimeSpan(17, 30, 0), result.HorarioSalidaMiercoles);
            Assert.Equal(new TimeSpan(17, 30, 0), result.HorarioSalidaJueves);
            Assert.Equal(new TimeSpan(17, 30, 0), result.HorarioSalidaViernes);
        }

        [Fact]
        public async Task AddAsync_WhenSomeWeekdaySchedulesAreZero_FallsBackIndividuallyToHorarioSalida()
        {
            // Arrange
            var dto = new CursoEscolarDTO
            {
                Grado = "1°",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalida = new TimeSpan(12, 0, 0),
                HorarioSalidaLunes = new TimeSpan(12, 45, 0),
                HorarioSalidaMartes = TimeSpan.Zero, // fallback to 12:00
                HorarioSalidaMiercoles = new TimeSpan(13, 0, 0),
                HorarioSalidaJueves = TimeSpan.Zero, // fallback to 12:00
                HorarioSalidaViernes = TimeSpan.Zero  // fallback to 12:00
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<CursoEscolar>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.Is<CursoEscolar>(c =>
                c.HorarioLunes == new TimeSpan(12, 45, 0) &&
                c.HorarioMartes == new TimeSpan(12, 0, 0) &&
                c.HorarioMiercoles == new TimeSpan(13, 0, 0) &&
                c.HorarioJueves == new TimeSpan(12, 0, 0) &&
                c.HorarioViernes == new TimeSpan(12, 0, 0)
            )), Times.Once);
        }

        #endregion

        #region 4. UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_WhenCourseExists_UpdatesAll5WeekdaySchedulesAndCallsRepoUpdateAsync()
        {
            // Arrange
            var existing = new CursoEscolar(25, "2°", "A", "Mañana", new TimeSpan(12, 0, 0));

            _mockRepo.Setup(r => r.GetAsync(25)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<CursoEscolar>())).ReturnsAsync(true);

            var updateDto = new CursoEscolarDTO
            {
                IdCurso = 25,
                Grado = "2°",
                Curso = "B",
                Turno = "Tarde",
                HorarioSalidaLunes = new TimeSpan(17, 0, 0),
                HorarioSalidaMartes = new TimeSpan(17, 15, 0),
                HorarioSalidaMiercoles = new TimeSpan(17, 30, 0),
                HorarioSalidaJueves = new TimeSpan(17, 45, 0),
                HorarioSalidaViernes = new TimeSpan(16, 30, 0)
            };

            // Act
            var success = await _service.UpdateAsync(updateDto);

            // Assert
            Assert.True(success);
            _mockRepo.Verify(r => r.GetAsync(25), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<CursoEscolar>(c =>
                c.IdCurso == 25 &&
                c.Grado == "2°" &&
                c.Curso == "B" &&
                c.Turno == "Tarde" &&
                c.HorarioLunes == new TimeSpan(17, 0, 0) &&
                c.HorarioMartes == new TimeSpan(17, 15, 0) &&
                c.HorarioMiercoles == new TimeSpan(17, 30, 0) &&
                c.HorarioJueves == new TimeSpan(17, 45, 0) &&
                c.HorarioViernes == new TimeSpan(16, 30, 0) &&
                c.HorarioSalida == new TimeSpan(17, 0, 0)
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenCourseDoesNotExist_ReturnsFalseAndNeverCallsRepoUpdate()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAsync(404)).ReturnsAsync((CursoEscolar?)null);

            var updateDto = new CursoEscolarDTO
            {
                IdCurso = 404,
                Grado = "1°",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalida = new TimeSpan(12, 0, 0)
            };

            // Act
            var success = await _service.UpdateAsync(updateDto);

            // Assert
            Assert.False(success);
            _mockRepo.Verify(r => r.GetAsync(404), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<CursoEscolar>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenWeekdaySchedulesAreZero_FallsBackToHorarioSalida()
        {
            // Arrange
            var existing = new CursoEscolar(30, "3°", "A", "Mañana", new TimeSpan(11, 0, 0));

            _mockRepo.Setup(r => r.GetAsync(30)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<CursoEscolar>())).ReturnsAsync(true);

            var updateDto = new CursoEscolarDTO
            {
                IdCurso = 30,
                Grado = "3°",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalida = new TimeSpan(12, 0, 0),
                HorarioSalidaLunes = TimeSpan.Zero,
                HorarioSalidaMartes = TimeSpan.Zero,
                HorarioSalidaMiercoles = TimeSpan.Zero,
                HorarioSalidaJueves = TimeSpan.Zero,
                HorarioSalidaViernes = TimeSpan.Zero
            };

            // Act
            var success = await _service.UpdateAsync(updateDto);

            // Assert
            Assert.True(success);
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<CursoEscolar>(c =>
                c.HorarioLunes == new TimeSpan(12, 0, 0) &&
                c.HorarioMartes == new TimeSpan(12, 0, 0) &&
                c.HorarioMiercoles == new TimeSpan(12, 0, 0) &&
                c.HorarioJueves == new TimeSpan(12, 0, 0) &&
                c.HorarioViernes == new TimeSpan(12, 0, 0)
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenRepoUpdateReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var existing = new CursoEscolar(31, "3°", "B", "Mañana", new TimeSpan(12, 0, 0));

            _mockRepo.Setup(r => r.GetAsync(31)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<CursoEscolar>())).ReturnsAsync(false);

            var updateDto = new CursoEscolarDTO
            {
                IdCurso = 31,
                Grado = "3°",
                Curso = "B",
                Turno = "Mañana",
                HorarioSalida = new TimeSpan(12, 0, 0)
            };

            // Act
            var success = await _service.UpdateAsync(updateDto);

            // Assert
            Assert.False(success);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<CursoEscolar>()), Times.Once);
        }

        #endregion

        #region 5. Validation Propagation & Repository Protection Tests

        [Fact]
        public async Task AddAsync_WhenNegativeScheduleProvided_ThrowsArgumentException_AndNeverCallsRepoAdd()
        {
            // Arrange
            var dto = new CursoEscolarDTO
            {
                Grado = "1°",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalidaLunes = TimeSpan.FromHours(-1),
                HorarioSalidaMartes = new TimeSpan(12, 0, 0),
                HorarioSalidaMiercoles = new TimeSpan(12, 0, 0),
                HorarioSalidaJueves = new TimeSpan(12, 0, 0),
                HorarioSalidaViernes = new TimeSpan(12, 0, 0)
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto));
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<CursoEscolar>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenScheduleGreaterOrEqualTo24HoursProvided_ThrowsArgumentException_AndNeverCallsRepoAdd()
        {
            // Arrange
            var dto = new CursoEscolarDTO
            {
                Grado = "1°",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalidaLunes = new TimeSpan(12, 0, 0),
                HorarioSalidaMartes = new TimeSpan(12, 0, 0),
                HorarioSalidaMiercoles = new TimeSpan(12, 0, 0),
                HorarioSalidaJueves = new TimeSpan(12, 0, 0),
                HorarioSalidaViernes = TimeSpan.FromHours(24)
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto));
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<CursoEscolar>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenInvalidTurnoProvided_ThrowsArgumentException_AndNeverCallsRepoAdd()
        {
            // Arrange
            var dto = new CursoEscolarDTO
            {
                Grado = "1°",
                Curso = "A",
                Turno = "Vespertino", // Invalid turno
                HorarioSalida = new TimeSpan(12, 0, 0)
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto));
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<CursoEscolar>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenEmptyGradoOrCursoProvided_ThrowsArgumentException_AndNeverCallsRepoAdd()
        {
            // Arrange
            var dtoInvalidGrado = new CursoEscolarDTO
            {
                Grado = "  ",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalida = new TimeSpan(12, 0, 0)
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dtoInvalidGrado));
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<CursoEscolar>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidScheduleProvided_ThrowsArgumentException_AndNeverCallsRepoUpdate()
        {
            // Arrange
            var existing = new CursoEscolar(15, "1°", "A", "Mañana", new TimeSpan(12, 0, 0));
            _mockRepo.Setup(r => r.GetAsync(15)).ReturnsAsync(existing);

            var updateDto = new CursoEscolarDTO
            {
                IdCurso = 15,
                Grado = "1°",
                Curso = "A",
                Turno = "Mañana",
                HorarioSalidaLunes = new TimeSpan(12, 0, 0),
                HorarioSalidaMartes = new TimeSpan(12, 0, 0),
                HorarioSalidaMiercoles = TimeSpan.FromHours(25), // Invalid schedule
                HorarioSalidaJueves = new TimeSpan(12, 0, 0),
                HorarioSalidaViernes = new TimeSpan(12, 0, 0)
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(updateDto));
            _mockRepo.Verify(r => r.GetAsync(15), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<CursoEscolar>()), Times.Never);
        }

        #endregion

        #region 6. DeleteAsync Delegation Tests

        [Fact]
        public async Task DeleteAsync_WhenCalled_DelegatesToRepositoryDeleteAsync()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(7);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(7), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenRepoReturnsFalse_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAsync(99);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.DeleteAsync(99), Times.Once);
        }

        #endregion
    }
}
