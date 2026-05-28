using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Bắt buộc phải thêm dòng này để dùng .Include()
using System.Linq;
using CMS.Data;

namespace CMS.Backend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // URL: https://localhost:7011/Orders
        public IActionResult Index()
        {
            // Lấy danh sách đơn hàng đồng thời kết nối sang bảng Customer để lấy tên
            var orders = _context.Orders.Include(o => o.Customer).ToList();
            return View(orders);
        }
    }
}