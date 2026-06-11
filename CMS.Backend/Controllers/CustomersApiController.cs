using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // 1. API LẤY TOÀN BỘ KHÁCH HÀNG (GET: api/CustomersApi)
        // ====================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            // Gọi chính xác bảng Customers trong DbContext của Toàn
            var customers = _context.Customers.OrderByDescending(c => c.Id).ToList();
            return Ok(customers);
        }

        // ====================================================================
        // 2. API LẤY CHI TIẾT 1 KHÁCH HÀNG (GET: api/CustomersApi/{id})
        // ====================================================================
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng này!" });
            }
            return Ok(customer);
        }
    }
}