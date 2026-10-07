using System;
using System.Collections.Generic;

namespace GTE.DTOs
{
    public class CarteleraDTO
    {
        public List<CursoEnSalidaDTO> EnSalida { get; set; } = new List<CursoEnSalidaDTO>();

        public CursoEscolarDTO? Proximo { get; set; }
    }

    public class CursoEnSalidaDTO
    {
        public string Grado { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public string Puerta { get; set; } = string.Empty;
        public DateTime FechaHoraInicio { get; set; }
        public int CantidadDeAlumnos { get; set; }
    }
}
