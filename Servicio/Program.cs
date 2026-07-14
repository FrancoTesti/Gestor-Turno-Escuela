using System;
using System.Collections.Generic;
using GTE.Dominio;

namespace GTE.Application.Services
{
    public class ValidarRetiro
    {
        public bool ValidarPermiso(
            Alumno alumno,
            Tutor tutor,
            List<Autorizacion> autorizaciones)
        {   
            bool existeVinculo = false;

            // verificar autorización
            foreach (var aut in autorizaciones)
            {
                if (aut.AlumnoId == alumno.IdAlumno &&
                    aut.TutorId == tutor.IdTutor)
                {
                    existeVinculo = true;
                    break;
                }
            }

            // si no existe autorización
            if (!existeVinculo)
                return false;

            // validar restricciones legales
            if (tutor.TieneRestriccion)
                return false;

            // validar estado del alumno
            if (alumno.Estado != "Presente")
                return false;

            return true;
        }

        // registrar el retiro
        public Retiro RegistrarEvento(
            int idAlu,
            int idTutor,
            int idPers,
            string obs = "")
        {
            return new Retiro(
                0,
                idAlu,
                idTutor,
                idPers,
                DateTime.Now,
                obs
            );
        }
    }
}

