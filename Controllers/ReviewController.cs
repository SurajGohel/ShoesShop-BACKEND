using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewRepository;

        public ReviewController(ReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet("ByShoeId/{shoeId}")]
        public IActionResult GetReviewsByShoeId(int shoeId)
        {
            var reviews = _reviewRepository.GetReviewsByShoeId(shoeId);
            return Ok(reviews);
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] AddReviewModel review)
        {
            if (review == null)
                return BadRequest("Invalid review data.");

            if (!_reviewRepository.HasUserPurchasedShoe(review.UserId, review.ShoeId))
                return BadRequest("User has not purchased this shoe.");

            if (_reviewRepository.HasUserReviewed(review.UserId, review.ShoeId))
                return BadRequest("User has already reviewed this shoe.");

            bool isInserted = _reviewRepository.InsertReview(review);

            if (isInserted)
                return Ok(new { Message = "Review added successfully" });

            return StatusCode(500, "An error occurred while adding the review.");
        }
    }
}
