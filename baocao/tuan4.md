Tổng quan về List:

- List là một Generic Collections đưa ra như một sự thay thế cho ArrayList, vì vậy khái niệm
và cách sử dụng của List hoàn toàn giống với ArrayList.

- List trong C# là một Generic Collections giúp lưu trữ và quản lý một danh sách các đối 
tượng theo kiểu mảng (truy cập vào phần tử thông qua chỉ số index).

- Cách khởi tạo List:
	1. List<O> list = new List<O>();	(O là kiểu dữ liệu của list).

	2. List<O> list = new List<O>(5);	(với 5 là kích cỡ mảng).

	3. List<O> MyList = new List<O>(list1);	(copy các phần tử của list1 sang list).


Các thuộc tính của List:

- Count: Trả về 1 số nguyên là số phần tử hiện có trong List.

- Capacity: Trả về 1 số nguyên cho biết số phần tử mà List có thể chứa (kích cỡ). Nếu số
phần tử vượt quá kích cỡ của List thì kích cỡ sẽ tự động tăng lên. Ngoài ra có thể gán 
1 kích cỡ bất kỳ cho List.


Các phương thức của List:

- Add (O value): thêm đối tượng value vào cuối List.

- AddRange(Collection ListObject): Thêm danh sách phần tử ListObject vào cuối List.

- BinarySearch(O value) Tìm kiếm đối tượng value trong List theo thuật toán tìm kiếm nhị phân.

- Clear(): Xóa tất cả phần tử trong List.

- Contains(O value): Kiểm tra xem đối tượng value có nằm trong List hay không.

- IndexOf(O value): Trả về vị trí đầu tiên xuất hiện đối tượng value trong List (không tìm 
thấy mặc định trả về -1).

- Insert(int index, O value): Chèn đối tượng value vào vị trí index của List.

- InsertRange(int index, Enumerable<O> ListObject): Chèn danh sách ListObject vào vị trí
index của List.

- LastIndexOf(O value): Trả về vị trí xuất hiện cuối cùng của đối tượng value trong List
(không tìm thấy mặc định trả về -1).

- Remove(O value): Xóa đối tượng value xuất hiện đầu tiên trong List.


- Reverse(): Đảo ngược tất cả phần tử trong List.

- Sost(): Sắp xếp các phần tử trong List theo thứ tự tăng dần.





Tổng quan về Dictionary:

- Dictionary là sự thay thế cho Collections HashTable.

- Dictionary trong C# là một Collection lưu trữ dữ liệu dưới dạng cặp Key - Value. Key đại 
diện cho 1 khóa giống như chỉ số index của List và Value chính là giá trị tương ứng của 
khóa đó. Ta sẽ sử dụng Key để truy cập tới Value tương ứng.

- Cách khởi tạo:
	1. Dictionary<string, string> table = new Dictionary<string, string>();

	2. Dictionary<string, string> table = new Dictionary<string, string>(4);
	(4 là sức chứa của dictionary).

	3. Dictionary<string, string> table = new Dictionary<string, string>(table1);
	(khởi tạo table với dữ liệu sao chép từ table1).


Các thuộc tính của Dictionary:

- Count: Trả về số phần tử hiện có trong Dic.

- Keys: Trả về danh sách chứa các Key trong Dic.

- Values: Trả về danh sách chứa các Value trong Dic.


Các phương thức của Dictionary:

- Add(TKey key, TValue value): Thêm 1 cặp Key-Value vào Dic.

- Clear(): Xóa tất cả các phần tử trong Dic.

- ContainsKey(TKey Key): Kiểm tra đối tượng Key có nằm trong Dic hay không.

- ContainsValue(TValue value): Kiểm tra đối tượng Value có nằm trong Dic hay không.

- Remove(TKey key): Xóa đối tượng có Key xuất hiện đầu tiên trong Dic.

- TryGetValue(TKey key, TValue value): Kiểm tra Key có tổn tại hay không. Nếu có trả về
true và đồng thời trả về giá trị value tương ứng. Ngược lại trả về false






