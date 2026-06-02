# 🏢 Smart Visitor Management API

A RESTful API built with **ASP.NET Core** to manage and track visitor entries in an organization.  
Follows **Clean Architecture** principles for a well-structured, maintainable codebase.

---

## 🛠️ Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Architecture:** Clean Architecture

---

## 📁 Project Structure
SmartVisitorManagement/
├── SmartVisitorManagement.API           # Entry point — Controllers, Middleware
├── SmartVisitorManagement.Core          # Entities, Interfaces, DTOs
├── SmartVisitorManagement.Services      # Business Logic
└── SmartVisitorManagement.Infrastructure # EF Core, Database, Repositories

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 / VS Code

### Setup

1. Clone the repository:
```bash
   git clone https://github.com/rmezanur521-boop/SmartVisitorManagement.API.git
```

2. Update the connection string in `appsettings.json`:
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=SmartVisitorDB;Trusted_Connection=True;"
   }
```

3. Apply migrations:
```bash
   dotnet ef database update
```

4. Run the project:
```bash
   dotnet run --project SmartVisitorManagement.API
```

---

## 📌 Features

- Register and track visitors
- Manage visitor check-in and check-out
- RESTful API endpoints
- Clean, layered architecture

---

## 👤 Author

**Mezanur Rahman**  
Diploma Engineering Student | ASP.NET Core Developer  
[GitHub Profile](https://github.com/rmezanur521-boop)
