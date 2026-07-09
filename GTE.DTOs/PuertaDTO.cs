using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class PuertaDTO
    {
        public int Id { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
    }
}
