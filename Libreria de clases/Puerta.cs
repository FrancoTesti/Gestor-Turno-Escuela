using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Puerta
    {
        public int Id { get; set; }
        public string Ubicacion { get; set; }
        public string Nivel { get; set; }

        public Puerta(int id, string ubicacion, string nivel)
        {
            Id = id;
            Ubicacion = ubicacion;
            Nivel = nivel;
        }
    }
}
