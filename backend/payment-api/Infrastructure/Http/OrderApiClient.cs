using System.Net;
using System.Net.Http.Json;
using payment_api.Application.DTO;
using payment_api.Application.Interfaces;

namespace payment_api.Infrastructure.Http
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly HttpClient _httpClient;

        public OrderApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> OrderExistsAsync(string orderId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/orders/{orderId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw new ApplicationException("No se pudo contactar al servicio de órdenes. Intente más tarde.");
            }
        }

        public async Task<IEnumerable<OrderWithUserDto>> GetAllOrdersWithUserAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/orders/with-user");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return [];

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrderWithUserDto>>() ?? [];
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch
            {
                throw new ApplicationException("No se pudo contactar al servicio de órdenes. Intente más tarde.");
            }
        }
    }
}
