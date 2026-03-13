using payment_api.Application.DTO;

namespace payment_api.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByOrderIdAsync(string orderId);
        Task<PaymentResponseDto?> GetPaymentByIdAsync(string id);
        Task<PaymentResponseDto> CreatePaymentAsync(CreateUpdatePaymentDto dto);
        Task<PaymentResponseDto> UpdatePaymentAsync(string id, CreateUpdatePaymentDto dto);
        Task<bool> DeletePaymentByIdAsync(string id);
        Task<IEnumerable<PaymentCompleteResponseDto>> GetAllPaymentsCompleteAsync();
    }
}
