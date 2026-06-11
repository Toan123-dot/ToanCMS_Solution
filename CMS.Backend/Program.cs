<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using CMS.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Linq;


// Đăng ký DbContext vào hệ thống

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

=======
var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 🌟 Đfont ĐÃ SỬA: Tự động kích hoạt bộ lọc ẩn các Controller MVC giao diện cũ
// ====================================================================
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new HideMvcFromSwaggerConvention());
});

// ====================================================================
// 1. Đboundary ĐĂNG KÝ SWAGGER (Trả về cấu hình gốc, an toàn tuyệt đối)
// ====================================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// ====================================================================
// 2. ĐĂNG KÝ CORS (Cho phép ReactJS ở cổng 3000 gọi lấy dữ liệu)
// ====================================================================
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Đăng ký DbContext kết nối SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký dịch vụ Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

>>>>>>> 9d1cf64c2e342b9c76b4a45141f15c77390774b3
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ====================================================================
// 3. KÍCH HOẠT MIDDLEWARE SWAGGER
// ====================================================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API V1");
});

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ====================================================================
// 🌟 LỚP BỘ LỌC TỰ ĐỘNG KHÓA MVC (Bắt buộc phải đặt ở cuối cùng file Program.cs)
// ====================================================================
public class HideMvcFromSwaggerConvention : Microsoft.AspNetCore.Mvc.ApplicationModels.IControllerModelConvention
{
    public void Apply(Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel controller)
    {
        // Nếu Controller kế thừa từ lớp Controller (Giao diện Admin quản trị HTML cũ) thì ẩn khỏi Swagger
        if (typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(controller.ControllerType))
        {
            controller.ApiExplorer.IsVisible = false;
        }
    }
}
app.Run();
