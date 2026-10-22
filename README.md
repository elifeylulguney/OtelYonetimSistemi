# Hotel Management System (Otel Yönetim Sistemi)

A desktop database application developed with **C# (.NET Windows Forms)** and **MS SQL Server / Azure SQL** as part of a **Database Management Systems** course project. 

The application manages daily hotel operations—including rooms, customers, reservations, employees, and housekeeping—through complete **CRUD (Create, Read, Update, Delete)** functionality and multi-criteria search filtering.

---

## Architecture & Technical Highlights

Rather than embedding SQL queries directly into UI event handlers, the project is structured around clean **Object-Oriented Programming (OOP)** principles and a layered architecture:

* **Repository Pattern (`Models/` & `Repositories/`):** Data entities (`Oda`, `Musteri`, `Rezervasyon`, `Calisan`, `OdaTemizlik`) and SQL data access logic are strictly separated from the user interface to ensure modularity and maintainability.
* **Form Inheritance (`Forms/BaseForm.cs`):** All module screens inherit from a custom `BaseForm` class, providing a unified layout behavior and reducing code duplication across the UI layer.
* **Externalized Configuration & Connectivity Checks (`DatabaseHelper.cs` & `NetworkCheck.cs`):** Database credentials are securely loaded from an external `appsettings.json` file rather than being hardcoded, and active network/database connections are verified prior to executing operations.

```text
OtelYonetimSistemi/
├── Database/        # DatabaseHelper.cs (SQL connection & configuration loader)
├── Forms/           # BaseForm.cs and UI forms for each management module
├── Models/          # Data entity classes
├── Repositories/    # ADO.NET SQL queries and CRUD implementations
├── MainForm.cs      # Main navigation dashboard
├── NetworkCheck.cs  # Network connectivity verification
└── appsettings.example.json
```

---

## Modules & Features

* **Room Management (`Odalar`):** Add, update, delete, and search hotel rooms by room number, name/type, floor, bed capacity, view type, balcony availability, and nightly price.
* **Customer Management (`Müşteriler`):** Maintain guest records (SSN/TC, full name, phone, email) and track payment statuses (*Paid*, *Pending*, *Partial Payment*, *Cancelled*).
* **Reservation Management (`Rezervasyonlar`):** Create and manage bookings by linking customer SSNs with room numbers, featuring check-in/check-out date selection and automatic stay duration calculation.
* **Employee Management (`Çalışanlar`):** Manage staff records, roles (*Receptionist*, *Manager*, *Technician*, *Security*, *Chef*, *Waiter*, *Housekeeping*), salaries, and weekly working hours.
* **Housekeeping Management (`Oda Temizlik`):** Assign room cleaning tasks to staff members by SSN and monitor cleaning statuses (*Completed*, *In Progress*, *Postponed*).

---

## Technologies Used

* **Language & Framework:** C#, .NET (Windows Forms)
* **Database:** Microsoft SQL Server / Azure SQL Database
* **Data Access:** ADO.NET (`Microsoft.Data.SqlClient`)

---

## Setup & Installation

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/elifeylulguney/OtelYonetimSistemi.git](https://github.com/elifeylulguney/OtelYonetimSistemi.git)
   cd OtelYonetimSistemi
   ```
2. **Configure the database connection:**
   Copy `appsettings.example.json` in the project directory, rename the copy to `appsettings.json`, and update the connection string with your own SQL Server credentials.
3. **Run the project:**
   Open `OtelYonetimSistemi.slnx` in Visual Studio and press **F5** to build and run the application.

---

## Contributors

* **Elif Eylül Güney** - [@elifeylulguney](https://github.com/elifeylulguney)
* **Damla Avşar** - [@damlaavsar](https://github.com/damlaavsar)
* **Fatma Tuğba Akkaya** - [@ftugbakkaya](https://github.com/ftugbakkaya)
