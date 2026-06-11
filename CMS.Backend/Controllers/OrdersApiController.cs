using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // 1. API LẤY TẤT CẢ ĐƠN HÀNG (GET: api/OrdersApi)
        // ====================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            // Gọi chính xác bảng Orders của Toàn
            var orders = _context.Orders.OrderByDescending(o => o.Id).ToList();
            return Ok(orders);
        }

        // ====================================================================
        // 2. API LẤY CHI TIẾT 1 ĐƠN HÀNG (GET: api/OrdersApi/{id})
        // ====================================================================
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });
            }
            return Ok(order);
        }
    }
}