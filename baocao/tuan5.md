Tổng quan về Delegate 
Delegate trong C# tương tự như con trỏ hàm trong C hoặc C++.
Delegate là một biến kiểu tham chiếu(references) chứa tham chiếu tới một phương thức.
Tham chiếu của Delegate có thể thay đổi runtime (khi chương trình đang thực thi).
Delegate thường được dùng để triển khai các phương thức hoặc sự kiện call-back.
Bạn cứ hiểu Delegate là một biến bình thường, biến này chứa hàm mà bạn cần gọi. 
Sau này lôi ra sài như hàm bình thường. Giá trị của biến Delegate lúc này là tham chiếu đến hàm. 
Có thể thay đổi runtime khi chương trình đang chạy.
Delegate được dẫn xuất từ lớp System.Delegate trong C#.
________________________________________
Khai báo Delegate trong C#
Khai báo Delegate trong C# sẽ tương tự như khai báo một biến. 
Nhưng cần thêm từ khóa Delegate để xác định đây là một Delegate. 
Đồng thời vì Delegate là để tham chiếu đến một hàm, 
nên cũng cần khai báo kèm kiểu dữ liệu trả về của và tham số đầu vào của Delegate tương ứng với hàm tham chiếu.
Công thức:
delegate <kiểu trả về> <tên delegate> (<danh sách tha số nếu có>);
Ví dụ:
delegate int MyDelegate(string s);
1
Lưu ý: Chữ delegate viết thường
Lúc này chúng ta đã tạo một Delegate có tên là MyDelegate. MyDelegate có kiểu trả về là int, một tham số đầu vào là string.
MyDelegate lúc này có thể dùng làm kiểu dữ liệu cho mọi Delegate tới hàm tương ứng kiểu trả về và tham số đầu vào.          
________________________________________
Khởi tạo và sử dụng Delegate trong C#
Khi kiểu Delegate được khai báo, đối tượng Delegate phải được tạo với từ khóa new và được tham chiếu đến một phương thức cụ thể. 
Phương thức này phải cùng kiểu trả về và tham số đầu vào với Delegate đã tạo.
Khi tạo một Delegate, tham số được truyền với biểu thức new được viết tương tự như một lời gọi phương thức,
nhưng không có tham số tới phương thức đó. Tức là chỉ truyền tên hàm vào thôi. Delegate sẽ tự nhận định 
hàm được đưa vào có cùng kiểu dữ liệu trả ra và cùng tham số đầu vào hay không.

Multicast(đa hướng) một Delegate trong C#
Khi bạn cần thực hiện một chuỗi hàm với cùng kiểu trả về và cùng tham số đầu vào mà không muốn gọi nhiều hàm tuần tự 
(chỉ gọi 1 hàm 1 lần duy nhất). Lúc này bạn sẽ cần dùng đến Multicast Delegate.
Bản chất bạn có thể làm một chuỗi Delegate cùng kiểu Delegate bằng cách dùng toán tử +. Lúc này khi bạn gọi 
Delegate sẽ thực hiện tuần từ các Delegate được cộng vào với nhau.
Bạn có thể loại bỏ Delegate trong multicast bằng toán tử -.


