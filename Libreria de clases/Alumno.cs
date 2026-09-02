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
        public int IdCurso { get; private set; }
        public CursoEscolar CursoEscolar { get; private set; } = null!;
        public string Estado { get; private set; }
        // Constructor privado sin parámetros requerido por Entity Framework para la materialización de entidades
        private Alumno() { }
        public Alumno(int id, string nombre, string apellido, int idCurso)
        {
            SetIdAlumno(id);
            SetNombre(nombre);
            SetApellido(apellido);
            SetCurso(idCurso);
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
        public void SetCurso(int idCurso)
        {
            if (idCurso <= 0)
                throw new ArgumentException("Debe seleccionar un curso válido.", nameof(idCurso));
            IdCurso = idCurso;
        }
        public void SetCursoEscolar(CursoEscolar cursoEscolar)
        {
            CursoEscolar = cursoEscolar ?? throw new ArgumentNullException(nameof(cursoEscolar));
            IdCurso = cursoEscolar.IdCurso;
        }
        public void SetEstado(string estado)
        {
            if (!EstadosValidos.Contains(estado))
                throw new ArgumentException($"Estado inválido. Valores permitidos: {string.Join(", ", EstadosValidos)}.", nameof(estado));
            Estado = estado;
        }
    }
}
