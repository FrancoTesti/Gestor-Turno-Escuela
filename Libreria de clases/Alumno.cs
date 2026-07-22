using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Alumno
    {
        private static string[] EstadosValidos = { "Presente", "Retirado", "Ausente" };

        public int IdAlumno { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Grado { get; private set; }
        public string Curso { get; private set; }
        public string Estado { get; private set; }

        public Alumno(int id, string nombre, string apellido, string grado, string curso)
        {
            SetIdAlumno(id);
            SetNombre(nombre);
            SetApellido(apellido);
            SetGrado(grado);
            SetCurso(curso);
            SetEstado("Presente");
        }

        public void SetIdAlumno(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual a 0.", nameof(id));
            IdAlumno = id;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre;
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            Apellido = apellido;
        }

        public void SetGrado(string grado)
        {
            if (string.IsNullOrWhiteSpace(grado))
                throw new ArgumentException("El grado no puede ser nulo o vacío.", nameof(grado));
            Grado = grado;
        }

        public void SetCurso(string curso)
        {
            if (string.IsNullOrWhiteSpace(curso))
                throw new ArgumentException("El curso no puede ser nulo o vacío.", nameof(curso));
            Curso = curso;
        }

        public void SetEstado(string estado)
        {
            if (!EstadosValidos.Contains(estado))
                throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", EstadosValidos)}.", nameof(estado));
            Estado = estado;
        }
    }
}