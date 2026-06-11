using CMS.Data;           // Đảm bảo đúng kết nối ApplicationDbContext của bạn

using CMS.Data.Entities;  // Nhận diện thực thể User

using Microsoft.AspNetCore.Mvc;

using System.Linq;



namespace CMS.Backend.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class UsersController : ControllerBase

    {

        private readonly ApplicationDbContext _context;



        public UsersController(ApplicationDbContext context)

        {

            _context = context;

        }



        // =======================================================

        // 1. API LẤY DANH SÁCH USER (GET: api/Users)

        // LƯU Ý BẢO MẬT: Ẩn cột mật khẩu (PasswordHash) khi trả về Frontend

        // =======================================================

        [HttpGet]

        public IActionResult GetAll()

        {

            var users = _context.Users

                                .Select(u => new {

                                    u.Id,

                                    u.Username,

                                    u.FullName,

                                    u.Role

                                })

                                .ToList();



            return Ok(users); // Trả về HTTP Status Code 200 OK kèm dữ liệu JSON

        }



        // =======================================================

        // 2. API LẤY CHI TIẾT 1 USER (GET: api/Users/{id})

        // =======================================================

        [HttpGet("{id}")]

        public IActionResult GetDetail(int id)

        {

            var user = _context.Users.Find(id);



            if (user == null)

            {

                return NotFound(); // Trả về mã 404 nếu truyền sai Id không có trong DB

            }



            // Trả về đối tượng an toàn không kèm mật khẩu

            return Ok(new

            {

                user.Id,

                user.Username,

                user.FullName,

                user.Role

            });

        }



        // =======================================================

        // 3. API TẠO MỚI USER (POST: api/Users)

        // =======================================================

        [HttpPost]

        public IActionResult Create([FromBody] User model)

        {

            // Kiểm tra trùng tên đăng nhập giống hệt logic MVC của bạn

            var checkExist = _context.Users.Any(u => u.Username == model.Username);

            if (checkExist)

            {

                return BadRequest(new { message = "Tên đăng nhập này đã có người dùng!" }); // Trả về mã 400 lỗi nhập liệu

            }



            _context.Users.Add(model);

            _context.SaveChanges();



            // Trả về mã 201 Created báo hiệu tạo dữ liệu thành công

            return CreatedAtAction(nameof(GetDetail), new { id = model.Id }, model);

        }



        // =======================================================

        // 4. API XÓA USER (DELETE: api/Users/{id})

        // =======================================================

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)

        {

            var user = _context.Users.Find(id);

            if (user == null)

            {

                return NotFound();

            }



            _context.Users.Remove(user);

            _context.SaveChanges();



            return Ok(new { message = "Xóa tài khoản thành công!" });

        }

    }

}