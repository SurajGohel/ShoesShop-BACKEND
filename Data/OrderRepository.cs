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

        public GetOrderByOrderIdModel SelectByID(int orderId)
        {
            GetOrderByOrderIdModel order = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_GetOrderDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    order = new GetOrderByOrderIdModel
                    {
                        Status = reader["Status"].ToString(),
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        Address = reader["Address"].ToString(),
                        State = reader["State"].ToString(),
                        City = reader["City"].ToString(),
                        Pincode = reader["Pincode"].ToString(),
                        PaymentMethod = reader["PaymentMethod"].ToString(),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        TotalAmount = Convert.ToDouble(reader["TotalAmount"])
                    };
                }
            }
            return order;
        }


        public IEnumerable<GetOrderDetailByOrderIdModel> SelectOrderDetail(int orderId)
        {
            var orders = new List<GetOrderDetailByOrderIdModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_GetOrderDetailsByOrderId", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new GetOrderDetailByOrderIdModel()
                    {
                        ShoeId = Convert.ToInt32(reader["ShoeId"]),
                        ImageURL = reader["ImageURL"].ToString(),
                        ShoeName = reader["ShoeName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),

                    });
                }
            }
            return orders;
        }

        public IEnumerable<GetUserOrdersModel> SelectAllOrders()
        {
            var orders = new List<GetUserOrdersModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_GetAllUserOrders", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new GetUserOrdersModel()
                    {
                        UserId = reader["UserId"].ToString(),
                        UserName = reader["UserName"].ToString(),
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

        public bool UpdateOrderStatus(int orderId, string newStatus, string userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_UpdateOrderStatus", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@NewStatus", newStatus);

                // Add OUTPUT parameter to get affected rows
                SqlParameter rowsAffected = new SqlParameter("@RowsAffected", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(rowsAffected);

                conn.Open();
                cmd.ExecuteNonQuery();

                int affectedRows = Convert.ToInt32(rowsAffected.Value);
                Console.WriteLine($"Rows affected: {affectedRows}");

                return affectedRows > 0;
            }
        }


    }
}
