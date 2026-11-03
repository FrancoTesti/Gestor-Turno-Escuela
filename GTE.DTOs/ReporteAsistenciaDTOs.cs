using System;
using System.Collections.Generic;

namespace GTE.DTOs
{
    public class ReporteNovedadItemDTO
    {
        public DateTime Fecha { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public int IdAlumno { get; set; }
        public string AlumnoNombreCompleto { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public string TipoInasistencia { get; set; } = string.Empty;
        public decimal ValorFalta { get; set; }
        public bool Justificada { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public bool NoComputa { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }

    public class ReporteEspejoSemanaDTO
    {
        public int IdCurso { get; set; }
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public DateTime FechaLunes { get; set; }
        public DateTime FechaViernes { get; set; }
        public List<FilaEspejoAlumnoDTO> Filas { get; set; } = new();
    }

    public class FilaEspejoAlumnoDTO
    {
        public int IdAlumno { get; set; }
        public string Alumno { get; set; } = string.Empty;
        public string Lunes { get; set; } = "P";
        public string Martes { get; set; } = "P";
        public string Miercoles { get; set; } = "P";
        public string Jueves { get; set; } = "P";
        public string Viernes { get; set; } = "P";
        public decimal TotalFaltasPeriodo { get; set; }
    }

    public class HistorialAsistenciaAlumnoDTO
    {
        public int IdAlumno { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public decimal TotalFaltasComputables { get; set; }
        public decimal TotalFaltasJustificadas { get; set; }
        public decimal TotalFaltasInjustificadas { get; set; }
        public List<InasistenciaAlumnoDTO> Inasistencias { get; set; } = new();
    }
}
