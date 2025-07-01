-- Kết nối đến master database
USE master;
GO

-- Xóa database nếu đã tồn tại
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'TrafficViolationDB')
    DROP DATABASE TrafficViolationDB;
GO

-- Tạo database mới
CREATE DATABASE TrafficViolationDB;
GO

USE TrafficViolationDB;
GO

-- Tạo bảng citizens
CREATE TABLE [dbo].[citizens] (
    [id] bigint primary key IDENTITY(1,1),
    [name] nvarchar(255) not null,
    [email] nvarchar(255) unique not null,
    [phone] nvarchar(50),
    [address] nvarchar(255)
);

-- Tạo bảng vehicles
CREATE TABLE [dbo].[vehicles] (
    [id] bigint primary key IDENTITY(1,1),
    [owner_id] bigint not null,
    [license_plate] nvarchar(50) unique not null,
    [make] nvarchar(100),
    [model] nvarchar(100),
    [year] int,
    FOREIGN KEY (owner_id) REFERENCES [dbo].[citizens](id)
);

-- Tạo bảng violation_types
CREATE TABLE [dbo].[violation_types] (
    [id] bigint primary key IDENTITY(1,1),
    [name] nvarchar(100) not null,
    [description] nvarchar(255)
);

-- Tạo bảng report_statuses
CREATE TABLE [dbo].[report_statuses] (
    [id] bigint primary key IDENTITY(1,1),
    [status] nvarchar(50) not null
);

-- Tạo bảng violation_reports
CREATE TABLE [dbo].[violation_reports] (
    [id] bigint primary key IDENTITY(1,1),
    [citizen_id] bigint not null,
    [vehicle_id] bigint not null,
    [violation_type_id] bigint not null,
    [status_id] bigint not null,
    [report_date] datetimeoffset default SYSDATETIMEOFFSET(),
    [location] nvarchar(255),
    [description] nvarchar(255),
    FOREIGN KEY (citizen_id) REFERENCES [dbo].[citizens](id),
    FOREIGN KEY (vehicle_id) REFERENCES [dbo].[vehicles](id),
    FOREIGN KEY (violation_type_id) REFERENCES [dbo].[violation_types](id),
    FOREIGN KEY (status_id) REFERENCES [dbo].[report_statuses](id)
);

-- Tạo bảng user_roles
CREATE TABLE [dbo].[user_roles] (
    [id] bigint primary key IDENTITY(1,1),
    [role_name] nvarchar(50) unique not null
);

-- Tạo bảng users
CREATE TABLE [dbo].[users] (
    [id] bigint primary key IDENTITY(1,1),
    [citizen_id] bigint,
    [role_id] bigint not null,
    [username] nvarchar(100) unique not null,
    [password_hash] nvarchar(255) not null,
    FOREIGN KEY (citizen_id) REFERENCES [dbo].[citizens](id),
    FOREIGN KEY (role_id) REFERENCES [dbo].[user_roles](id)
);

-- Thêm dữ liệu vào bảng citizens
INSERT INTO [dbo].[citizens] ([name], [email], [phone], [address]) VALUES
(N'Nguyễn Văn A', N'nguyenvana@example.com', N'0123456789', N'Số 123, Đường ABC, Quận 1'),
(N'Trần Thị B', N'tranthib@example.com', N'0987654321', N'Số 456, Đường DEF, Quận 2'),
(N'Phạm Văn C', N'phamvanc@example.com', N'0912345678', N'Số 789, Đường GHI, Quận 3');

-- Thêm dữ liệu vào bảng vehicles
INSERT INTO [dbo].[vehicles] ([owner_id], [license_plate], [make], [model], [year]) VALUES
(1, N'29A-12345', N'Toyota', N'Corolla', 2015),
(1, N'29B-67890', N'Ford', N'Fusion', 2010),
(2, N'29C-54321', N'Chevrolet', N'Cruze', 2012);

-- Thêm dữ liệu vào bảng violation_types
INSERT INTO [dbo].[violation_types] ([name], [description]) VALUES
(N'Tốc độ', N'Vi phạm tốc độ là một vi phạm giao thông nghiêm trọng'),
(N'Lái xe ẩu', N'Lái xe ẩu là một vi phạm giao thông nghiêm trọng'),
(N'Chạy đèn đỏ', N'Chạy đèn đỏ là một vi phạm giao thông nghiêm trọng');

-- Thêm dữ liệu vào bảng report_statuses
INSERT INTO [dbo].[report_statuses] ([status]) VALUES
(N'Mở'),
(N'Đóng'),
(N'Đang xử lý');

-- Thêm dữ liệu vào bảng violation_reports
INSERT INTO [dbo].[violation_reports] ([citizen_id], [vehicle_id], [violation_type_id], [status_id], [report_date], [location], [description]) VALUES
(1, 1, 1, 1, CAST('2022-01-01 00:00:00' AS DateTimeOffset), N'Số 123, Đường ABC', N'Đây là một báo cáo thử nghiệm'),
(1, 1, 2, 2, CAST('2022-01-02 00:00:00' AS DateTimeOffset), N'Số 456, Đường DEF', N'Đây là một báo cáo thử nghiệm khác'),
(2, 2, 3, 3, CAST('2022-01-03 00:00:00' AS DateTimeOffset), N'Số 789, Đường GHI', N'Đây là một báo cáo thử nghiệm thứ ba');

-- Thêm dữ liệu vào bảng user_roles
INSERT INTO [dbo].[user_roles] ([role_name]) VALUES
(N'Quản trị viên'),
(N'Moderator'),
(N'Người dùng');

-- Thêm dữ liệu vào bảng users
INSERT INTO [dbo].[users] ([citizen_id], [role_id], [username], [password_hash]) VALUES
(1, 1, N'admin', N'password123'),
(2, 2, N'moderator', N'password123'),
(3, 3, N'user', N'password123');