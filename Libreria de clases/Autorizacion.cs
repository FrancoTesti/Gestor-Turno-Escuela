using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Autorizacion
    {
        public int IdAutorizacion { get; set; }
        public int AlumnoId { get; set; }
        public int TutorId { get; set; }

        public Autorizacion(int idAuto, int idAlu, int idTut)
        {
            IdAutorizacion = idAuto;
            AlumnoId = idAlu;
            TutorId = idTut;
        }
    }
}
