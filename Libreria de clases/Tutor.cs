using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GTE.Dominio
{
    public class Tutor
    {
        public int IdTutor { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Dni { get; private set; }
        public string Parentesco { get; private set; }
        public string Telefono { get; private set; }
        public bool TieneRestriccion { get; private set; }
        public Usuario Usuario { get; private set; }

        public Tutor(string nombre, string apellido, string dni, string parentesco, string telefono, Usuario usuario)
        {
            SetNombre(nombre);
            SetApellido(apellido);
            SetDni(dni);
            SetParentesco(parentesco);
            SetTelefono(telefono);
            SetUsuario(usuario);
            TieneRestriccion = false;
            IdTutor = 0;
        }

        public void AsignarIdGenerado(int nuevoId)
        {
            if (IdTutor != 0)
                throw new InvalidOperationException("El Id ya ha sido asignado y no puede ser modificado.");
            if (nuevoId <= 0)
                throw new ArgumentException("El Id generado debe ser mayor que 0.");

            IdTutor = nuevoId;
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

        public void SetDni(string dni)
        {
            if (!Regex.IsMatch(dni ?? string.Empty, @"^\d{7,8}$"))
                throw new ArgumentException("El DNI debe tener entre 7 y 8 dígitos numéricos.");
            Dni = dni!;
        }

        public void SetParentesco(string parentesco)
        {
            if (string.IsNullOrWhiteSpace(parentesco))
                throw new ArgumentException("El parentesco no puede ser nulo o vacío.");
            Parentesco = parentesco;
        }

        public void SetTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono no puede ser nulo o vacío.");
            Telefono = telefono;
        }

        public void SetTieneRestriccion(bool tieneRestriccion)
        {
            TieneRestriccion = tieneRestriccion;
        }

        public void SetUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            Usuario = usuario;
        }
    }
}