# Event Management

**Your Platform for Organizing and Participating in Events.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-9.0-blueviolet.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)

Event Management is a web application built with ASP.NET Core 9.0, designed to streamline the process of creating, managing, and participating in events. It offers functionalities for user registration, event creation and browsing, user administration, and tracking event participation through user roles.

> **Mission:** To provide a seamless and organized online platform for managing events and coordinating user participation effectively.

---

## Core Features

Event Management offers a range of features for different user roles:

**Event & User Management:**

* **Event Catalogue:** Browse a list of available events with details (e.g., description, date, time, location - based on the `Event.cs` model).
* **Event Creation & Management:** Authenticated users (potentially with specific roles) can perform Create, Read, Update, and Delete (CRUD) operations on events. Events are linked to their creators (`CreatorId`).
* **User Management:** Administrators can view and potentially manage application users (`UsersController.cs`).
* **User-Event Association:** Tracks which users are associated with which events (`UserEvent.cs`), likely for registration or attendance purposes.

**User Authentication & Roles:**

* **Robust User Authentication:** Secure user registration, login/logout, and password recovery using ASP.NET Core Identity.
* **Role-Based Access:** Utilizes user roles (e.g., "Admin", "User") seeded into the database (`SeedRoles.cs`) to control access to different features (like event creation or user management).

**Infrastructure:**

* **Database Seeding:** Automatic initialization of the database with essential data (like user roles) on first launch.
* **Database Migrations:** Managed database schema evolution using Entity Framework Core Migrations.

---

## Technology Stack

Built with a modern and robust technology stack:

* **Backend Framework:** ASP.NET Core 9.0 (using MVC and Razor Pages for Identity) (C#)
* **Data Access:** Entity Framework Core 9.0 (using Code-First migrations)
* **Authentication:** ASP.NET Core Identity
* **Database:** Configured for EF Core compatible databases (e.g., Microsoft SQL Server - requires connection string setup).
* **Frontend:** Razor Views (`.cshtml`), HTML, CSS, JavaScript
* **UI Libraries:** Bootstrap, jQuery, jQuery Validation
* **Architecture:** MVC, Dependency Injection

---

## 🚀 Getting Started

Follow these steps to get Event Management running locally:

**1. Prerequisites:**

* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express, Developer, or other editions) or another configured EF Core compatible database.
* Development Environment (e.g., Visual Studio 2022+, VS Code)

**2. Clone the Repository:**

```bash
# Replace YOUR_USERNAME/Event-Management.git with the actual repository URL if applicable
git clone [https://github.com/YOUR_USERNAME/Event-Management.git](https://www.google.com/search?q=https://github.com/YOUR_USERNAME/Event-Management.git)
cd Event-Management
(Note: Update the repository URL if you host it)

3. Configure Application Secrets:

Use User Secrets for sensitive data like connection strings.

Initialize User Secrets (if not already done):
Bash

dotnet user-secrets init
Set Database Connection String:
Bash

# Adjust the connection string for your specific SQL Server instance
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=EventManagementDb_Dev;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
(You might need to adjust EventManagementDb_Dev based on your appsettings.json or context configuration)
4. Restore Dependencies:

Bash

dotnet restore
5. Apply Database Migrations:

This command creates the database (if it doesn't exist) and applies the schema.

Bash

dotnet ef database update
6. Run the Application:

Bash

dotnet run
The application will start, and the console output will show the URLs (e.g., https://localhost:xxxx).

7. Explore! Open your browser and navigate to one of the provided URLs. Register a new account (the application should seed roles, potentially including an Admin role if configured).

Contributing
Contributions are welcome! If you'd like to help improve Event Management:

Fork the repository.
Create a new branch (git checkout -b feature/YourFeature or bugfix/YourBugfix).
Make your changes and commit them (git commit -am 'Add some feature').
Push to the branch (git push origin feature/YourFeature).
Open a Pull Request.
Please adhere to standard coding practices and provide clear descriptions. Use GitHub Issues for bugs or feature suggestions.

License
This project is licensed under the MIT License.

(Ensure a LICENSE file containing the MIT License text exists in the root of your repository.)


Remember to:

1.  Replace placeholder text like `[https://github.com/YOUR_USERNAME/Event-Management.git](https://www.google.com/search?q=https://github.com/YOUR_USERNAME/Event-Management.git)` with the actual URL if you host this project on GitHub.
2.  Add a real screenshot in the "Visual Peek" section.
3.  Verify the database name used in the connection string example matches your project's configuration (`EventManagementDb_Dev` was assumed).
4.  Include an actual `LICENSE` file (containing the MIT license text) in the root directory of your project.
