namespace order_api.Application.DTO
{
    public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
}
