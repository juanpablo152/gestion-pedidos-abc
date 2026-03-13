using Microsoft.AspNetCore.Mvc;
using payment_api.Application.DTO;
using payment_api.Application.Interfaces;

namespace payment_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetAllPaymentsComplete()
        {
            var payments = await _paymentService.GetAllPaymentsCompleteAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(string id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            return payment is null ? NotFound() : Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrder(string orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreateUpdatePaymentDto dto)
        {
            var created = await _paymentService.CreatePaymentAsync(dto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(string id, [FromBody] CreateUpdatePaymentDto dto)
        {
            var updated = await _paymentService.UpdatePaymentAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(string id)
        {
            var deleted = await _paymentService.DeletePaymentByIdAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
