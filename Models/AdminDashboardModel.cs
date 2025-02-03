namespace ShoesShop.Models
{
    public class SalesDataModel
    {
        public string Month { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class LowStockModel
    {
        public string Name { get; set; }
        public int Stock { get; set; }
    }

    public class TopSellingModel
    {
        public string Name { get; set; }
        public int TotalSold { get; set; }
    }

    public class DashboardStatsModel
    {
        public int TotalVisitors { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
    }
}
