using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class SecretarioDTO
    {
        public int IdPersonal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int NivelAccesoSistema { get; set; }
    }
}
