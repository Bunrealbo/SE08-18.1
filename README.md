# SE08-18.1
Nhóm 18.1: Lập trình game đơn giản thể loại Match 3 với Unity

Công việc chung:
- Tìm hiểu logic Match3 của game (nối 3 kẹo thì biến mất, 4 & 5 kẹo tạo ra item bonus).

- Tìm cơ chế tính điểm của game, các hàm gọi item bonus (rocket, bomb,...).

- Các thành phần của design.

- Cơ chế giao tiếp với server - REST API.



Phân công công việc cá nhân:

- Phục hồi phần Text ở màn hình chính - Tùng Dương
	+) Hiển thị lại những text đã bị ẩn (stars, coins, lives).
	+) Đổi màu text cho phù hợp với thuộc tính.
	+) Đổi một số hình ảnh của phần thiết kế.
	+) Đổi biểu tượng icon của chức năng (icon setting).


- Phục hồi phần Text trong màn hình chơi game - Tuấn Anh
	+) Hiển thị điểm cập nhật theo từng bước đi.
	+) Hiển thị số level, chỉ tiêu cần đạt được để chiến thắng của từng level tương ứng.
	+) Thêm một số hình ảnh tùy chỉnh.

- Kết nối tới Facebook - Anh Quân
	+) Sử dụng thư viện Unity.Facebook.
	+) Xây dựng cơ chế đăng nhập theo Client - Server.
	+) Khi đăng nhập, mỗi tài khoản Facebook sẽ có một token request tương ứng.
	+) Nhà phát triển cần xác nhận (send success) để người chơi hoàn tất thủ tục đăng nhập.

- Cách thức kết nối tới server và giao tiếp với server - Thùy Dung
	+) Kết nối tới server thông qua Docker API.
	+) Giao tiếp với server bằng Postman.
	+) Sử dụng cac hàm GET, POST với các dữ liệu kiểu JSON.

- Đẩy điểm lên server sau khi hoàn thành màn chơi - Xuân Anh
	+) Để được tính điểm đẩy lên server, người chơi cần phải đăng nhập Facebook.
	+) Xác định biến tính điểm sau khi hoàn thành (thắng màn chơi)
	+) Đẩy điểm lên server với lệnh POST,  file JSON gồm thuộc tính ID - Level - Score 
	(ID là unique và được lấy từ ID Facebook).

- Gửi request điểm để lấy điểm từ server về - Anh Quân
	+) Lấy dữ liệu theo UID.
	+) Lấy dữ liệu của tất cả người chơi trong hệ thống.
	+) Lấy dữ liệu từ danh sách UID.



Các mục tiêu có thể bổ sung:

- Sau khi lấy điểm của các người chơi về có thể tạo bảng xếp hạng.

- Tối ưu thêm phần giao diện.

- Thêm các cảnh trang trí để đa dạng cho người dùng lựa chọn.


