# **Đồ Án Batch Rename**

## **Mục lục**
### [Thông tin nhóm](#team)
### [Môi trường cài đặt](#install)
### [Cách chạy chương trình](#run)   
### [Chức năng đã thực hiện](#done)  
### [Chức năng chưa thực hiện](#notdone)  
### [Chức năng cộng thêm](#other)
### [Số giờ làm việc](#hour)
### [Điểm số mong muốn](#score)
### [Video demo](#video)
## **Thông tin nhóm**<a name="team"></a>
***
|STT|Thành viên|MSSV
| :------: | ------ | ----- |
|1|Đỗ Thái Duy| 19120492
|2|Nguyễn Tuấn Khanh| 19120540
|3|Trương Công Thành| 19120660
## **Môi trường cài đặt**<a name="install"></a>
***
- Ngôn ngữ chính: C#.
- Framework: .NET 5.
- IDE: Visual Studio 2022.
## **Cách chạy chương trình**<a name="run"></a>
***
- Build toàn bộ solution để có được file dll của các luật (các file được load sẵn và hiển thị trên droplist không cần phải thêm vào thủ công).
- Run project ở chế độ release hay debug đều được.
## **Chức năng đã thực hiện**<a name="done"></a>
***
### **Core Requirements - Yêu cầu cốt lõi**
1. Các rule được load từ file DLL.
2. Có thể chọn File, Folder muốn đổi tên.
3. Tạo được danh sách các Rule:
    1. Các rule được thêm sẵn vào một menu.
    2. Mỗi rule có giao diện riêng để chỉnh sửa thông số.
4. Các Rule được áp dụng để đổi tên theo thứ tự từ trên xuống dưới.
5. Có thể lưu các Rule thành các preset để có thể tái sử dụng nhanh chóng.
### **Các luật cơ bản**
1. Đổi extension của file.
2. Thêm Counter vào tên file.
- Có thể tự chọn Giá trị bắt đầu, bước nhảy và số lượng chữ số của Counter.
3. Xóa khoảng trắng khỏi đầu hoặc cuối tên file:
- Người dùng có thể chọn xóa ở đầu tên file, cuối tên file hoặc cả đầu và cuối tên file.
4. Thay đổi một chuỗi kí tự thành chuỗi các kí tự khác.
5. Thêm prefix vào đầu file.
6. Thêm suffix vào cuối file.
7. Chuyển các kí tự viết hoa thành viết thường, đồng thời xóa mọi khoảng trắng.
8. Đổi tên file theo dạng PascalCase.
## **Chức năng chưa thực hiện**<a name="notdone"></a>
* * *
**Không**
## **Chức năng cộng thêm**<a name="other"></a>
* * *
### **Improvements - Tất cả improvement đều đã được thực hiện:** 
1. Kéo thả các File, Folder.
2. Lưu trữ dữ liệu bằng file JSON.
3. Chỉ cần kéo thả một folder vào tab file, thì tất cả các file của folder đó sẽ được thêm vào danh sách files.
4. Có xử lý lỗi khi tên file, folder bị trùng.
5. Khi đóng ứng dụng, chương trình sẽ tự động lưu trạng thái cuối cùng của ứng dụng:
- Kích thước cửa sổ ứng dụng.
- Vị trí cửa sổ ứng dụng trên màn hình.
- Preset hoặc danh sách các rule cuối cùng được chọn.
- Danh sách file, folder đang thao tác.
6. Có chức năng auto save lại project khi ứng dụng bị tắt đột ngột (ví dụ như mất điện):
- Kích thước cửa sổ ứng dụng.
- Vị trí cửa sổ ứng dụng trên màn hình.
- Preset hoặc danh sách các rule cuối cùng được chọn.
- Danh sách file, folder đang thao tác.
- Project được auto-save mỗi 60 giây.
7. Sử dụng Regular Expression để kiểm tra tính hợp lệ của tên file, tên folder và Regex còn được áp dụng để xử lý chuỗi trong một số Rule để kiểm tra tính hợp lệ của input từ người dùng.
8. Có chức năng kiểm tra tính hợp lệ của tên file, folder. Kiểm tra độ dài tên file/folder có vượt quá 255 kí tự và thông báo cho người dùng.
9. Có chức năng lưu trạng thái làm việc hiện tại vào các project (dưới dạng file json), người dùng có thể mở lại các project này để làm việc tiếp tục. 
10. Người dùng có thể xem trước được kết quả của việc đổi tên file, folder.
11. Ngoài chức năng đổi tên trên chính file/folder gốc, thì có thể chọn chức năng copy toàn bộ file/folder sang nơi khác rồi đổi tên.
### **Các improvements khác:**
1. Sử dụng thư viện HandyControl (https://hosseini.ninja/handycontrol/) để giao diện của ứng dụng trở nên đẹp hơn và giao diện ứng dụng được thiết kế responsive.
2. Có các chức năng như new, save, save as, open để tạo mới, lưu, tạo bản sao và mở các project. 
3. Phần giao diện danh sách file/folder có cột Status để hiển thị trạng thái của file/folder sau khi đổi tên (thành công, thất bại).
4. Hiển thị tổng sổ file/folder được rename thành công, rename thất bại, tổng số file và folder đang xử lí.

## **Số giờ làm việc** <a name="hour"></a>
***
Tụi em quản lý công việc qua ứng dụng JIRA với mỗi chức năng có tổng thời gian làm việc của các thành viên là: <br/>
| Công việc | Số giờ |
| --------- | :-----: |
| Core Requirements - 1 | 20 |
| Core Requirements - 2 | 8 |
| Core Requirements - 3 | 6 |
| Core Requirements - 4 | 2 |
| Core Requirements - 5 | 14 |
| Improvements - 1 | 3.5 |
| Improvements - 2 | 5 |
| Improvements - 3 | 5.5 |
| Improvements - 4 | 0.5 |
| Improvements - 5 | 2 |
| Improvements - 6 | 2 |
| Improvements - 7 | 4.5 |
| Improvements - 8 | 5 |
| Improvements - 9 | 5 |
| Improvements - 10 | 8 |
| Improvements - 11 | 6 |
| Improvements - Khác | 3 |
| **SUM** | **100** |

Mỗi thành viên đều có thời gian làm việc trung bình từ 32 đến 34 giờ tùy theo chức năng được phân công.

## **Điểm số mong muốn** <a name="score"></a>
* * *
|STT|Thành viên|MSSV|Điểm mong muốn
| :------: | ------ | ----- | :-----: |
|1|Đỗ Thái Duy| 19120492 | 10
|2|Nguyễn Tuấn Khanh| 19120540 | 10
|3|Trương Công Thành| 19120660 | 10
## **Video demo** <a name="video"></a>
***
Link video: 