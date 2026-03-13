using order_api.Domain.Entities;

namespace order_api.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByIdAsync(string id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(string id, Order order);
        Task DeleteOrderByIdAsync(string id);
    }
}
