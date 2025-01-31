using Microsoft.Data.SqlClient;
using ShoesShop.Models;
using System.Data;

namespace ShoesShop.Data
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public int Checkout(OrderModel order)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Checkout", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", order.UserId);
                cmd.Parameters.AddWithValue("@FullName", order.FullName);
                cmd.Parameters.AddWithValue("@Address", order.Address);
                cmd.Parameters.AddWithValue("@City", order.City);
                cmd.Parameters.AddWithValue("@State", order.State);
                cmd.Parameters.AddWithValue("@Pincode", order.Pincode);
                cmd.Parameters.AddWithValue("@MobileNumber", order.MobileNumber);

                conn.Open();

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
