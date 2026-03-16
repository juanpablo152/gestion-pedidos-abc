using order_api.Application.DTO;

namespace order_api.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(string userId);
        Task<OrderResponseDto?> GetOrderByIdAsync(string id);
        Task<OrderResponseDto> CreateOrderAsync(CreateUpdateOrderDto order);
        Task<OrderResponseDto> UpdateOrderAsync(string id, CreateUpdateOrderDto order);
        Task<bool> DeleteOrderByIdAsync(string id);
        Task<IEnumerable<OrderCompleteResponseDto>> GetAllOrdersWithUserAsync();
        Task<OrderResponseDto> PatchOrderStatusAsync(string id, UpdateOrderStatusDto dto);
    }
}
