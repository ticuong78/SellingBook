using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellingBook.Models;
using SellingBook.Models.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace SellingBook.Controllers
{
    [Area("Admin")] // Chỉ định khu vực cho controller này
    [Authorize(Roles = "Admin")] // Chỉ Admin mới có quyền quản lý đơn hàng
    public class OrderManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderManagementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)  
                .Include(o => o.OrderItems)  
                .ThenInclude(oi => oi.Product)  
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(string orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId); 

            if (order == null)
            {
                return NotFound();
            }

            return View(order);  
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems) 
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound();

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Index)); 
        }
    }
}
