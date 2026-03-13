using order_api.Domain.Entities;

namespace order_api.Application.DTO
{
    public record CreateUpdateOrderDto(
        string UserId,
        List<OrderItemDto> Items
    );
}
