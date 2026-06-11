using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data; // Thư mục chứa DbContext
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var latestPosts = await _context.Posts
                .AsNoTracking()
                .Include(p => p.Category) // eager load category
                .OrderByDescending(p => p.CreatedDate)
                .Take(3)
                .ToListAsync();

            return View(latestPosts);
        }
    }
}