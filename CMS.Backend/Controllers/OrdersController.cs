using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Authorization; // 🌟 THÊM DÒNG NÀY: Để sử dụng bộ lọc bảo mật

namespace CMS.Backend.Controllers
{
    [Authorize] // 🌟 THÊM DÒNG NÀY: Khóa toàn bộ Controller đơn hàng (bắt buộc phải đăng nhập)
    [Route("Orders/[action]")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TRANG DANH SÁCH ĐƠN HÀNG (Chấp nhận cả /Orders và /Order)
        [Route("~/Orders")]
        [Route("~/Order")]
        public IActionResult Index()
        {
            var orders = _context.Orders.Include(o => o.Customer).ToList();
            return View(orders);
        }

        // 2. GIAO DIỆN THAY ĐỔI TRẠNG THÁI ĐƠN HÀNG (GET: /Orders/Edit/1)
        [HttpGet("{id?}")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = _context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            return View(order);
        }

        // 3. XỬ LÝ LƯU TRẠNG THÁI ĐƠN HÀNG (POST: /Orders/Edit/1)
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int status, string notes)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            // Sửa trạng thái và ghi chú
            order.Status = status;
            order.Notes = notes;

            if (ModelState.IsValid)
            {
                _context.Orders.Update(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
    }
}