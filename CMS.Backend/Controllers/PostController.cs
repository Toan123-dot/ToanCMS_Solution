<<<<<<< HEAD
﻿using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Hỗ trợ Eager Loading liên kết dữ liệu giữa các bảng
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
=======
﻿using Microsoft.AspNetCore.Mvc;
using CMS.Data.Entities; // Quan trọng: Phải có dòng này để dùng lớp Post
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
<<<<<<< HEAD
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            var query = _context.Posts
                .Include(p => p.Category)
                .AsQueryable();
            if (id != null)
            {
                query = query.Where(p => p.CategoryId == id);
            }
            var posts = query.OrderByDescending(p => p.CreatedDate).ToList();
            return View(posts);
        }
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound(); // Trả về trang lỗi 404
            }
            return View(post);
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Chúng ta lấy danh sách Category để đổ vào ViewBag
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post model, IFormFile uploadImage)
        {
            if (uploadImage != null && uploadImage.Length > 0)
            {
                // 1. Định nghĩa đường dẫn lưu file: wwwroot/uploads
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                // 2. Tạo tên file duy nhất để không bị đè dữ liệu
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                string filePath = Path.Combine(folder, fileName);

                // 3. Chép file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }

                // 4. Lưu đường dẫn vào CSDL để sau này hiển thị
                model.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                model.ImageUrl = "/uploads/default.jpg";
            }

            if (model.CreatedDate == DateTime.MinValue)
            {
                model.CreatedDate = DateTime.Now;
            }

            _context.Posts.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Hiển thị form kèm dữ liệu cũ
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            // Chuẩn bị lại danh sách danh mục để người dùng có thể đổi chuyên mục
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Thực hiện cập nhật
        [HttpPost]
        public IActionResult Edit(Post model, IFormFile uploadImage)
        {
            // Bước 1: Kiểm tra xem người dùng có chọn file ảnh mới không
            if (uploadImage != null && uploadImage.Length > 0)
            {
                // Thực hiện quy trình upload giống như trang Create
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }

                // Cập nhật đường dẫn ảnh mới vào model
                model.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                // Bước quan trọng: Nếu không upload ảnh mới, chúng ta phải giữ lại ảnh cũ
                // Chúng ta cần lấy lại giá trị ImageUrl từ Database để tránh bị ghi đè thành rỗng
                var oldPost = _context.Posts.AsNoTracking().FirstOrDefault(p => p.Id == model.Id);
                if (oldPost != null && string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = oldPost.ImageUrl;
                }
            }
            _context.Posts.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            // 1. Tìm bài viết theo Id
            var post = _context.Posts.Find(id);

            if (post != null)
            {
                // 2. Xóa khỏi bộ nhớ tạm
                _context.Posts.Remove(post);

                // 3. Cập nhật xuống SQL Server
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
=======
        // Hàm Index: Hiển thị danh sách bài viết mẫu
        public IActionResult Index()
        {
            // 1. Tạo dữ liệu giả (Mock Data) cho Bài viết
            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Lộ trình học ASP.NET Core cho người mới",
                    Content = "Nội dung bài viết về lộ trình học .NET...",
                    ImageUrl = "https://via.placeholder.com/150",
                    CreatedDate = DateTime.Now
                },
                new Post
                {
                    Id = 2,
                    Title = "ReactJS và WebAPI: Xu hướng Fullstack 2026",
                    Content = "Nội dung bài viết về sự kết hợp React và API...",
                    ImageUrl = "https://via.placeholder.com/150",
                    CreatedDate = DateTime.Now.AddDays(-1)
                },
                new Post
                {
                    Id = 3,
                    Title = "Hướng dẫn cài đặt môi trường Visual Studio",
                    Content = "Các bước cài đặt công cụ cần thiết cho lập trình viên...",
                    ImageUrl = "https://via.placeholder.com/150",
                    CreatedDate = DateTime.Now.AddDays(-2)
                }
            };

            // 2. Gửi danh sách dữ liệu sang View
            return View(posts);
        }

        // Hàm Details: Hiển thị chi tiết một bài viết (Bổ sung  khá giỏi)
        public IActionResult Details(int id)
        {
            // Giả lập tìm bài viết trong Database bằng Id
            // Trong thực tế tuần sau sẽ là: _context.Posts.Find(id);
            var post = new Post
            {
                Id = id,
                Title = "Nội dung chi tiết bài viết số " + id,
                Content = "Đây là nội dung đầy đủ của bài viết mà bạn vừa click vào. Ở đây  có thể viết dài hơn để thấy sự khác biệt với trang danh sách.",
                ImageUrl = "https://via.placeholder.com/600x300", // Ảnh to hơn
                CreatedDate = DateTime.Now
            };

            if (post == null) return NotFound();

            return View(post);
        }
    }
}
>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3
