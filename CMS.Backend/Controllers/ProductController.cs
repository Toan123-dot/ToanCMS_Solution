using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize] // Bắt buộc đăng nhập để bảo mật trang quản lý sản phẩm
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Hàm khởi tạo bắt buộc để kết nối SQL và nhận quyền lưu file vào wwwroot
        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. TRANG DANH SÁCH SẢN PHẨM (GET: /Product hoặc /Product/Index)
        public IActionResult Index()
        {
            if (_context.Products == null)
            {
                return Content("Lỗi: Không tìm thấy bảng Products trong cơ sở dữ liệu.");
            }

            var products = _context.Products.ToList();
            return View(products);
        }

        // 2. TRANG CHI TIẾT SẢN PHẨM (GET: /Product/Details/1)
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var product = _context.Products.FirstOrDefault(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // 3. TRANG FORM THÊM MỚI SẢN PHẨM (GET: /Product/Create)
        public IActionResult Create()
        {
            return View();
        }

        // 4. XỬ LÝ LƯU THÊM MỚI SẢN PHẨM (POST: /Product/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }

                    product.ImageUrl = fileName;
                }

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // 5. TRANG FORM SỬA SẢN PHẨM (GET: /Product/Edit/1)
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // 6. XỬ LÝ LƯU CẬP NHẬT SẢN PHẨM (POST: /Product/Edit/1)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product, IFormFile? ImageFile)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin sản phẩm hiện tại trong DB ra để so sánh ảnh cũ (Dùng AsNoTracking để tránh xung đột Entity)
                    var existingProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (existingProduct == null) return NotFound();

                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        // Tạo tên file ảnh mới ngẫu nhiên hoàn toàn
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string filePath = Path.Combine(uploadDir, fileName);

                        // Bước A: Ghi file ảnh MỚI vào thư mục trước
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageFile.CopyTo(fileStream);
                        }

                        // Bước B: Sau khi ghi thành công ảnh mới, tiến hành xóa ảnh cũ an toàn (nếu có)
                        if (!string.IsNullOrEmpty(existingProduct.ImageUrl) && !existingProduct.ImageUrl.StartsWith("http"))
                        {
                            string oldFilePath = Path.Combine(uploadDir, existingProduct.ImageUrl);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                                catch (Exception)
                                {
                                    // Nếu ảnh cũ đang bị giữ luồng đọc, bỏ qua không xóa để tránh sập web
                                }
                            }
                        }

                        // Gán tên file ảnh mới vào sản phẩm để lưu
                        product.ImageUrl = fileName;
                    }
                    else
                    {
                        // Nếu người dùng không upload ảnh mới -> Giữ nguyên tên file ảnh cũ từ cơ sở dữ liệu
                        product.ImageUrl = existingProduct.ImageUrl;
                    }

                    _context.Products.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // 7. MỚI: CHỨC NĂNG XÓA SẢN PHẨM VÀ ẢNH VẬT LÝ (GET: /Product/Delete/Id)
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            try
            {
                // Bước A: Tự động xóa file ảnh trong thư mục wwwroot/images để tránh rác ổ cứng
                if (!string.IsNullOrEmpty(product.ImageUrl) && !product.ImageUrl.StartsWith("http"))
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string filePath = Path.Combine(uploadDir, product.ImageUrl);

                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath);
                        }
                        catch (Exception)
                        {
                            // Nếu file bị khóa, bỏ qua để không làm nghẽn tiến trình xóa SQL
                        }
                    }
                }

                // Bước B: Xóa dữ liệu sản phẩm trong bảng SQL
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content("Lỗi khi xóa sản phẩm: " + ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}