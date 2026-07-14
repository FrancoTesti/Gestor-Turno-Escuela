using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Secretario : Personal
    {
        public int NivelAccesoSistema { get; private set; }

        public Secretario(string nombre, int nivelAcceso, Usuario usuario) : base(nombre, usuario)
        {
            SetNivelAccesoSistema(nivelAcceso);
        }

        public void SetNivelAccesoSistema(int nivelAcceso)
        {
            if (nivelAcceso <= 0)
                throw new ArgumentException("El nivel de acceso debe ser mayor que 0.", nameof(nivelAcceso));
            NivelAccesoSistema = nivelAcceso;
        }

        public void CargarNuevaAutorizacion()
        {
            Console.WriteLine($"{Nombre} está cargando un nuevo permiso en el sistema.");
        }
    }
}