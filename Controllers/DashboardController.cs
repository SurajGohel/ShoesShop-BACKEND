using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;
using System.Collections.Generic;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _dashboardRepository;

        public DashboardController(DashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("sales-data")]
        public IActionResult GetMonthlySales()
        {
            IEnumerable<SalesDataModel> salesData = _dashboardRepository.GetMonthlySales();
            return Ok(salesData);
        }

        [HttpGet("low-stock")]
        public IActionResult GetLowStockItems()
        {
            IEnumerable<LowStockModel> lowStockItems = _dashboardRepository.GetLowStockItems();
            return Ok(lowStockItems);
        }

        [HttpGet("top-selling")]
        public IActionResult GetTopSellingShoes()
        {
            IEnumerable<TopSellingModel> topSellingShoes = _dashboardRepository.GetTopSellingShoes();
            return Ok(topSellingShoes);
        }

        [HttpGet("stats")]
        public IActionResult GetDashboardStats()
        {
            DashboardStatsModel stats = _dashboardRepository.GetDashboardStats();
            return Ok(stats);
        }
    }
}
