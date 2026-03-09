# aspnetcore_api | Mastering RESTful API Development in .NET 10

<p align="center">
  <img src="https://skillicons.dev/icons?i=cs,dotnet,visualstudio,postman,powershell" />
</p>


---


## Overview
This repository serves as a deep-dive into building production-grade APIs using **ASP.NET Core 10**. It tracks my progress through the "Master RESTful API Development" course, moving from fundamental REST principles to complex security architectures and frontend integration.

The primary project is a robust **Bug Tracker Web API**, designed with a focus on **Clean Architecture**, **Best Practices**, and **Identity Management**.

---

## Architecture & Tech Stack
![.NET 10](https://img.shields.io/badge/.NET-10-512bd4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-623697?style=for-the-badge&logo=dotnet)
![IdentityServer4](https://img.shields.io/badge/Security-Identity_Server_4-f37021?style=for-the-badge)
![EF Core](https://img.shields.io/badge/ORM-EF_Core-512bd4?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-In%20Progress-yellow?style=for-the-badge)


---

## Learning Roadmap

### 1. API Architecture & Routing
- [x] **RESTful Design:** Resource-based naming, HTTP verbs, and proper status code usage.
- [x] **Routing & Model Binding:** Deep dive into how ASP.NET Core maps requests to logic.
- [x] **Model Validation & Filters:** Implementing a clean Filter Pipeline to handle cross-cutting concerns.
- [x] **Versioning & OpenAPI:** Implementing API versioning and full Swagger/OpenAPI documentation.

### 2. Data & Business Logic
- [x] **Entity Framework Core:** Mastering migrations and data relationships for the Bug Tracker.
- [x] **Repository Pattern:** Abstracting data access to ensure the system is loosely coupled.
- [x] **Best Practices:** Implementing proper error handling and logging throughout the API.

### 3. Security (The Core Focus)
- [x] **JWT Authentication:** Implementing stateless JSON Web Tokens.
- [x] **Custom Token Auth:** Understanding token generation and validation mechanics.
- [x] **Identity Server 4:** In-depth implementation of centralized authentication and OpenID Connect.

### 4. Client Consumption
- [x] **Blazor WebAssembly:** Building the "Shirts" frontend to consume the secured API.
- [x] **Integration Testing:** Using PowerShell and Postman for automated endpoint verification.

---

## Key Project: Shirts API
This API demonstrates a real-world scenario where shirts, are retrieved, deleted, edited and added to the database.



**Key Features:**
- **Secure Auth:** Users must be authenticated via Identity Server 4 or JWT to manage bugs.
- **Fluent Validation:** Ensures high data integrity for bug reports.
- **Auto-Documentation:** Interactive Swagger UI for easy testing.

---

## How to Run Locally


1. **Clone the repo:**
   ```bash
   git clone https://github.com/Gescale/aspnetcore_api.git
2. **Database Migration:**
   ```bash
   dotnet ef database update

3. **Run the API:**
   ```bash
   dotnet run --project BugTracker.API




