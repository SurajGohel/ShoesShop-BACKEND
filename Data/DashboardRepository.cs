using Microsoft.Data.SqlClient;
using ShoesShop.Models;
using System.Collections.Generic;
using System.Data;

namespace ShoesShop.Data
{
    public class DashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        // Fetch monthly sales data
        public IEnumerable<SalesDataModel> GetMonthlySales()
        {
            var salesData = new List<SalesDataModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT FORMAT(OrderDate, 'MM-yyyy') AS Month, SUM(TotalAmount) AS TotalSales
                    FROM Orders
                    GROUP BY FORMAT(OrderDate, 'MM-yyyy')
                    ORDER BY Month;", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    salesData.Add(new SalesDataModel()
                    {
                        Month = reader["Month"].ToString(),
                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                    });
                }
            }
            return salesData;
        }

        // Fetch low stock items
        public IEnumerable<LowStockModel> GetLowStockItems()
        {
            var stockData = new List<LowStockModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT Name, Stock FROM Shoes WHERE Stock < 10 ORDER BY Stock ASC;", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    stockData.Add(new LowStockModel()
                    {
                        Name = reader["Name"].ToString(),
                        Stock = Convert.ToInt32(reader["Stock"])
                    });
                }
            }
            return stockData;
        }

        // Fetch top selling shoes
        public IEnumerable<TopSellingModel> GetTopSellingShoes()
        {
            var topSelling = new List<TopSellingModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 5 s.Name, SUM(od.Quantity) AS TotalSold
                    FROM OrderDetails od
                    JOIN Shoes s ON od.ShoeId = s.ShoeId
                    GROUP BY s.Name
                    ORDER BY TotalSold DESC;", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    topSelling.Add(new TopSellingModel()
                    {
                        Name = reader["Name"].ToString(),
                        TotalSold = Convert.ToInt32(reader["TotalSold"])
                    });
                }
            }
            return topSelling;
        }

        // Fetch dashboard statistics using stored procedures
        public DashboardStatsModel GetDashboardStats()
        {
            var stats = new DashboardStatsModel();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Total Visitors
                using (SqlCommand cmd = new SqlCommand("GetTotalVisitors", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    stats.TotalVisitors = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Total Sales
                using (SqlCommand cmd = new SqlCommand("GetTotalSales", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    stats.TotalSales = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // Total Orders
                using (SqlCommand cmd = new SqlCommand("GetTotalOrders", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    stats.TotalOrders = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return stats;
        }
    }
}
