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

        /// <summary>
        /// Relación de este tutor con este alumno en particular. Va acá y no en el
        /// tutor porque el mismo adulto puede ser padre de uno de los chicos y tío
        /// de otro, y antes se mostraba el mismo parentesco para todos.
        /// </summary>
        public string Parentesco { get; private set; } = string.Empty;

        private Autorizacion() { }

        public Autorizacion(int idAuto, int idAlu, int idTut, string parentesco = "")
        {
            IdAutorizacion = idAuto;
            AlumnoId = idAlu;
            TutorId = idTut;
            SetParentesco(parentesco);
        }

        public Autorizacion(int idAlu, int idTut, string parentesco = "")
        {
            AlumnoId = idAlu;
            TutorId = idTut;
            SetParentesco(parentesco);
        }

        public void SetParentesco(string parentesco)
        {
            Parentesco = (parentesco ?? string.Empty).Trim();
        }
    }
}
