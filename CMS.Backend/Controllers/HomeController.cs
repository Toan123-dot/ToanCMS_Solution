using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using System.Linq;
using System.Diagnostics;
using CMS.Backend.Models;
using System.Threading.Tasks; // Thêm namespace này nếu chưa có mặc định

namespace CMS.Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 1. Thay đổi 'IActionResult' thành 'async Task<IActionResult>'
        public async Task<IActionResult> Index()
        {   
            // LINQ Async: Thêm 'await' và đổi '.ToList()' thành '.ToListAsync()'
            var latestPosts = await _context.Posts
                                .Include(p => p.Category)
                                .OrderByDescending(p => p.CreatedDate)
                                .Take(3)
                                .ToListAsync();

            return View(latestPosts);
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