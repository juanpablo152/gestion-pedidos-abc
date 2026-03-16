using System.Text.Json.Serialization;

namespace payment_api.Application.DTO
{
    public record UpdateOrderStatusDto(OrderStatus Status);
}
