using System;
using System.Collections.Generic;

namespace GTE.DTOs
{
    public class RetiroDTO
    {
        public int IdRetiro { get; set; }
        public int IdTutor { get; set; }
        public string? TutorNombreCompleto { get; set; }
        public int IdPersonal { get; set; }
        public string? PersonalNombre { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public string Observaciones { get; set; } = string.Empty;
        public List<DetalleRetiroDTO> Detalles { get; set; } = new List<DetalleRetiroDTO>();
    }
}
