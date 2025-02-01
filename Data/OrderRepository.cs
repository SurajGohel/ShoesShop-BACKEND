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

        public IEnumerable<GetUserOrdersModel> SelectAll(string userId)
        {
            var orders = new List<GetUserOrdersModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_GetUserOrders", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new GetUserOrdersModel()
                    {
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        PaymentMethod = reader["PaymentMethod"].ToString(),
                        Status = reader["Status"].ToString(),
                        LastStatusChangedDate = Convert.ToDateTime(reader["LastStatusChangedDate"]),
                        TotalAmount = Convert.ToDouble(reader["TotalAmount"]),

                    });
                }
            }
            return orders;
        }
    }
}
