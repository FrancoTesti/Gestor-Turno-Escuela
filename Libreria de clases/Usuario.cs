using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Usuario
    {
        public int IdUsuario { get; private set; }
        public string NombreUsuario { get; private set; }
        public string Contrasena { get; private set; }
        public bool EstaActivo { get; private set; }


        public Usuario(string nombreUsuario, string contrasena, bool activo = true)
        {
            SetNombreUsuario(nombreUsuario);
            SetContrasena(contrasena);
            EstaActivo = activo;
            IdUsuario = 0;
        }

        public void AsignarIdGenerado(int nuevoId)
        {
            if (IdUsuario != 0)
                throw new InvalidOperationException("El Id ya ha sido asignado y no puede ser modificado.");
            if (nuevoId <= 0)
                throw new ArgumentException("El Id generado debe ser mayor que 0.", nameof(nuevoId));

            IdUsuario = nuevoId;
        }
        public void SetNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.", nameof(nombreUsuario));
            NombreUsuario = nombreUsuario;
        }

        public void SetContrasena(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena) || contrasena.Length < 4)
                throw new ArgumentException("La contraseña debe tener al menos 4 caracteres.", nameof(contrasena));
            Contrasena = contrasena;
        }

        public void SetEstaActivo(bool activo)
        {
            EstaActivo = activo;
        }

        public bool ValidarCredenciales(string usuario, string pass)
        {
            return NombreUsuario == usuario && Contrasena == pass && EstaActivo;
        }
    }
}