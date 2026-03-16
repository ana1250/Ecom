# E-Commerce Inventory Management System

A production-grade, full-stack inventory management system built with ASP.NET Core MVC (Razor Pages), Entity Framework Core, and JWT authentication. Designed to securely manage complex relational data including Products, Brands, Suppliers, and Product Types.

**Live Demo:** [https://anant.runasp.net/](https://anant.runasp.net/)

---

## 🏗 System Architecture

The application follows an N-Tier architecture designed for separation of concerns, maintainability, and security:

* **Presentation Layer (Razor Pages & Bootstrap 5):**
  * Server-side rendered views providing a fast, SEO-friendly, and highly secure frontend.
  * Enhanced with a custom glassmorphism design system (`custom.css`), Feather Icons, and buttery-smooth CSS3 micro-animations for a premium UX.
* **Controller / API Layer (`Controllers/`):**
  * RESTful API endpoints secured by strict `[Authorize]` middleware policies (e.g., `AdminOnly`) to handle data mutations.
* **Service Layer (`Services/`):**
  * Encapsulates core business logic, user authentication, profile management, and comprehensive data validation.
  * Implements structured exception handling via Serilog to prevent application crashes and ensure observability.
* **Data Access Layer (Entity Framework Core):**
  * Object-Relational Mapping (ORM) handling complex relationships (One-to-Many, Many-to-Many).
  * Optimized using eager loading (`.Include()`) to eliminate N+1 query bottlenecks and maximize performance.
* **Database (SQL Server):**
  * Secure, relational data storage.
  * Maintains immutable audit trails (`CreatedAt`, `CreatedBy`, `AuthLog`) for all sensitive operations.

---

## 🚀 Key Features

* **Secure Authentication & Identity:**
  * JWT (JSON Web Token) based session management.
  * Passwords securely hashed using `BCrypt`.
  * Role-Based Access Control (RBAC) separating `Admin` and standard `User` privileges.
* **Comprehensive Inventory Management:**
  * Full CRUD (Create, Read, Update, Delete) operations for Products, Suppliers, Brands, and Product Types.
* **Data Integrity & Audit Trails:**
  * Automatic tracking of creation dates and authors to prevent data loss during concurrent editing.
* **Premium UI/UX Polish:**
  * Fully responsive dashboard protected by layout wrappers.
  * "Buttery smooth" transitions, hover states, and dynamic gradient cards.

---

## 🛠 Technologies Used

* **Backend:** C#, .NET 8, ASP.NET Core
* **Data & ORM:** SQL Server, Entity Framework Core, LINQ
* **Frontend:** HTML5, CSS3, Bootstrap 5, Feather Icons, Razor Pages
* **Security:** JWT (JSON Web Tokens), BCrypt.Net
* **Observability:** Serilog

---

## ⚙️ Local Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd ECom_Inventory
   ```

2. **Configure AppSettings:**
   Update your `appsettings.json` or `appsettings.Development.json` with your SQL database connection string and a secure JWT Secret Key:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=...;Database=ECom_Db;Trusted_Connection=True;MultipleActiveResultSets=true"
   },
   "AuthSettings": {
     "SecretKey": "YOUR_SUPER_SECRET_KEY_AT_LEAST_32_CHARS",
     "Issuer": "EcomInventoryApp",
     "Audience": "EcomInventoryUsers",
     "ExpirationMinutes": 60
   }
   ```

3. **Run EF Core Migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Build and Run:**
   ```bash
   dotnet run
   ```
