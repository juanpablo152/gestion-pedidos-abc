namespace payment_api.Application.DTO
{
    public record PaymentCompleteResponseDto(
        string Id,
        decimal Amount,
        string PaymentMethod,
        string PaymentStatus,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        string OrderId,
        decimal OrderTotalAmount,
        List<OrderItemDto> OrderItems,
        string UserName,
        string UserEmail,
        string UserAddress
    );
}
