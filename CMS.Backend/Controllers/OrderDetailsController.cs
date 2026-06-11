using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization; // 🌟 THÊM DÒNG NÀY: Thư viện hỗ trợ kiểm tra đăng nhập

namespace CMS.Backend.Controllers
{
    [Authorize] // 🌟 THÊM DÒNG NÀY: Khóa trang này lại, chưa đăng nhập sẽ bị đá ra trang Login
    [Route("OrderDetail/[action]")]
    [Route("OrderDetails/[action]")] // Chống lỗi 404 cho cả số ít và số nhiều
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hỗ trợ truy cập linh hoạt: /OrderDetail/Index/1 hoặc /OrderDetail?id=1
        [Route("~/OrderDetail/{id?}")]
        [Route("~/OrderDetails/{id?}")]
        public IActionResult Index(int? id)
        {
            // 1. Cơ chế phòng vệ: Nếu truy cập chạy chay không có ID, tự lấy mã đơn đầu tiên để hiển thị mẫu
            if (id == null || id == 0)
            {
                var firstOrder = _context.Orders.FirstOrDefault();
                if (firstOrder != null)
                {
                    id = firstOrder.Id;
                }
                else
                {
                    return Content("Hệ thống chưa có đơn hàng nào trong cơ sở dữ liệu.");
                }
            }

            // 2. Lấy danh sách chi tiết của đơn hàng, kết nối thông tin bảng Sản phẩm (Product)
            var orderDetails = _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == id)
                .ToList();

            // 3. Lấy thông tin tổng quát của hóa đơn để hiển thị lên phần Header
            var order = _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu đơn hàng qua ViewBag để hiển thị ngoài giao diện
            ViewBag.Order = order;

            return View(orderDetails);
        }
    }
}