using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Alumno
    {
        private static readonly string[] EstadosValidos = { "Presente", "Retirado", "Ausente" };

        public int IdAlumno { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Grado { get; private set; }
        public string Curso { get; private set; }
        public string Estado { get; private set; }

        public Alumno(string nombre, string apellido, string grado, string curso)
        {
            SetNombre(nombre);
            SetApellido(apellido);
            SetGrado(grado);
            SetCurso(curso);
            SetEstado("Presente");
            IdAlumno = 0;
        }

        public void AsignarIdGenerado(int nuevoId)
        {
            if (IdAlumno != 0)
                throw new InvalidOperationException("El Id ya ha sido asignado y no puede ser modificado.");
            if (nuevoId <= 0)
                throw new ArgumentException("El Id generado debe ser mayor que 0.");

            IdAlumno = nuevoId;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.");
            Nombre = nombre;
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.");
            Apellido = apellido;
        }

        public void SetGrado(string grado)
        {
            if (string.IsNullOrWhiteSpace(grado))
                throw new ArgumentException("El grado no puede ser nulo o vacío.");
            Grado = grado;
        }

        public void SetCurso(string curso)
        {
            if (string.IsNullOrWhiteSpace(curso))
                throw new ArgumentException("El curso no puede ser nulo o vacío.");
            Curso = curso;
        }

        public void SetEstado(string estado)
        {
            if (!EstadosValidos.Contains(estado))
                throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", EstadosValidos)}.");
            Estado = estado;
        }
    }
}