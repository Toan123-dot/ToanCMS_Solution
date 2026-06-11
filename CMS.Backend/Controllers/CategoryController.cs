using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    
    [Authorize] // Bắt buộc phải đăng nhập mới được vào [cite: 4184]

    [Authorize] // Bắt buộc phải đăng nhập mới được vào [cite: 4184]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TRANG DANH SÁCH (INDEX)
        public IActionResult Index()
        {
            var data = _context.Categories.ToList(); // Lấy dữ liệu thật từ bảng Categories [cite: 2975]
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            ModelState.Remove("Posts");

            if (ModelState.IsValid)
            {
                _context.Categories.Add(model); // Thêm vào bộ nhớ tạm [cite: 3184]
                _context.SaveChanges(); // Lưu thực sự vào SQL Server [cite: 3187]
                return RedirectToAction("Index");
            }
            return View(model); // Nếu dữ liệu lỗi, trả lại form kèm dữ liệu đã nhập
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
           

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound(); 
            }

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            ModelState.Remove("Posts");
            if (ModelState.IsValid)
            {
                _context.Categories.Update(model);
                _context.SaveChanges(); // Lưu thay đổi xuống SQL Server [cite: 3323]

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // ====================================================================
        // 4. XÓA DANH MỤC (Xử lý xóa nhanh trực tiếp từ trang danh sách)
        // Đường dẫn kích hoạt từ trang danh sách: /Category/Delete/{id}
        // ====================================================================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}