using Microsoft.Data.SqlClient;
using ShoesShop.Models;
using System.Data;

namespace ShoesShop.Data
{
    public class ReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        // Get reviews for a specific shoe
        public IEnumerable<ReviewModel> GetReviewsByShoeId(int shoeId)
        {
            var reviews = new List<ReviewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Reviews_SelectByShoeId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reviews.Add(new ReviewModel
                    {
                        ReviewId = Convert.ToInt32(reader["ReviewId"]),
                        UserId = reader["UserId"].ToString(),
                        ReviewerName = reader["ReviewerName"].ToString(),
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        Rating = Convert.ToInt32(reader["Rating"]),
                        Comment = reader["Comment"].ToString(),
                        ReviewDate = Convert.ToDateTime(reader["ReviewDate"])
                    });
                }
            }
            return reviews;
        }

        // Check if a user has purchased a shoe (before allowing a review)
        public bool HasUserPurchasedShoe(string userId, int shoeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_CheckUserPurchase", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);

                conn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar()); // 1 (purchased) or 0 (not purchased)
                return result == 1;
            }
        }

        // Insert a new review
        public bool InsertReview(AddReviewModel review)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Reviews_Add", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", review.UserId);
                cmd.Parameters.AddWithValue("@ShoeId", review.ShoeId);
                cmd.Parameters.AddWithValue("@Rating", review.Rating);
                cmd.Parameters.AddWithValue("@Comment", review.Comment);
                cmd.Parameters.AddWithValue("@ReviewDate", DateTime.UtcNow);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        // Check if user already reviewed this shoe
        public bool HasUserReviewed(string userId, int shoeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_CheckUserReview", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ShoeId", shoeId);

                conn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar()); // 1 (reviewed) or 0 (not reviewed)
                return result == 1;
            }
        }
    }
}
