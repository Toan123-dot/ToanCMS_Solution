using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data; // Thư mục chứa ApplicationDbContext của bạn
using System.Linq;

namespace CMS.Backend.Controllers
{
    // 1. Định nghĩa đường dẫn để gọi API. [controller] sẽ tự lấy tên "Posts"
    // Khi chạy, địa chỉ sẽ là: https://localhost:xxxx/api/posts
    [Route("api/[controller]")]

    // 2. Đánh dấu đây là một API Controller để hệ thống hỗ trợ các tính năng RESTful
    [ApiController]

    // 3. API Controller phải kế thừa từ ControllerBase (thay vì Controller như MVC)
    public class PostsController : ControllerBase
    {
        // 4. Khai báo biến kết nối Database
        private readonly ApplicationDbContext _context;

        // 5. Hàm khởi tạo (Constructor): "Tiêm" kết nối Database vào để sử dụng
        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // API 1: Lấy toàn bộ danh sách bài viết (GET: api/posts)
        // ====================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            // Lấy dữ liệu từ bảng Posts
            var posts = _context.Posts
                .OrderByDescending(p => p.Id) // Sắp xếp bài mới nhất lên đầu
                .Select(p => new {            // "Gọt tỉa" dữ liệu: chỉ lấy những trường cần thiết
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate,            // Đã sửa từ CreatedAt thành CreatedDate cho đúng thuộc tính của lớp Post
                    CategoryName = p.Category.Name // Lấy tên danh mục thay vì chỉ lấy ID
                })
                .ToList();

            // Trả về kết quả cho Frontend kèm mã trạng thái 200 (Thành công)
            return Ok(posts);
        }

        // ====================================================================
        // API 2: Lọc danh sách bài viết theo danh mục (GET: api/posts/category/{categoryId})
        // ====================================================================
        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            // Lọc các bài viết có CategoryId trùng với ID truyền vào từ URL
            var posts = _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new {
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate             // Đã sửa từ CreatedAt thành CreatedDate cho đồng bộ
                })
                .ToList();

            return Ok(posts);
        }

        // ====================================================================
        // API 3: Lấy chi tiết nội dung của một bài viết (GET: api/posts/{id})
        // ====================================================================
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            // Tìm bài viết đầu tiên có Id khớp với tham số truyền vào
            var post = _context.Posts
                .FirstOrDefault(p => p.Id == id);

            // Xử lý trường hợp không tìm thấy (ID không tồn tại)
            if (post == null)
            {
                // Trả về lỗi 404 kèm thông báo dưới dạng JSON
                return NotFound(new { message = "Không tìm thấy bài viết này trong hệ thống" });
            }

            // Trả về bài viết tìm thấy kèm mã 200 (Thành công)
            return Ok(post);
        }

    } // Kết thúc class PostsController (Hợp lệ: Hàm GetDetail đã nằm trước dấu đóng này)
} // Kết thúc namespace