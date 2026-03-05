# PC Store API

A production-ready RESTful Web API built with ASP.NET Core Minimal APIs and Clean Architecture. This application manages users and their PC build configurations with full CRUD functionality using Entity Framework Core and SQL Server.

---

## 🔧 Tech Stack

**Framework**: ASP.NET Core Minimal APIs  
**ORM**: Entity Framework Core (Code-First)  
**Database**: SQL Server  
**Mapping**: AutoMapper  
**Validation**: FluentValidation  
**Testing**: Postman  
**Architecture**: Clean Architecture (API, Application, Domain, Infrastructure)

---

## 📌 Features

- RESTful API with **full CRUD** operations for:
  - Users
  - PC Build Configurations
- Clean separation of concerns using layered architecture:
  - `Domain` → Entities
  - `Application` → DTOs, interfaces, business rules
  - `Infrastructure` → Data access (DbContext, repositories)
  - `API` → Minimal API endpoint definitions
- Implements **Repository and Service Patterns** to isolate logic and improve testability
- **AutoMapper** for mapping between domain models and DTOs
- **FluentValidation** for structured and reusable input validation
- SQL Server integration with EF Core migrations
- Manual testing done using **Postman** and real SQL Server database

---

## 📦 Required Packages

The following NuGet packages are used in this project:

- `Microsoft.EntityFrameworkCore.SqlServer` – Entity Framework Core with SQL Server support  
- `Microsoft.EntityFrameworkCore.Tools` – EF Core command-line tools (for migrations)  
- `AutoMapper` – For mapping between domain models and DTOs  
- `FluentValidation` – Input validation for DTOs  
- `FluentValidation.AspNetCore` – Middleware support for FluentValidation  
- `AutoMapper.Extensions.Microsoft.DependencyInjection` – AutoMapper integration with DI container  
