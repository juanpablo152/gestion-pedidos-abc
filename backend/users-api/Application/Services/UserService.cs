using users_api.Application.DTO;
using users_api.Application.Interfaces;
using users_api.Domain.Entities;

namespace users_api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _repository.GetAllUsersAsync();
                return users.Select(u => new UserResponseDto(u.Id!, u.Name, u.Email, u.Address));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los usuarios.", ex);
            }
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(id);
                if (user is null) return null;
                return new UserResponseDto(user.Id!, user.Name, user.Email, user.Address);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener el usuario con id.", ex);
            }
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUpdateUserDto dto)
        {
            try
            {
                var entity = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Address = dto.Address,
                    CreatedAt = DateTime.UtcNow
                };
                await _repository.CreateUserAsync(entity);
                return new UserResponseDto(entity.Id!, entity.Name, entity.Email, entity.Address);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al crear el usuario.", ex);
            }
        }

        public async Task<UserResponseDto> UpdateUserAsync(string id, CreateUpdateUserDto dto)
        {
            try
            {
                var existing = await _repository.GetUserByIdAsync(id);
                if (existing is null)
                    throw new KeyNotFoundException($"Usuario no encontrado.");

                existing.Name = dto.Name;
                existing.Email = dto.Email;
                existing.Address = dto.Address;

                await _repository.UpdateUserAsync(id, existing);
                return new UserResponseDto(existing.Id!, existing.Name, existing.Email, existing.Address);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            try
            {
                var existing = await _repository.GetUserByIdAsync(id);
                if (existing is null) return false;

                await _repository.DeleteUserByIdAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar el usuario.", ex);
            }
        }
    }
}
