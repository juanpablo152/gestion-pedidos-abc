using order_api.Application.DTO;
using order_api.Application.Interfaces;
using order_api.Domain.Entities;

namespace order_api.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUserApiClient _userApiClient;

        public OrderService(IOrderRepository repository, IUserApiClient userApiClient)
        {
            _repository = repository;
            _userApiClient = userApiClient;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _repository.GetAllOrdersAsync();
                return orders.Select(o => new OrderResponseDto(
                    o.Id!,
                    o.UserId,
                    o.Items.Select(i => new OrderItemDto(i.ProductName, i.Quantity, i.UnitPrice)).ToList(),
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt,
                    o.UpdatedAt
                ));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener las órdenes.", ex);
            }
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                var orders = await _repository.GetOrdersByUserIdAsync(userId);
                return orders.Select(o => new OrderResponseDto(
                    o.Id!,
                    o.UserId,
                    o.Items.Select(i => new OrderItemDto(i.ProductName, i.Quantity, i.UnitPrice)).ToList(),
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt,
                    o.UpdatedAt
                ));
            }
            catch (Exception ex) when (ex is not ApplicationException)
            {
                throw new ApplicationException("Error al obtener las órdenes del usuario.", ex);
            }
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(string id)
        {
            try
            {
                var order = await _repository.GetOrderByIdAsync(id);
                if (order is null) return null;
                return MapToDto(order);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener la orden.", ex);
            }
        }

        public async Task<IEnumerable<OrderCompleteResponseDto>> GetAllOrdersWithUserAsync()
        {
            try
            {
                var orders = await _repository.GetAllOrdersAsync();
                var users = await _userApiClient.GetAllUsersAsync();
                var userMap = users.ToDictionary(u => u.Id);

                return orders.Select(o =>
                {
                    userMap.TryGetValue(o.UserId, out var user);
                    return new OrderCompleteResponseDto(
                        o.Id!,
                        user?.Name ?? "—",
                        user?.Email ?? "—",
                        user?.Address ?? "—",
                        o.Items.Select(i => new OrderItemDto(i.ProductName, i.Quantity, i.UnitPrice)).ToList(),
                        o.TotalAmount,
                        o.Status,
                        o.CreatedAt,
                        o.UpdatedAt
                    );
                });
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener las órdenes con información de usuarios.", ex);
            }
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateUpdateOrderDto dto)
        {
            try
            {
                var user = await _userApiClient.GetUserByIdAsync(dto.UserId);
                if (user is null)
                    throw new KeyNotFoundException($"El usuario no existe.");

                var items = dto.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList();

                var entity = new Order
                {
                    UserId = dto.UserId,
                    Items = items,
                    TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice),
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.CreateOrderAsync(entity);
                return MapToDto(entity);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al crear la orden.", ex);
            }
        }

        public async Task<OrderResponseDto> UpdateOrderAsync(string id, CreateUpdateOrderDto dto)
        {
            try
            {
                var existing = await _repository.GetOrderByIdAsync(id);
                if (existing is null)
                    throw new KeyNotFoundException($"La orden con id '{id}' no existe.");

                if (existing.UserId != dto.UserId)
                {
                    var user = await _userApiClient.GetUserByIdAsync(dto.UserId);
                    if (user is null)
                        throw new KeyNotFoundException($"El usuario con id '{dto.UserId}' no existe.");
                }

                existing.UserId = dto.UserId;
                existing.Items = dto.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList();
                existing.TotalAmount = existing.Items.Sum(i => i.Quantity * i.UnitPrice);
                existing.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateOrderAsync(id, existing);
                return MapToDto(existing);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al actualizar la orden.", ex);
            }
        }

        public async Task<bool> DeleteOrderByIdAsync(string id)
        {
            try
            {
                var existing = await _repository.GetOrderByIdAsync(id);
                if (existing is null) return false;

                await _repository.DeleteOrderByIdAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar la orden.", ex);
            }
        }

        public async Task<OrderResponseDto> PatchOrderStatusAsync(string id, UpdateOrderStatusDto dto)
        {
            try
            {
                var existing = await _repository.GetOrderByIdAsync(id);
                if (existing is null)
                    throw new KeyNotFoundException($"La orden con id '{id}' no existe.");

                existing.Status = dto.Status;
                existing.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateOrderAsync(id, existing);
                return new OrderResponseDto(
                    existing.Id!,
                    existing.UserId,
                    existing.Items.Select(i => new OrderItemDto(i.ProductName, i.Quantity, i.UnitPrice)).ToList(),
                    existing.TotalAmount,
                    existing.Status,
                    existing.CreatedAt,
                    existing.UpdatedAt
                );
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al actualizar el estado de la orden.", ex);
            }
        }

        private static OrderResponseDto MapToDto(Order o) =>
            new(
                o.Id!,
                o.UserId,
                o.Items.Select(i => new OrderItemDto(i.ProductName, i.Quantity, i.UnitPrice)).ToList(),
                o.TotalAmount,
                o.Status,
                o.CreatedAt,
                o.UpdatedAt
            );
    }
}
