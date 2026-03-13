using payment_api.Domain.Entities;

namespace payment_api.Application.DTO
{
    public record CreateUpdatePaymentDto(
        string OrderId,
        decimal Amount,
        PaymentMethod Method,
        PaymentStatus Status
    );
}
