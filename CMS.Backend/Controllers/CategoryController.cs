using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CMS.Data;
using CMS.Data.Entities;

namespace CMS.Backend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. XEM DANH SÁCH
        public IActionResult Index()
        {
            var data = _context.Categories.ToList();
            return View(data);
        }

        // 2. THÊM MỚI (CREATE)
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Category model)
        {
            _context.Categories.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 3. CHỈNH SỬA (EDIT)
        // [GET]: Hiển thị form chứa thông tin cũ để người dùng sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // [POST]: Lưu thay đổi xuống SQL Server
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            // BƯỚC 1: Đánh dấu đối tượng model là đã thay đổi
            _context.Categories.Update(model);

            // BƯỚC 2: Chốt lệnh cập nhật (UPDATE) vào Database
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 4. XÓA (DELETE)
        public IActionResult Delete(int id)
        {
            // BƯỚC 1: Tìm đối tượng cần xóa
            var category = _context.Categories.Find(id);

            // BƯỚC 2: Nếu tìm thấy thì xóa khỏi bộ nhớ tạm
            if (category != null)
            {
                _context.Categories.Remove(category);

                // BƯỚC 3: Chốt lệnh xóa (DELETE) vào Database
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}