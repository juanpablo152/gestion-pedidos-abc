using users_api.Domain.Entities;

namespace users_api.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task DeleteUserByIdAsync(string id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(string id, User user);
    }
}
