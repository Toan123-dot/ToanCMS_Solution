using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CMS.Data;
using CMS.Data.Entities;

namespace CMS.Backend.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách sản phẩm từ SQL
        public IActionResult Index()
        {
            // Lấy toàn bộ danh sách sản phẩm thực tế từ DB
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}