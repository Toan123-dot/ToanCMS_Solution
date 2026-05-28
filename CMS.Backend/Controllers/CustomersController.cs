using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CMS.Data;
using CMS.Data.Entities;

namespace CMS.Backend.Controllers
{
    // Đảm bảo tên class phải có chữ 's' giống tên file CustomersController.cs
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Đường dẫn mặc định sẽ là: https://localhost:7011/Customers
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            return View(customers); // Sẽ tìm đến Views/Customers/Index.cshtml
        }
    }
}