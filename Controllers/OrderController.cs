using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]

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

        [HttpGet("/OrderByOrderId/{id}")]
        public IActionResult OrderById(int id)
        {
            var order = _orderRepository.SelectByID(id);
            if (order == null)
            {
                NotFound("No order found with the provided Id.");
            }
            return Ok(order);
        }

        [HttpGet("/GetAllOrderByOrderId/{id}")]
        public IActionResult ShoDetail(int id)
        {
            var orders = _orderRepository.SelectOrderDetail(id);
            if (orders == null)
            {
                NotFound("No Order found with the provided Id.");
            }
            return Ok(orders);
        }

        [HttpGet("/GetAllUserOrders")]
        public IActionResult AllOrders()
        {
            var ods = _orderRepository.SelectAllOrders();
            if (ods == null)
            {
                NotFound("No Order found with the provided Id.");
            }
            return Ok(ods);
        }

        [HttpPost("updatestatus/{userId}/{orderId}/{newStatus}")]
        public IActionResult UpdateOrderStatus(string userId, int orderId, string newStatus)
        {
            Console.WriteLine("Callinf Baby");
            Console.WriteLine(userId+" "+orderId+" "+newStatus);
            // Update status in the database (Assuming you have an `UpdateOrderStatus` method)
            bool isUpdated = _orderRepository.UpdateOrderStatus(orderId, newStatus, userId);

            if (isUpdated)
            {
                return Ok(new { Message = "Order status updated successfully." });
            }
            return BadRequest("Failed to update order status.");
        }

    }
}
