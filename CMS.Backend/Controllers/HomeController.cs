<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data; // Th? m?c ch?a DbContext
using System.Linq;
=======
using System.Diagnostics;
using CMS.Backend.Models;
using Microsoft.AspNetCore.Mvc;
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3

namespace CMS.Backend.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
=======
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3
        }

        public IActionResult Index()
        {
<<<<<<< HEAD
            // LINQ: Gi? nguyên 100% code c?a th?y, không thay ??i m?t ch?
            var latestPosts = _context.Posts
                                .Include(p => p.Category) // L?y kèm tên danh m?c ?? hi?n th? 
                                .OrderByDescending(p => p.CreatedDate) // S?p x?p ngày m?i nh?t lên ??u 
                                .Take(3) // Ch? l?y ?úng 3 b?n tin ??u tiên
                                .ToList();

            return View(latestPosts);
        }
    }
}
=======
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3
