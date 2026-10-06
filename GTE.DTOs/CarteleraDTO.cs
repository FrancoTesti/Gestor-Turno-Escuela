using System;
using System.Collections.Generic;

namespace GTE.DTOs
{
    /// <summary>
    /// Lo que muestra la pantalla que está en la puerta del colegio.
    ///
    /// A propósito no lleva nombres de alumnos ni de personal: la pantalla la ve
    /// cualquiera que pase por la calle, así que solo se publica qué curso está
    /// saliendo. El detalle de cada alumno lo ve su tutor desde el celular.
    /// </summary>
    public class CarteleraDTO
    {
        public List<CursoEnSalidaDTO> EnSalida { get; set; } = new List<CursoEnSalidaDTO>();

        /// <summary>El próximo curso que sale, para avisar mientras no hay ninguno.</summary>
        public CursoEscolarDTO? Proximo { get; set; }
    }

    /// <summary>Un curso que está saliendo, sin ningún dato personal.</summary>
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
