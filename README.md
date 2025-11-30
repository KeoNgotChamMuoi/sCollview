Developed by KeoNgotCutie
# 🔫 Gun Inventory System (Unity + Spring Boot)

Dự án demo hệ thống quản lý kho vũ khí trong game (Inventory) với mô hình Client-Server.
Dự án bao gồm 2 phần chính:
1.  **Server (Backend):** Viết bằng Java Spring Boot, cung cấp RESTful API.
2.  **Client (Frontend):** Viết bằng Unity (C#), hiển thị UI và gọi API.

---

## 📂 Cấu trúc dự án
```text
ScrollViewxSpringboot/
├── gungame/            # Source code Backend (Spring Boot)
├── scrollview/         # Source code Frontend (Unity Project)
└── README.md           # File hướng dẫn này
```
🛠 Yêu cầu cài đặt (Prerequisites)
Để chạy được dự án, máy tính cần cài sẵn:
-   Java Development Kit (JDK): Phiên bản 17 trở lên.
-   Maven: (Thường đã tích hợp sẵn trong IntelliJ IDEA).
-   Unity Hub & Editor: Phiên bản 2021.3 (LTS) hoặc mới hơn.
-   Postman: (Tùy chọn) Để test API thủ công.
-   IDE: IntelliJ IDEA (khuyên dùng cho Java) và Visual Studio/VS Code (cho C#).

🚀 Hướng dẫn chạy Server (Backend)
Server cần được khởi động trước để Game có thể kết nối.
-   Mở thư mục GunGame_Server bằng IntelliJ IDEA.
-   Đợi Maven tải các thư viện cần thiết (Spring Web).
-   Tìm file chính: src/main/java/com/example/gungame/GungameApplication.java.
-   Nhấn nút Run (Play màu xanh).
-   Khi thấy dòng log bên dưới hiện: Tomcat started on port 8080 là thành công.

Lưu ý:
Server chạy mặc định tại: http://localhost:8080
Dữ liệu súng được lưu tạm trên RAM (In-Memory List), tắt server sẽ mất dữ liệu và reset về mặc định.

🎮 Hướng dẫn chạy Client (Unity)
-   Mở Unity Hub.
-   Chọn Add (hoặc Open) -> Trỏ đến thư mục GunGame_Client.
-   Mở Scene chính (thường trong Assets/Scenes/SampleScene.unity).
-   Đảm bảo Server (Bước trên) đang chạy.
-   Nhấn nút Play trên Unity Editor.

### 📡 Danh sách API (API Documentation)
| Method | Endpoint | Mô tả | Body mẫu (JSON) |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/guns` | Lấy danh sách súng | _N/A_ |
| `POST` | `/api/guns` | Thêm súng mới | `{"gunName": "AK47", "level": 1, "damage": 30}` |
| `PUT` | `/api/guns/{id}` | Update level/damage | `{"level": 5, "damage": 120}` |
| `DELETE`| `/api/guns/{id}` | Xóa súng | _N/A_ |

Database: Hiện tại đang dùng List<Gun> trên RAM. Tắt server sẽ mất dữ liệu (Reset).
