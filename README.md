Mục tiêu: Cài đặt môi trường và hiểu cấu trúc Solution 3 lớp (Data - Backend - Frontend).
Làm những gì (Nội dung & Thực hành):
Giới thiệu tổng quan về nền tảng .NET Core, Dependency Injection cơ bản và tổ chức cấu trúc thư mục Web API.
Khởi tạo một Thùng chứa dự án trống (Blank Solution) trong Visual Studio 2022 với tên gọi ToanCMS_Solution.
Tạo Project 1 - CMS.Data thuộc kiểu thư viện lớp (Class Library .NET Core) đóng vai trò làm lớp dữ liệu. Xóa file mặc định Class1.cs và tạo thư mục con tên là Entities
Tạo Project 2 - CMS.Backend thuộc mẫu ứng dụng Web MVC (ASP.NET Core Web App (Model-View-Controller)) đóng vai trò xử lý trang Admin và API. Thực hiện kết nối tham chiếu dự án (Project Reference) từ CMS.Backend sang CMS.Data
Thiết lập dự án khởi động (Set as Startup Project) cho CMS.Backend, nhấn F5 chạy thử nghiệm trang chào mừng mặc định của ASP.NET Core.
Tạo Project 3 - cms.frontend bên ngoài bằng ReactJS thông qua công cụ dòng lệnh npx create-react-app cms.frontend trong môi trường chạy Node.js LTS. Sau đó nhúng vào Solution thông qua tính năng Add -> Existing Web Site... của Visual Studio.
Tạo CategoryController.cs trống trong CMS.Backend, viết dữ liệu "giả" (Mock Data) trực tiếp trong code và tạo Razor View Index.cshtml để hiển thị danh sách danh mục mẫu dưới dạng bảng HTML ra trình duyệt.
Phải hoàn thiện những gì (Tiêu chí nghiệm thu & Bài tập):
[x] Khởi tạo thành công cấu trúc phân tầng Solution gồm cả 3 Project (CMS.Data, CMS.Backend, cms.frontend) xuất hiện đầy đủ trong cửa sổ Solution Explorer.
Khai báo định nghĩa đầy đủ cấu trúc thuộc tính và quan hệ liên kết cho 8 lớp thực thể (Entities) cốt lõi trong thư mục Entities của project CMS.Data : Category.cs , Post.cs , User.cs , CategoryProduct.cs , Product.cs , Customer.cs , Order.cs , và OrderDetail.cs
Hoàn thành luồng hiển thị dữ liệu mẫu ban đầu qua việc thực hiện bài tập tự rèn luyện : Tạo thêm PostController.cs hiển thị danh sách bài viết mẫu và UserController.cs hiển thị danh sách thành viên hệ thống mẫu , hoàn thiện các file Razor View Index.cshtml tương ứng. Chạy ứng dụng kiểm tra tính kết nối hiển thị bảng đẹp mắt.
