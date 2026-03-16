namespace payment_api.Application.DTO
{
    public record OrderWithUserDto(
        string Id,
        string UserName,
        string UserEmail,
        string UserAddress,
        List<OrderItemDto> Items,
        decimal TotalAmount,
        OrderStatus Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public record OrderItemDto(
        string ProductName,
        int Quantity,
        decimal UnitPrice
    );

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled,
    }
}
