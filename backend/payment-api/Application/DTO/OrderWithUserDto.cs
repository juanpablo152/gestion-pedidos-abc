namespace payment_api.Application.DTO
{
    public record OrderWithUserDto(
        string Id,
        string UserName,
        string UserEmail,
        string UserAddress,
        List<OrderItemDto> Items,
        decimal TotalAmount,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public record OrderItemDto(
        string ProductName,
        int Quantity,
        decimal UnitPrice
    );
}
