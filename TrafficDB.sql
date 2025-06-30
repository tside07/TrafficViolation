-- Kết nối đến master database
USE master;
GO

-- Tạo database mới, ví dụ tên là TrafficViolationDB
CREATE DATABASE TrafficViolationDB;
GO

CREATE TABLE citizens (
    id bigint primary key IDENTITY(1,1),
    name nvarchar(255) not null,
    email nvarchar(255) unique not null,
    phone nvarchar(50),
    address nvarchar(255)
);

CREATE TABLE vehicles (
    id bigint primary key IDENTITY(1,1),
    owner_id bigint not null,
    license_plate nvarchar(50) unique not null,
    make nvarchar(100),
    model nvarchar(100),
    year int,
    FOREIGN KEY (owner_id) REFERENCES citizens(id)
);

CREATE TABLE violation_types (
    id bigint primary key IDENTITY(1,1),
    name nvarchar(100) not null,
    description nvarchar(255)
);

CREATE TABLE report_statuses (
    id bigint primary key IDENTITY(1,1),
    status nvarchar(50) not null
);

CREATE TABLE violation_reports (
    id bigint primary key IDENTITY(1,1),
    citizen_id bigint not null,
    vehicle_id bigint not null,
    violation_type_id bigint not null,
    status_id bigint not null,
    report_date datetimeoffset default SYSDATETIMEOFFSET(),
    location nvarchar(255),
    description nvarchar(255),
    FOREIGN KEY (citizen_id) REFERENCES citizens(id),
    FOREIGN KEY (vehicle_id) REFERENCES vehicles(id),
    FOREIGN KEY (violation_type_id) REFERENCES violation_types(id),
    FOREIGN KEY (status_id) REFERENCES report_statuses(id)
);

CREATE TABLE user_roles (
    id bigint primary key IDENTITY(1,1),
    role_name nvarchar(50) unique not null
);

CREATE TABLE users (
    id bigint primary key IDENTITY(1,1),
    citizen_id bigint,
    role_id bigint not null,
    username nvarchar(100) unique not null,
    password_hash nvarchar(255) not null,
    FOREIGN KEY (citizen_id) REFERENCES citizens(id),
    FOREIGN KEY (role_id) REFERENCES user_roles(id)
);