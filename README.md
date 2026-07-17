# 📦 WareHouseApp - The 4-Hour Client Special 🚀

Welcome to the **WareHouseApp**—a lightweight, desktop-first Warehouse Management System (WMS) built in C# and Windows Forms (WinForms). 

This project was built from scratch in **under 4 hours** to meet the *exact* requirements of a specific client. 

> [!WARNING]
> **Security Status: Non-Existent by Design!** 🛑
> This application was built for a secure, closed internal environment. Passwords are saved in plain text, there is no transport encryption, and anyone who gets the database file owns the warehouse. Please **DO NOT** expose this system to the public internet unless you want to invite hackers for a cup of tea. ☕

---

## 🎨 Feature Tour

Here's what this compact powerhouse can do:
1. **Simple Role-Based Authentication** 🔑: Users can login or register. Role division checks if you are an `Admin` or an `Employee` (e.g. `ShippingOperator`).
2. **Customer Contact Directory** 👥: Full CRUD system to track customer names and contact numbers.
3. **Inventory Management** 📦:
   - **Load Stocks**: Safely check in stock (increases inventory and logs the transaction ledger).
   - **Ship Stocks**: Check out stock with automated validation to prevent shipping things that aren't there.
4. **Transaction Log Ledger** 📊: Records every stock load and ship transaction with the material ID, operating employee ID, transaction type, and quantity.

---

## 🛠️ The Tech Stack (A Classic Combo)

* **Language:** C# 7.3+
* **Framework:** .NET Framework 4.7.2
* **GUI Engine:** WinForms (Windows Forms) - because native desktop grids and layouts are timeless.
* **Database:** Microsoft SQL Server LocalDB (`(LocalDB)\MSSQLLocalDB` attached database file `WarehouseDB.mdf` directly in the project directory).
* **Data Access:** Direct SQL parameterized queries via ADO.NET (`SqlDataAdapter`, `SqlCommand`, `SqlParameter`).

---

## 🧩 OOP Implementation & Design

Even though it was built in a dash, it follows solid Object-Oriented Programming (OOP) principles:
* **Abstraction**: The abstract base class [Person](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/People/Person.cs) establishes the core identity contract for any user in the system.
* **Inheritance & Polymorphism**: [Admin](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/People/Admin.cs) and [ShippingOperator](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/People/ShippingOperator.cs) extend `Person` and override `Login` to manage roles.
* **Encapsulation**: Private fields, custom accessors (`get`/`set`), and business logic are safely packaged inside components like [Material](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/Material/Material.cs) and [Customer](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/Customer.cs).

---

## ⚡ How to Get Started

### Prerequisites
1. **Windows OS** (WinForms is Windows-only natively).
2. **Visual Studio 2019/2022** with the **.NET desktop development** workload checked.
3. **Microsoft SQL Server LocalDB** (installed by default with Visual Studio).

### Running the App
1. Clone this repository to your local machine.
2. Double-click [WareHouseApp.sln](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp.sln) to open the solution in Visual Studio.
3. Set `WareHouseApp` as the startup project if it isn't already.
4. Press **`F5`** or click **Start** to run.
5. Either log in with your credentials or click **Register** to create a new user!

---

## 📂 Code Layout

* [Program.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/Program.cs): Application entry point. Launches the login screen.
* [LoginForm.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/LoginForm.cs): The welcome screen asking for credentials.
* [DatabaseHelper.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/DatabaseHelper.cs): Hand-written wrapper for managing connections, executing queries, and handling parameters.
* [DashBoard.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/DashBoard.cs): Main application window. Dynamically loads modules.
* **Dashboards / Modules**:
  * [CustomerDash.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/CustomerDash.cs): Manages the customer registry.
  * [InventoryDash.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/InventoryDash.cs): Handles loading, shipping, and transaction views.
  * [MainDash.cs](file:///c:/Users/MSI/Downloads/CS107.3%20Object%20Oriented%20Programming%20with%20C%23/WareHouseApp%20-%20repeat%20coursework/WareHouseApp%20-%20repeat%20coursework/WareHouseApp/MainDash.cs): Admin dashboard for user role overview.

---

## 🤝 Contributions & License

Pull Requests are welcome! Feel free to add security audits, hash the passwords, or redesign the UI, but remember: *keep it simple!* 

Designed for standard single-terminal local desktop usage. Built for a client in under 4 hours. No fancy fluff. Just pure, functional code.
