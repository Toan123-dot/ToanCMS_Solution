using CMS.Data; // Thừa hưởng kết nối Database giống file MVC của bạn

using CMS.Data.Entities; // Nhận diện bảng dữ liệu Category

using Microsoft.AspNetCore.Mvc;

using System.Linq;



namespace CMS.Backend.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class CategoriesController : ControllerBase

    {

        // Sử dụng chính xác tên lớp cơ sở dữ liệu ApplicationDbContext từ file MVC của bạn

        private readonly ApplicationDbContext _context;



        public CategoriesController(ApplicationDbContext context)

        {

            _context = context;

        }



        // ==========================================

        // 1. API LẤY TOÀN BỘ DANH MỤC (GET: api/Categories)

        // Phục vụ hiển thị menu danh mục trên ReactJS

        // ==========================================

        [HttpGet]

        public IActionResult GetAll()

        {

            var categories = _context.Categories.ToList();



            // Trả về mã Http Status Code 200 OK kèm chuỗi dữ liệu JSON

            return Ok(categories);

        }



        // ==========================================

        // 2. API LẤY CHI TIẾT 1 DANH MỤC (GET: api/Categories/{id})

        // Phục vụ xem thông tin cụ thể của danh mục theo Id

        // ==========================================

        [HttpGet("{id}")]

        public IActionResult GetDetail(int id)

        {

            var category = _context.Categories.Find(id);



            if (category == null)

            {

                // Trả về mã Http Status Code 404 nếu truyền sai Id không có trong DB

                return NotFound();

            }



            return Ok(category);

        }

    }

}