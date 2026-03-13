namespace payment_api.Application.DTO
{
    public record PaymentResponseDto(
        string Id,
        string OrderId,
        decimal Amount,
        string Method,
        string Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
