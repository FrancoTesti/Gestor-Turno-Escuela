using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public abstract class Personal
    {
        public int IdPersonal { get; private set; }
        public string Nombre { get; private set; }
        public Usuario Usuario { get; private set; }

        protected Personal(string nombre, Usuario usuario)
        {
            SetNombre(nombre);
            SetUsuario(usuario);
            IdPersonal = 0;
        }

        public void AsignarIdGenerado(int nuevoId)
        {
            if (IdPersonal != 0)
                throw new InvalidOperationException("El Id ya ha sido asignado y no puede ser modificado.");
            if (nuevoId <= 0)
                throw new ArgumentException("El Id generado debe ser mayor que 0.", nameof(nuevoId));

            IdPersonal = nuevoId;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre;
        }

        public void SetUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            Usuario = usuario;
        }
    }
}