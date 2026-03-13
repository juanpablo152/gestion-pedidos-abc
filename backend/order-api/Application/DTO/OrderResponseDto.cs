namespace order_api.Application.DTO
{
    public record OrderResponseDto(
        string Id,
        string UserId,
        List<OrderItemDto> Items,
        decimal TotalAmount,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public record OrderCompleteResponseDto(
        string Id,
        string UserName,
        string UserEmail,
        string UserAddress,
        List<OrderItemDto> Items,
        decimal TotalAmount,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
