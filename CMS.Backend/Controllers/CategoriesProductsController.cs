using CMS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // 1. API LẤY TOÀN BỘ DANH MỤC SẢN PHẨM (GET: api/CategoriesProducts)
        // ====================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            // Gọi chính xác bảng CategoriesProducts trùng khớp 100% với DbContext của Toàn
            var data = _context.CategoriesProducts.ToList();
            return Ok(data);
        }

        // ====================================================================
        // 2. API LẤY CHI TIẾT 1 DANH MỤC SẢN PHẨM (GET: api/CategoriesProducts/{id})
        // ====================================================================
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var item = _context.CategoriesProducts.Find(id);

            if (item == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục sản phẩm này!" });
            }
            return Ok(item);
        }
    }
}