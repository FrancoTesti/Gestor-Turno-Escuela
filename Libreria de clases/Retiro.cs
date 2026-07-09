using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Retiro
    {
        public int IdRetiro { get; set; }
        public int IdAlumno { get; set; }
        public int IdTutor { get; set; }
        public int IdPersonal { get; set; } // quién entregó al niño
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public string Observaciones { get; set; } = string.Empty; // para registrar cualquier detalle adicional

        public Retiro(int idRet, int idAlu, int idTut, int idPer, DateTime fechayhora, string observ)
        {
            IdRetiro = idRet;
            IdAlumno = idAlu;
            IdTutor = idTut;
            IdPersonal = idPer;
            FechaHora = fechayhora;
            Observaciones = observ;
        }
    }
}
