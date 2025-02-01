using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost("/CheckOut")]
        public IActionResult Checkout([FromBody] OrderModel order)
        {
            if (order == null)
            {
                return BadRequest("Invalid checkout data.");
            }

            int orderId = _orderRepository.Checkout(order);

            if (orderId > 0)
            {
                return Ok(new { Message = "Checkout successful", OrderId = orderId });
            }

            return StatusCode(500, "An error occurred while processing checkout.");
        }

        [HttpGet("/GetUserOrders/{id}")]
        public IActionResult ShoDetail(string id)
        {
            var orders = _orderRepository.SelectAll(id);
            if (orders == null)
            {
                NotFound("No Order found with the provided Id.");
            }
            return Ok(orders);
        }

    }
}
