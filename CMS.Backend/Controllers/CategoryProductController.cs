using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CMS.Data;
using CMS.Data.Entities; // Kết nối tới lớp dữ liệu thực thể của bạn

namespace CMS.Backend.Controllers
{
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối (DbContext) vào Controller thông qua Dependency Injection
        public CategoryProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action Index để lấy danh sách danh mục sản phẩm từ SQL
        public IActionResult Index()
        {
            // Lấy dữ liệu THẬT từ bảng CategoriesProducts trong SQL Server
            // Lưu ý: Đảm bảo trong ApplicationDbContext bạn đã khai báo DbSet<CategoryProduct> CategoriesProducts
            var data = _context.CategoriesProducts.ToList();

            return View(data);
        }
    }
}