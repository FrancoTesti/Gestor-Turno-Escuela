using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository _tutorRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TutorService(ITutorRepository tutorRepository, IUsuarioRepository usuarioRepository)
        {
            _tutorRepository = tutorRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<TutorDTO>> GetAllAsync()
        {
            var tutores = await _tutorRepository.GetAllAsync();
            return tutores.Select(MapToDTO).ToList();
        }

        public async Task<TutorDTO?> GetAsync(int id)
        {
            var tutor = await _tutorRepository.GetAsync(id);
            return tutor is null ? null : MapToDTO(tutor);
        }

        public async Task<TutorDTO> AddAsync(TutorDTO dto)
        {
            if (await _tutorRepository.DniExisteAsync(dto.Dni))
                throw new ArgumentException("Ya existe un tutor registrado con ese DNI.");

            if (string.IsNullOrWhiteSpace(dto.NombreUsuario))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            if (await _usuarioRepository.NombreUsuarioExisteAsync(dto.NombreUsuario))
                throw new ArgumentException("Ese nombre de usuario ya está en uso. Ingrese otro.");

            var usuario = new Usuario(dto.NombreUsuario, dto.Contrasena);
            var tutor = new Tutor(dto.Nombre, dto.Apellido, dto.Dni, dto.Parentesco, dto.Telefono, usuario);
            tutor.SetTieneRestriccion(dto.TieneRestriccion);

            await _usuarioRepository.AddAsync(usuario);
            await _tutorRepository.AddAsync(tutor);

            return MapToDTO(tutor);
        }

        public async Task<bool> UpdateAsync(TutorDTO dto)
        {
            var existente = await _tutorRepository.GetAsync(dto.IdTutor);
            if (existente is null)
                return false;

            if (await _tutorRepository.DniExisteAsync(dto.Dni, excludeId: dto.IdTutor))
                throw new ArgumentException("Ya existe otro tutor registrado con ese DNI.");

            existente.SetNombre(dto.Nombre);
            existente.SetApellido(dto.Apellido);
            existente.SetDni(dto.Dni);
            existente.SetParentesco(dto.Parentesco);
            existente.SetTelefono(dto.Telefono);
            existente.SetTieneRestriccion(dto.TieneRestriccion);

            return await _tutorRepository.UpdateAsync(existente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tutor = await _tutorRepository.GetAsync(id);
            if (tutor is null)
                return false;

            bool eliminado = await _tutorRepository.DeleteAsync(id);

            if (eliminado && tutor.Usuario is not null)
                await _usuarioRepository.DeleteAsync(tutor.Usuario.IdUsuario);

            return eliminado;
        }

        private static TutorDTO MapToDTO(Tutor tutor) => new()
        {
            IdTutor = tutor.IdTutor,
            Nombre = tutor.Nombre,
            Apellido = tutor.Apellido,
            Dni = tutor.Dni,
            Parentesco = tutor.Parentesco,
            Telefono = tutor.Telefono,
            TieneRestriccion = tutor.TieneRestriccion,
            NombreUsuario = tutor.Usuario?.NombreUsuario ?? string.Empty
        };
    }
}
