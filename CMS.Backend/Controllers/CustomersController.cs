using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize]
    [Route("Customer/[action]")]
    [Route("Customers/[action]")] // ĐÃ THÊM: Đồng bộ bảo vệ định tuyến tránh lỗi gõ link lệch chữ 's'
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TRANG DANH SÁCH KHÁCH HÀNG (Truy cập: /Customer hoặc /Customers)
        [Route("~/Customer")]
        [Route("~/Customers")] // ĐÃ THÊM
        public IActionResult Index()
        {
            if (_context.Customers == null)
            {
                return Content("Lỗi: Không tìm thấy bảng Customers trong cơ sở dữ liệu.");
            }

            var customers = _context.Customers.ToList();
            return View(customers);
        }

        // 2. TRANG CHI TIẾT KHÁCH HÀNG (Truy cập: /Customer/Details/1)
        [HttpGet("{id?}")]
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = _context.Customers.FirstOrDefault(m => m.Id == id);
            if (customer == null) return NotFound();

            // Hệ thống sẽ tự tìm chính xác file Details.cshtml (có chữ s) theo tên của Action
            return View(customer);
        }

        // 3. TRANG FORM THÊM MỚI KHÁCH HÀNG (Truy cập: /Customer/Create)
        public IActionResult Create()
        {
            return View();
        }

        // 4. XỬ LÝ LƯU THÊM MỚI KHÁCH HÀNG (POST: /Customer/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            ModelState.Remove("Orders");
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi Validation Form: " + error.ErrorMessage);
            }

            return View(customer);
        }

        // 5. TRANG FORM SỬA THÔNG TIN KHÁCH HÀNG (Truy cập: /Customer/Edit/1)
        [HttpGet("{id?}")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // 6. XỬ LÝ LƯU CẬP NHẬT KHÁCH HÀNG (POST: /Customer/Edit/1)
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.Id) return NotFound();

            ModelState.Remove("Orders");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customers.Any(e => e.Id == customer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // 7. CHỨC NĂNG XÓA KHÁCH HÀNG (Truy cập: /Customer/Delete/1)
        [HttpGet("{id?}")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            try
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content("Lỗi khi xóa khách hàng: " + ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}