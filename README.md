# Employee Management System

This project is a comprehensive **Employee Management System** designed to demonstrate expertise in **.NET 9 Web API**, **Angular 18**, and database management using **SQL Server**, **PostgreSQL**, and **MongoDB**. The solution adheres to **SOLID principles** and implements the **Repository Pattern** for scalability and maintainability.

---

## Objective

The objective is to manage employees, departments, designations, and attendance records while maintaining operation logs for CRUD activities. It showcases the use of **Dapper**, **stored procedures**, and the integration of multiple database technologies.

---

## Features

### **1. Employee Management (SQL Server)**

#### Entities:
- **Employee Profile**: Includes details like Name, Email, Phone, Address, Date of Birth, etc.
- **Designation**: Represents employee roles.
- **Department**: Represents organizational departments.

#### Operations:
- CRUD operations for Employee, Designation, and Department.
- Data access via **Dapper** using **stored procedures** (no inline SQL queries).
- Repositories:
  - `EmployeeRepository`
  - `DesignationRepository`
  - `DepartmentRepository`

### **2. Operation Logs (MongoDB)**

#### Purpose:
- Store logs of CRUD operations performed on SQL Server entities.

#### Log Schema:
- **Fields**:
  - `OperationType`: Type of operation (e.g., Create, Update, Delete).
  - `EntityName`: Name of the entity affected.
  - `EntityId`: ID of the affected entity.
  - `TimeStamp`: Time of operation.
  - `OperationDetails`: Detailed description of the operation.

#### Implementation:
- `OperationLogRepository` for managing MongoDB logs.

### **3. Employee Attendance (PostgreSQL)**

#### Purpose:
- Record check-in and check-out times for employees.
- Link attendance records to employee profiles stored in SQL Server.

#### Implementation:
- CRUD operations for attendance via `AttendanceRepository`.
- Repository pattern for PostgreSQL interactions.

---

## Technology Stack

### **Frontend**: 
- Angular 18
  - **Features**:
    - Minimalistic, responsive design using Bootstrap.
    - Dynamic forms with validation.
    - Component-based architecture.

### **Backend**:
- **.NET 9.0.100-rc.2.24474.11 Web API**
  - Features:
    - Modular architecture following SOLID principles.
    - Repository pattern for database interactions.
    - Logging of operations.

### **Databases**:
1. **SQL Server** (for Employee Management)
2. **PostgreSQL** (for Attendance Management)
3. **MongoDB** (for Operation Logs)

---

## Prerequisites

1. **Node.js** (v18 or later)
2. **Angular CLI** (v18 or later)
3. **.NET SDK** (v9 or later)
4. **Databases**:
   - SQL Server
   - PostgreSQL
   - MongoDB

---

