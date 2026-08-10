using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
namespace GTE.Dominio
{
    public class Alumno
    {
        private static string[] EstadosValidos = { "Presente", "Retirado", "Ausente" };
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public int IdAlumno { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Grado { get; private set; }
        public string Curso { get; private set; }
        public string Estado { get; private set; }
<<<<<<< HEAD
        // Constructor privado sin parámetros requerido por Entity Framework para la materialización de entidades
        private Alumno() { }
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public Alumno(int id, string nombre, string apellido, string grado, string curso)
        {
            SetIdAlumno(id);
            SetNombre(nombre);
            SetApellido(apellido);
            SetGrado(grado);
            SetCurso(curso);
            SetEstado("Presente");
        }
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public void SetIdAlumno(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual a 0.", nameof(id));
            IdAlumno = id;
        }
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre;
        }
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            Apellido = apellido;
        }
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public void SetGrado(string grado)
        {
            if (string.IsNullOrWhiteSpace(grado))
                throw new ArgumentException("El grado no puede ser nulo o vacío.", nameof(grado));
            Grado = grado;
        }
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public void SetCurso(string curso)
        {
            if (string.IsNullOrWhiteSpace(curso))
                throw new ArgumentException("El curso no puede ser nulo o vacío.", nameof(curso));
            Curso = curso;
        }
<<<<<<< HEAD
=======

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public void SetEstado(string estado)
        {
            if (!EstadosValidos.Contains(estado))
                throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", EstadosValidos)}.", nameof(estado));
            Estado = estado;
        }
    }
}