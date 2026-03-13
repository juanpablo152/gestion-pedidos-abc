using payment_api.Domain.Entities;

namespace payment_api.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(string orderId);
        Task<Payment?> GetPaymentByIdAsync(string id);
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(string id, Payment payment);
        Task DeletePaymentByIdAsync(string id);
    }
}
