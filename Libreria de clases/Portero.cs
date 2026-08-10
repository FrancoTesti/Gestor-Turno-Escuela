<<<<<<< HEAD
    using System;
=======
﻿using System;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class Portero : Personal
    {
        public string PuertaAsignada { get; private set; }

<<<<<<< HEAD
        private Portero() : base() { }

=======
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        public Portero(string nombre, string puerta, Usuario usuario) : base(nombre, usuario)
        {
            SetPuertaAsignada(puerta);
        }

        public void SetPuertaAsignada(string puerta)
        {
            if (string.IsNullOrWhiteSpace(puerta))
                throw new ArgumentException("La puerta asignada no puede ser nula o vacía.", nameof(puerta));
            PuertaAsignada = puerta;
        }

        public void RegistrarSalidaFisica()
        {
            Console.WriteLine($"{Nombre} está registrando la salida en la puerta {PuertaAsignada}");
        }
    }
}
