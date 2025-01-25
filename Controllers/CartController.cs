using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartRepository _cartRepository;

        public CartController(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("/ByUserId/{id}")]
        public IActionResult GetAllCartByUserId(string id)
        {
            var carts = _cartRepository.GetCartByUserId(id);

            //if (carts == null || !carts.Any())
            //{
            //    return NotFound(new { message = "No carts found for the provided user ID." }); 
            //}

            return Ok(carts);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            var isDel = _cartRepository.Delete(id);
            if (!isDel)
                return NotFound();
            return Ok(new { message = "Cart deleted successfully" });
        }
    }
}
