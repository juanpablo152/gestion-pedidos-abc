using payment_api.Application.DTO;

namespace payment_api.Application.Interfaces
{
    public interface IOrderApiClient
    {
        Task<bool> OrderExistsAsync(string orderId);
        Task<IEnumerable<OrderWithUserDto>> GetAllOrdersWithUserAsync();
    }
}
