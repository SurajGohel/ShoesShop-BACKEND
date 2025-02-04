namespace ShoesShop.Models
{
    public class ReviewModel
    {
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string ReviewerName { get; set; }
        public int ShoeId { get; set; }
        public int Rating { get; set; } // Rating (1-5)
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class AddReviewModel
    {
        public string UserId { get; set; }
        public int ShoeId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
