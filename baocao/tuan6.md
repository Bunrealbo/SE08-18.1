Tổng quan về Event:
- **Event** (sự kiện) là các hành động của người dùng, VD: nhấn phím, click, di chuyển chuột,...
- Các ứng dụng cần phản hồi các sự kiện này khi chúng xuất hiện, các sự kiện được sử dụng để giao tiếp bên trong tiến trình.

Sử dụng Delegate với Event:
- Các Event được khai báo và được tạo trong một lớp và được liên kết với Event Handler bởi sử dụng các Delegate trong cùng lớp đó hoặc một số lớp khác.
- Lớp mà chứa Event được sử dụng để công bố event đó được gọi là **Publisher**.
  - Một Publisher là một đối tượng mà chứa định nghĩa của Event và Delegate đó.
  - Mối liên hệ Event-Delegate cũng được định nghĩa trong đối tượng này.
  - Một đối tượng Publisher gọi Event và nó được thông báo tới các đối tượng khác.
- Lớp khác mà chấp nhận Event ngoài được gọi là **Subscriber**.
  - Một Subsciber là một đối tượng mà chấp nhận Event ngoài và cung cấp Event Handler.
  - Delegate trong lớp Publisher gọi đến Event Handler của lớp Subcriber.
- Các Event trong C# sử dụng mô hình **Publisher-Subscriber**.

Khai báo Event trong C#:
- Trước khi khai báo một Event, ta phải khai báo một kiểu Delegate cho Event đó. VD: 
```C#
public delegate void NewHandler(string s);
```
- Sau đó, ta khai báo Event, sử dụng từ khóa **event** trong C#. VD:
```C#
// Định nghĩa Event dựa vào Delegate đã khai báo ở trên
public event NewHandler NewEvent;
```

Tổng quan về Phương thức nặc danh:
- **Phương thức nặc danh (Anonymous Method)** cung cấp một kỹ thuật để truyền một khối code như là một tham số delegate.
- Là phương thức không có tên mà chỉ có phần thân phương thức.
- Ta không cần xác định kiểu trả về trong phương thức nặc danh, nó được suy ra từ lệnh return bên trong phần thân của phương thức nặc danh đó.

Viết một Phương thức nặc danh trong C#:
- Các phương thức nặc danh được khai báo cùng với việc tạo instance của delegate đó, với từ khóa **delegate**. VD:
```C#
delegate void NewDelegate(string s);
// ...
NewDelegate nd = delegate(string name)
{
	Console.WriteLine("Hello " + name);
};
```
