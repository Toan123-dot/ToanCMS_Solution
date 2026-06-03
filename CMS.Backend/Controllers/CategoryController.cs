using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using System.Linq;
using CMS.Data;
using CMS.Data.Entities;
=======
using Microsoft.AspNetCore.Mvc;
using CMS.Data.Entities; // Kết nối tới lớp dữ liệu bạn vừa tạo
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3

namespace CMS.Backend.Controllers
{
    public class CategoryController : Controller
    {
<<<<<<< HEAD
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
=======
        public IActionResult Index()
        {
            // Tạo danh sách dữ liệu mẫu trực tiếp trong code
            var list = new List<Category> {
            new Category { Id = 1, Name = "Tin Công Nghệ", Description = "Review Laptop, AI" },
            new Category { Id = 2, Name = "Giáo Dục", Description = "Thông tin tuyển sinh" }
        };

            return View(list);// Gửi danh sách này sang giao diện
        }
    }
}
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3
