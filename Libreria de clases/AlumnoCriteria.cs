using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class AlumnoCriteria
    {
        public string? Nombre { get; }
        public string? Grado { get; }
        public string? Curso { get; }
        public string? Estado { get; }

        public AlumnoCriteria(string? nombre, string? grado, string? curso, string? estado)
        {
            Nombre = nombre;
            Grado = grado;
            Curso = curso;
            Estado = estado;
        }
    }
}
