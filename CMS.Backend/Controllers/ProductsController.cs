using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // 1. API LẤY TẤT CẢ SẢN PHẨM (GET: api/Products)
        // ====================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            // Gọi chính xác bảng Products của Toàn
            var products = _context.Products.OrderByDescending(p => p.Id).ToList();
            return Ok(products);
        }

        // ====================================================================
        // 2. API LẤY CHI TIẾT 1 SẢN PHẨM (GET: api/Products/{id})
        // ====================================================================
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm này!" });
            }
            return Ok(product);
        }
    }
}