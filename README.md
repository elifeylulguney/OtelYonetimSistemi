# Hotel Management System

A comprehensive desktop **Hotel Management System** developed with **C# (.NET Windows Forms)** and **MS SQL Server / Azure SQL**. Designed using the **Repository Pattern**, this application streamlines daily hotel operations including room inventory, customer records, bookings, staff management, and housekeeping tracking.

---

## Key Features

The application provides complete **CRUD (Create, Read, Update, Delete)** operations, real-time record counters, and dynamic multi-criteria search filtering across five core modules:

* ** Room Management (`Oda Yönetimi`):**
  * Manage room numbers, names/types (Luxury, Standard, Economic, Royal Suite), floor levels, bed capacities, view types (Sea, Pool, City, Forest), balcony availability, and nightly pricing.
  * Instant search and filtering by room number, name, view, or floor.
* ** Customer Management (`Müşteri Yönetimi`):**
  * Maintain guest profiles including National ID / SSN, full name, phone number, and email address.
  * Track payment statuses (*Paid*, *Pending*, *Partial Payment*, *Cancelled*).
* ** Reservation Management (`Rezervasyon Yönetimi`):**
  * Create and manage bookings by linking Room Numbers and Customer SSNs.
  * Interactive check-in and check-out date selection with automatic total night calculation.
* ** Employee Management (`Çalışan Yönetimi`):**
  * Record staff details (SSN, full name, salary, and weekly working hours).
  * Role-based categorization (*Receptionist*, *Manager*, *Technician*, *Security*, *Chef*, *Waiter*, *Housekeeping*).
* ** Housekeeping Management (`Oda Temizlik Yönetimi`):**
  * Assign rooms to housekeeping staff via Employee SSN and schedule cleaning dates.
  * Monitor real-time cleaning statuses (*Completed*, *In Progress*, *Postponed*).
* ** Network & Database Connectivity:**
  * Built-in network availability verification (`NetworkCheck`) and database connection validation (`DatabaseHelper`) before executing operations.

---

## Project Architecture

The codebase follows clean object-oriented design principles and separates concerns into distinct layers:

```text
OtelYonetimSistemi/
├── Database/
│   └── DatabaseHelper.cs          # SQL connection management & configuration loader
├── Forms/
│   ├── BaseForm.cs                # Base form template for consistent UI/UX
│   ├── OdaForm.cs                 # Room Management UI
│   ├── MusteriForm.cs             # Customer Management UI
│   ├── RezervasyonForm.cs         # Reservation Management UI
│   ├── CalisanForm.cs             # Employee Management UI
│   └── OdaTemizlikForm.cs         # Housekeeping Management UI
├── Models/
│   ├── Oda.cs                     # Room entity model
│   ├── Musteri.cs                 # Customer entity model
│   ├── Rezervasyon.cs             # Reservation entity model
│   ├── Calisan.cs                 # Employee entity model
│   └── OdaTemizlik.cs             # Housekeeping entity model
├── Repositories/
│   ├── OdaRepository.cs           # Data access layer for Rooms
│   ├── MusteriRepository.cs       # Data access layer for Customers
│   ├── RezervasyonRepository.cs   # Data access layer for Reservations
│   ├── CalisanRepository.cs       # Data access layer for Employees
│   └── OdaTemizlikRepository.cs   # Data access layer for Housekeeping
├── MainForm.cs                    # Main navigation dashboard
├── NetworkCheck.cs                # Internet/network connectivity checker
├── Program.cs                     # Application entry point
└── appsettings.example.json       # Template for database connection string
```

---

## Tech Stack

* **Language:** C#
* **Framework:** .NET (Windows Forms)
* **Database:** Microsoft SQL Server / Azure SQL Database
* **Data Access:** ADO.NET (`Microsoft.Data.SqlClient`)
* **Architecture:** Repository Pattern & Object-Oriented Programming (OOP)

---

## Getting Started

### Prerequisites
* Visual Studio 2022 (or later) with the **.NET Desktop Development** workload installed.
* Access to an **MS SQL Server** or **Azure SQL Database** instance.

### Installation & Configuration

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/elifeylulguney/OtelYonetimSistemi.git](https://github.com/elifeylulguney/OtelYonetimSistemi.git)
   cd OtelYonetimSistemi
   ```

2. **Configure the Database Connection:**
   * Locate the `appsettings.example.json` file in the project directory.
   * Create a copy of this file in the same directory and rename it to `appsettings.json`.
   * Open `appsettings.json` and replace the placeholder values with your actual SQL Server credentials:
     ```json
     {
       "ConnectionStrings": {
         "OtelDbConnection": "Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=OtelYonetimiBulut;Persist Security Info=False;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
       }
     }
     ```
   *(Note: `appsettings.json` is listed in `.gitignore` to prevent sensitive credentials from being committed to version control).*

3. **Build and Run:**
   * Open `OtelYonetimSistemi.slnx` in Visual Studio.
   * Press **F5** (or click **Start**) to build and launch the application.

---

## Contributors

* **Elif Eylül Güney** - [@elifeylulguney](https://github.com/elifeylulguney)
* **Damla Avşar** - [@damlaavsar](https://github.com/damlaavsar)
* **Fatma Tuğba Akkaya** - [@ftugbakkaya](https://github.com/ftugbakkaya)
