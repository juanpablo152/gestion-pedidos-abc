using users_api.Application.DTO;
using users_api.Domain.Entities;

namespace users_api.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(string id);
        Task<bool> DeleteUserByIdAsync(string id);
        Task<UserResponseDto> CreateUserAsync(CreateUpdateUserDto user);
        Task<UserResponseDto> UpdateUserAsync(string id, CreateUpdateUserDto user);
    }
}
