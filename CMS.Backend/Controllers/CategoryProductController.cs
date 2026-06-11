using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace CMS.Backend.Controllers
{
    // Ép toàn bộ các hành động trong file này nhận diện qua đường dẫn ProductCategory
    [Route("ProductCategory")]
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TRANG DANH SÁCH (Đường dẫn: /ProductCategory)
        [HttpGet("")]
        public IActionResult Index()
        {
            var data = _context.CategoriesProducts.ToList();
            return View(data);
        }

        // 2. TẠO MỚI (GET: Hiển thị Form /ProductCategory/Create)
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // 2. TẠO MỚI (POST: Nhận dữ liệu từ Form gửi lên)
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                _context.CategoriesProducts.Add(model);
                _context.SaveChanges();
                // Ép chuyển hướng trực tiếp về thẳng URL gốc /ProductCategory
                return Redirect("/ProductCategory");
            }
            return View(model);
        }

        // 3. SỬA (GET: Hiển thị Form cũ /ProductCategory/Edit/id)
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = _context.CategoriesProducts.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // 3. SỬA (POST: Cập nhật dữ liệu)
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                _context.CategoriesProducts.Update(model);
                _context.SaveChanges();
                return Redirect("/ProductCategory");
            }
            return View(model);
        }

        // 4. XÓA NHANH (Đường dẫn: /ProductCategory/Delete/id)
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.CategoriesProducts.Find(id);
            if (category != null)
            {
                _context.CategoriesProducts.Remove(category);
                _context.SaveChanges();
            }
            return Redirect("/ProductCategory");
        }
    }
}