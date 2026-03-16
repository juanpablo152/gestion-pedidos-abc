using Microsoft.AspNetCore.Mvc;
using order_api.Application.DTO;
using order_api.Application.Interfaces;

namespace order_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("with-user")]
        public async Task<IActionResult> GetAllOrdersWithUser()
        {
            var orders = await _orderService.GetAllOrdersWithUserAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(string userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateUpdateOrderDto dto)
        {
            var created = await _orderService.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(GetOrderById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] CreateUpdateOrderDto dto)
        {
            var updated = await _orderService.UpdateOrderAsync(id, dto);
            return Ok(updated);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchOrderStatus(string id, [FromBody] UpdateOrderStatusDto dto)
        {
            var updated = await _orderService.PatchOrderStatusAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var deleted = await _orderService.DeleteOrderByIdAsync(id);
            return deleted ? Ok() : NotFound();
        }
    }
}
