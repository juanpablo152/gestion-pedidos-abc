using order_api.Domain.Entities;

namespace order_api.Application.DTO
{
    public record UpdateOrderStatusDto(OrderStatus Status);
}
