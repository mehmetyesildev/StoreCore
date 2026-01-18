# 📦 StoreCore – Role-Based E-Commerce Platform

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-blue)
![License](https://img.shields.io/badge/License-Proprietary-red)

**StoreCore** is a **Full-stack ASP.NET Core MVC application** built with **industry-standard ASP.NET Core practices**, following **N-Tier architecture** and **repository-based data access**, featuring **Iyzico payment integration**.

> **ℹ️ Transparency Note:**
> This project was developed as a personal portfolio project.
> Core features such as Iyzico integration, role-based authorization,
> admin dashboard calculations, and cart logic were designed
> and implemented by me to reflect real-world backend scenarios.

---

## 🚀 Key Features

- **💳 Iyzico Payment Integration** Fully functional sandbox payment infrastructure with 3D Secure support.

- **📊 Live Admin Dashboard** Real-time revenue, product count, and category statistics fetched directly from the database (not static HTML).

- **🔐 Identity & Security** Role-based authorization (Admin/User) using ASP.NET Core Identity.

- **🛒 Smart Cart Management** Session-based cart system with dynamic price and quantity calculations.

- **📱 Responsive UI** Modern, mobile-friendly interface built with Bootstrap 5.

---

## 📸 Screenshots

### 1. Admin Dashboard
*Live business metrics calculated from database records.*
<img width="1913" height="904" alt="Admin Dashboard" src="https://github.com/user-attachments/assets/f2fb8021-10b2-489c-bbb5-f9c488d2f644" />

---

### 2. Checkout & Payment
*User-friendly, fully localized checkout experience.*
<img width="1159" height="870" alt="Checkout" src="https://github.com/user-attachments/assets/be6f473a-cd16-4a6b-a3cb-8982a1fb2a56" />

---

### 3. Product Management (Admin)
*Full CRUD operations for product management.*
<img width="1911" height="915" alt="Product Management" src="https://github.com/user-attachments/assets/610c6ea0-88cf-43b0-92cc-6f46e76d15dc" />

---

### 4. Storefront
*Category-based filtering and detailed product pages for customers.*
<img width="1898" height="854" alt="Storefront" src="https://github.com/user-attachments/assets/3002f290-742e-4d63-9a7a-5afb5fc4d0d5" />

---

## 🛠️ Technologies & Architecture

- **Backend:** ASP.NET Core 8.0 MVC
- **Database:** Entity Framework Core (Code-First, SQLite for development)
- **Architecture:** N-Tier Architecture, Repository Pattern
- **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript
- **Tools:** Visual Studio Code (C# Dev Kit), Git, Iyzico API

---

## ⚙️ Getting Started

### Prerequisites
- .NET SDK 8.0
- Visual Studio Code (with C# Dev Kit extension recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone [https://github.com/mehmetyesildev/StoreCore.git](https://github.com/mehmetyesildev/StoreCore.git)
2. **Open the project Open the project folder with VS Code.**
3. **Apply database migrations Open Terminal and run:**
    ```bash
    dotnet ef database update
4. **Run the application**
    ```bash
    dotnet run
Demo admin and user accounts are seeded for development purposes.

⚠️ License
This project is shared for educational and portfolio purposes. Commercial use requires permission from the author.

👨‍💻 Developer
Mehmet Yeşil Software Developer (.NET)

This project was developed to demonstrate my experience in backend architecture, payment integrations, and role-based web application development using ASP.NET Core.
