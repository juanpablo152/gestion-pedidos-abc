using System.Net;
using System.Net.Http.Json;
using order_api.Application.DTO;
using order_api.Application.Interfaces;

namespace order_api.Infrastructure.Http
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfoDto?> GetUserByIdAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/{userId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<UserInfoDto>();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch
            {
                throw new ApplicationException("No se pudo contactar al servicio de usuarios. Intente más tarde.");
            }
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/users");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return [];

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<UserInfoDto>>() ?? [];
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch
            {
                throw new ApplicationException("No se pudo contactar al servicio de usuarios. Intente más tarde.");
            }
        }
    }
}
