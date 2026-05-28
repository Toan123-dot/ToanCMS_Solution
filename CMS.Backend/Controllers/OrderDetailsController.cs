using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hỗ trợ truy cập cả gõ chay: /OrderDetails hoặc truyền mã /OrderDetails?orderId=1
        public IActionResult Index(int? orderId)
        {
            // 1. Cơ chế phòng vệ: Nếu gõ chạy không có Id, tự động lấy mã đơn hàng đầu tiên hiện có để hiển thị mẫu
            if (orderId == null || orderId == 0)
            {
                var firstOrder = _context.Orders.FirstOrDefault();
                if (firstOrder != null)
                {
                    orderId = firstOrder.Id;
                }
                else
                {
                    // Nếu hệ thống hoàn toàn trống rỗng không có đơn nào
                    return Content("Hệ thống chưa có đơn hàng nào. Vui lòng nạp dữ liệu SQL trước!");
                }
            }

            // 2. Lấy danh sách chi tiết của đơn hàng, đồng thời kết nối thông tin Sản phẩm (Product) đi kèm
            var orderDetails = _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId)
                .ToList();

            // 3. Lấy thông tin tổng quát của hóa đơn để hiển thị trên phần Header
            var order = _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Truyền thông tin đơn hàng qua ViewBag để hiển thị ngoài giao diện
            ViewBag.Order = order;

            return View(orderDetails);
        }
    }
}