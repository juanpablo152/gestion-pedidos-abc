using payment_api.Application.DTO;
using payment_api.Application.Interfaces;
using payment_api.Domain.Entities;

namespace payment_api.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IOrderApiClient _orderApiClient;

        public PaymentService(IPaymentRepository repository, IOrderApiClient orderApiClient)
        {
            _repository = repository;
            _orderApiClient = orderApiClient;
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _repository.GetAllPaymentsAsync();
                return payments.Select(p => new PaymentResponseDto(
                    p.Id!,
                    p.OrderId,
                    p.Amount,
                    p.Method.ToString(),
                    p.Status.ToString(),
                    p.CreatedAt,
                    p.UpdatedAt
                ));
            }
            catch (Exception ex) when (ex is not ApplicationException)
            {
                throw new ApplicationException("Error al obtener los pagos.", ex);
            }
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByOrderIdAsync(string orderId)
        {
            try
            {
                var payments = await _repository.GetPaymentsByOrderIdAsync(orderId);
                return payments.Select(p => new PaymentResponseDto(
                    p.Id!,
                    p.OrderId,
                    p.Amount,
                    p.Method.ToString(),
                    p.Status.ToString(),
                    p.CreatedAt,
                    p.UpdatedAt
                ));
            }
            catch (Exception ex) when (ex is not ApplicationException)
            {
                throw new ApplicationException("Error al obtener los pagos de la orden.", ex);
            }
        }

        public async Task<PaymentResponseDto?> GetPaymentByIdAsync(string id)
        {
            try
            {
                var payment = await _repository.GetPaymentByIdAsync(id);
                if (payment is null) return null;
                return new PaymentResponseDto(
                    payment.Id!,
                    payment.OrderId,
                    payment.Amount,
                    payment.Method.ToString(),
                    payment.Status.ToString(),
                    payment.CreatedAt,
                    payment.UpdatedAt
                );
            }
            catch (Exception ex) when (ex is not ApplicationException)
            {
                throw new ApplicationException("Error al obtener el pago.", ex);
            }
        }

        public async Task<PaymentResponseDto> CreatePaymentAsync(CreateUpdatePaymentDto dto)
        {
            try
            {
                var orderExists = await _orderApiClient.OrderExistsAsync(dto.OrderId);
                if (!orderExists)
                    throw new KeyNotFoundException($"La orden con id no existe.");

                var entity = new Payment
                {
                    OrderId = dto.OrderId,
                    Amount = dto.Amount,
                    Method = dto.Method,
                    Status = dto.Status,
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.CreatePaymentAsync(entity);

                if (dto.Status == PaymentStatus.Completed) {
                    await _orderApiClient.UpdateOrderStatusAsync(dto.OrderId, OrderStatus.Completed);
                } else {
                    await _orderApiClient.UpdateOrderStatusAsync(dto.OrderId, OrderStatus.Pending);
                }
                return new PaymentResponseDto(
                    entity.Id!,
                    entity.OrderId,
                    entity.Amount,
                    entity.Method.ToString(),
                    entity.Status.ToString(),
                    entity.CreatedAt,
                    entity.UpdatedAt
                );
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al crear el pago.", ex);
            }
        }

        public async Task<PaymentResponseDto> UpdatePaymentAsync(string id, CreateUpdatePaymentDto dto)
        {
            try
            {
                var existing = await _repository.GetPaymentByIdAsync(id);
                if (existing is null)
                    throw new KeyNotFoundException($"El pago con id '{id}' no existe.");

                if (existing.OrderId != dto.OrderId)
                {
                    var orderExists = await _orderApiClient.OrderExistsAsync(dto.OrderId);
                    if (!orderExists)
                        throw new KeyNotFoundException($"La orden con id '{dto.OrderId}' no existe.");
                }

                existing.OrderId = dto.OrderId;
                existing.Amount = dto.Amount;
                existing.Method = dto.Method;
                existing.Status = dto.Status;
                existing.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdatePaymentAsync(id, existing);

                if (dto.Status == PaymentStatus.Completed) {
                    await _orderApiClient.UpdateOrderStatusAsync(existing.OrderId, OrderStatus.Completed);
                } else {
                    await _orderApiClient.UpdateOrderStatusAsync(existing.OrderId, OrderStatus.Pending);
                }

                return new PaymentResponseDto(
                    existing.Id!,
                    existing.OrderId,
                    existing.Amount,
                    existing.Method.ToString(),
                    existing.Status.ToString(),
                    existing.CreatedAt,
                    existing.UpdatedAt
                );
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al actualizar el pago.", ex);
            }
        }

        public async Task<bool> DeletePaymentByIdAsync(string id)
        {
            try
            {
                var existing = await _repository.GetPaymentByIdAsync(id);
                if (existing is null) return false;

                await _repository.DeletePaymentByIdAsync(id);
                return true;
            }
            catch (Exception ex) when (ex is not ApplicationException)
            {
                throw new ApplicationException("Error al eliminar el pago.", ex);
            }
        }

        public async Task<IEnumerable<PaymentCompleteResponseDto>> GetAllPaymentsCompleteAsync()
        {
            try
            {
                var payments = await _repository.GetAllPaymentsAsync();
                var orders = await _orderApiClient.GetAllOrdersWithUserAsync();

                var orderMap = orders.ToDictionary(o => o.Id);

                return payments.Select(p =>
                {
                    orderMap.TryGetValue(p.OrderId, out var order);
                    return new PaymentCompleteResponseDto(
                        p.Id!,
                        p.Amount,
                        p.Method.ToString(),
                        p.Status.ToString(),
                        p.CreatedAt,
                        p.UpdatedAt,
                        p.OrderId,
                        order?.TotalAmount ?? 0,
                        order?.Items ?? [],
                        order?.Status ?? OrderStatus.Pending,
                        order?.UserName ?? "—",
                        order?.UserEmail ?? "—",
                        order?.UserAddress ?? "—"
                    );
                });
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los pagos con información completa.", ex);
            }
        }
    }
}
