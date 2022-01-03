Thêm Firebase vào trong Unity Project:

B1: Tạo một project Firebase
  Để thêm được Firebase vào trong Unity project, trước hết ta cần tạo một Firebase project
để kết nối tới Unity project của mình.


B2: Đăng ký app với Firebase
  - Đi tới Firebase console.
  - Trong phần trung tâm của trang tổng quan trong project, click vào biểu tượng Unity để
chạy phần thiết lập quy trình làm việc.
(Nếu đã hoàn thành việc thêm 1 app vào Firebase project, nhấn vào Add app để hiện ra
nền tảng cài đặt).
  - Chọn mục tiêu xâu dựng của Unity project mà bạn muốn đăng ký, hoặc thậm chí có thể 
chọn đăng ký cả 2 mục tiêu cùng một lúc.
  - Chọn nền tảng cho Unity project 
	+) IOS: nhập ID của Unity project trong trường IOS bundle ID.
	+) Android: Nhập ID của Unity proect trong trường Android package name.
Tên gói và ID của ứng dụng thườn được dùng để thay thế cho nhau.
  - (Có thể lựa chọn) Thêm nickname của Unity project.
  - Nhấn Register app.

B3: Thêm tập tin cấu hình cho Firebase 
  - Tải về tệp cấu hình Firebase dành cho nền tảng tương ứng
	+) IOS: Nhấn Dowload GoogleService-Info.plist.
	+) Android: Nhấn Dowload google-services.json.
  - Mở cửa sổ project của Unity, sau đó chuyển tập tin cấu hình tới thư mục Assets.
  - Trở lại Firebase console, trong phần setup chọn Next.

B4: Thêm Firebase Unity SDKs.
  - Trong Firebase console, nhấn Dowload Firebase Unity SDK, sau đó giải nén SDK ở đường dẫn 
phù hợp
  - Trong Unity project, truy cập theo đường dẫn Assets > Import Package > Custom Package
  - Trong file giải nén SDK, chọn những dịch vụ được Firebase hỗ trợ mà bạn muốn đưa vào
trong app của mình.