using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellingBook.Models;
using SellingBook.Models.Roles;
using SellingBook.Models.Statistic;

namespace SellingBook.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StatisticController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult DailyStats()
        {
            var endDate = DateTime.Today;
            var startDate = endDate.AddDays(-6); // Lấy 7 ngày gần nhất

            var dailyStats = _context.Orders
                .Where(o => o.CreatedAt.Date >= startDate && o.CreatedAt.Date <= endDate)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            var model = new DailyStatistic
            {
                Dates = dailyStats.Select(d => d.Date.ToString("dd/MM/yyyy")).ToArray(),
                OrderCounts = dailyStats.Select(d => d.TotalOrders).ToArray(),
                Revenues = dailyStats.Select(d => d.TotalRevenue).ToArray()
            };

            return View(model);
        }

        // Thống kê theo tháng (trong năm hiện tại)
        public IActionResult MonthlyStats(int? year)
        {
            var selectedYear = year ?? DateTime.Today.Year;

            var monthlyStats = _context.Orders
                .Where(o => o.CreatedAt.Year == selectedYear)
                .GroupBy(o => o.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .ToList();

            var model = new MonthlyStatistic
            {
                Year = selectedYear,
                Months = monthlyStats.Select(m => m.Month).ToArray(),
                OrderCounts = monthlyStats.Select(m => m.TotalOrders).ToArray(),
                Revenues = monthlyStats.Select(m => m.TotalRevenue).ToArray()
            };

            return View(model);
        }
    }
}
