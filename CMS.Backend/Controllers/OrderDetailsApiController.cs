using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // 1. API LẤY CÁC MÓN HÀNG THUỘC VỀ MỘT ĐƠN HÀNG (GET: api/OrderDetailsApi/{orderId})
        // ====================================================================
        [HttpGet("{orderId}")]
        public IActionResult GetByOrderId(int orderId)
        {
            // Lọc chính xác trong bảng OrderDetails của Toàn theo mã đơn hàng
            var details = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();

            if (details == null || !details.Any())
            {
                return NotFound(new { message = "Không tìm thấy chi tiết cho đơn hàng này!" });
            }
            return Ok(details);
        }
    }
}