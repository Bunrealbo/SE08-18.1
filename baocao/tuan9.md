Kết nối tới Docker API:

- Để pull một image, thông thường chúng ta cần lệnh docker pull, nhưng thay vì gõ docker 
pull chúng ta có thể dùng WGET hay CURL để gọi đến api pull image.
VD: curl --unix-socket /var/run/docker.sock \
  -X POST "http:/v1.24/images/create?fromImage=alpine:latest"

- Sau khi chạy chạy lệnh trên, kiểm tra lại bằng lệnh docker images chúng ta sẽ thấy 
pull về được image alpine, tương tự như việc gõ docker pull alpine:lastest.

-  Docker server có thể lắng nghe qua unix socket, TCP và UDP. Nhưng mặc định, docker server
sẽ lắng nghe qua unix socket. Gọi qua unix socket thì phù hợp với việc gọi local. Tức là gọi
đến docker server trên cùng một máy, còn nếu muốn gọi qua từ bên ngoài thì phải thì buộc phải
expose và cấu hình lại đôi chút để có thể gọi qua TCP IP.


- trong trường hợp bị lỗi permission denied docker.sock, chúng ta cần cấp quyền đọc ghi cho 
file docker.sock.



Test API thông qua Postman:

- Một số chức năng của Postman:
	+) New – Là nơi bạn sẽ tạo request, collection hoặc enviroment mới.
	+) Import – Được sử dụng để import collection hoặc environment. Có các tuỳ chọn để 
	import từ file, folder, link hoặc paste từ text thuần.
	+) Open New – Mở một tab mới, cửa sổ Postman hoặc cửa sổ Runner bằng việc kích trên nút này.
	+) My Workspace – Bạn có thể tạo sổ làm việc riêng hoặc như cho một nhóm.
	+) HTTP Request – Click vào đây sẽ hiển thị danh sách thả xuống với các request khác
 	nhau như GET, POST, COPY, DELETE, v.v. Trong thử nghiệm, các yêu cầu được sử dụng 
	phổ biến nhất là GET và POST.
	+) Request URL – Còn được gọi là điểm cuối (endpoint), đây là nơi bạn sẽ xác định 
	liên kết đến nơi API sẽ giao tiếp.
	+)Tests – Đây là các script được thực thi khi request. Điều quan trọng là phải có
	các thử nghiệm như thiết lập các điểm checkpoint để kiểm tra trạng thái là ok, 
	dữ liêu nhận được có như mong đợi không và các thử nghiệm khác.

- Làm việc với Request GET:
	+) Thiết lập request HTTP của bạn là GET.
	+) Trong trường URL yêu cầu, nhập vào link.
	+) Kích nút Send.
	+) Bạn sẽ nhìn thấy message OK.
	+) Sẽ hiển thị kết quả 10 bản ghi trong phần Body của bạn.

	Chú ý: Có thể có nhiều trường hợp request GET không thành công. Nó có thể thể do URL của 
	request không hợp lệ hoặc do chứng thực không thành công (authentication).

- Làm việc với Request POST:
	+) Kích dấu + để thêm mới một tab cho request mới.
	+) Trong tab mới:
		* Thiết lập request HTTP là POST.
		* Nhập vào link với url của API.
		* Chuyển tới tab Body.
	+) Trong tab Body,
		* Kích chọn raw.
		* Chọn JSON.
	+)  Copy và paste chỉ một user từ kết quả request trước. Đảm bảo rằng mã đã được 
	sao chép chính xác với các dấu đóng mở. Thay đổi id thành 11 và đặt name bất kỳ tên 
	nào bạn muốn. Bạn cũng có thể thay đổi các trường khác như address.

	Chú ý: Request POST nên đúng định dạng để đảm bảo dữ liệu được yêu cầu sẽ được tạo.

	+) Tiếp theo,
		* Kích nút Send.
		* Status: 201 Created được hiển thị
		* Dữ liệu Post được hiển thị trong tab Body.



