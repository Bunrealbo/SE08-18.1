Tổng quan về Collection trong C#:

- Các lớp hỗ trợ lưu trữ, quản lý và thao tác với các đối tượng một cách có thứ tự.

- Các lớp này nằm trong namespace System.Collections.


Đặc điểm của Colletions:

- Là mảng kích thước động (không cần khai báo kích thước khi khởi tạo, có thể tăng giảm số 
lượng phần tử linh hoạt.

- Có thể lưu trữ một tập hợp đối tượng thuộc nhiều kiểu khác nhau.

- Hỗ trợ nhiều phương thức để thao tác với tập hợp: tìm kiếm, sắp xếp, đảo ngược.

- Mỗi Collection được tổ chức thành một lớp nên cần khởi tạo trước khi sử dụng.


Một số Collections thông dụng:

- ArrayList:  Lớp cho phép lưu trữ và quản lý các phần tử giống mảng. Tuy nhiên ta có thể
thêm hoặc xóa phần tử một cách linh hoạt và có thể tự điều chỉnh kích cỡ tự động.

- HashTable: Lớp lưu trữ dữ liệu dưới dạng cặp Key-Value. Khi đó ta sẽ truy xuất các phần tử
trong danh sách này thông qua Key (thay vì chỉ số phần tử).

- SortedList: Là sự kết hợp của ArrayList và HashTable, tức là dữ liệu sẽ vẫn được lưu dưới
dạng Key-Value. Ta có thể truy xuất các phần tử trong danh sách thông qua Key hoặc thông qua 
chỉ số phần tử. Đặc biệt là các phần tử trong danh sách này luôn được sắp xếp theo giá trị 
của Key.

- Stack: Lớp cho phép lưu trữ và thao tác dữ liệu theo cấu trúc LIFO (Last In First Out).

- Queue: Lớp cho phép lưu trữ và thao tác dữ liệu theo cấu trúc FIFO (First In First Out).

- BitArray: Lớp cho phép lưu trữ và quản lý một danh sách các bit (giống mảng phần tử kiểu 
Bool với true biểu thị bit 1 và bit 0 biểu thị cho bit 0). BitArray hỗ trợ một số phương thức
cho việc tính toán trên bit.


Tổng quan về Generic trong C#:

- Generic trong C# cho phép ta định nghĩa một hàm, một lớp mà không cần chỉ ra đối số kiểu 
dữ liệu là gì. Tùy vào kiểu dữ liệu mà người dùng truyền vào thì nó sẽ hoạt động theo kiểu đó

VD: public static void Swap<T> (ref T a, ref T b)
{
	T temp = a;
	a = b;
	b = temp;
}

Khi sử dụng ta chỉ cần gọi đến kiểu dữ liệu mà ta cần
Swap<int>(ref a, ref b);
Swap<double>(ref c, ref d);

- Tương tự Generic cũng có thể áp dụng cho lớp 

VD: public class FirstGeneric<T>
{
	private T[] list;
	...
}


Đặc điểm của Generic:

- Generic Collection có thể giúp ta sử dụng được các Collections mà không bị thêm vào các
phần tử không mong muốn (vì Collection thông thường sẽ có kiểu dữ liệu là Object, có thể
thêm các phần tử có kiểu dữ liệu không mong muốn).

-  Generic còn giúp hạn chế truy cập nếu không truyền đúng kiểu dữ liệu.




