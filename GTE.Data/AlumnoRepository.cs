using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTE.Dominio; 
namespace GTE.Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private static readonly List<Alumno> _alumnos = new List<Alumno>();
        private static int _nextId = 1;

        public Task AddAsync(Alumno alumno)
        {
            alumno.AsignarIdGenerado(_nextId++);
            _alumnos.Add(alumno);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Alumno>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Alumno>>(_alumnos.ToList());
        }
    }
}

