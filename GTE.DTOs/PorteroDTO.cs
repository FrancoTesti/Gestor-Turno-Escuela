using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class PorteroDTO
    {
        public int IdPersonal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string PuertaAsignada { get; set; } = string.Empty;
    }
}
