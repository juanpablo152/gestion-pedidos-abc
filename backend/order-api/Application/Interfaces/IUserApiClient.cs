using order_api.Application.DTO;

namespace order_api.Application.Interfaces
{
    public interface IUserApiClient
    {
        Task<UserInfoDto?> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
    }
}
