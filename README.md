# C# & .NET Daily Learning Journey 🚧 Active Development

> **Kategori:** Learning Repository / Console Applications / Mini Projects  
> **Status Proyek:** Active Development  
> **Target Pengguna:** C#/.NET Developers, Technical Recruiters, Peer Learners  

---

## 💡 Gambaran Umum & Tujuan Bisnis
Repositori ini berisi serangkaian modul latihan harian, eksplorasi arsitektur, serta implementasi logika bisnis berbasis **C# 12** dan **.NET 8.0**. Proyek ini dirancang untuk membangun pemahaman teknis mendalam secara bertahap (*incremental learning*) — mulai dari konsep fundamental Pemrograman Berorientasi Objek (OOP) hingga penerapan praktik pengujian terisolasi (*Unit Testing*) dan penulisan kode bersih (*Clean Code*).

Setiap modul di dalam repositori ini menyimulasikan skenario dunia nyata untuk memberikan nilai tambah berikut:
* **Keandalan Perangkat Lunak:** Memastikan logika bisnis tervalidasi dengan baik melalui pengujian otomatis (*Unit Testing*).
* **Kemudahan Pemeliharaan (Maintainability):** Menerapkan arsitektur kode terstruktur yang memisahkan antara entitas utama dan pengujian.
* **Standardisasi Industri:** Mengadopsi konvensi penulisan C# modern, penggunaan *Try-Pattern*, *type-safety*, serta *XML Documentation* (`///`).

---

## ✨ Fitur Utama (Key Features)
- **Modular Learning Labs:** Setiap modul harian diisolasi dalam proyek tersendiri. - *Memudahkan penelusuran materi dan eksperimen fitur C# secara terfokus.*
- **Automated Unit Testing:** Proyek pengujian terpisah untuk setiap modul lab. - *Meningkatkan kualitas kode serta mencegah terjadinya regresi logika.*
- **Self-Documenting Code:** Penggunaan C# XML Comments (`///`) yang presisi di seluruh class dan method. - *Mempermudah navigasi API dan pemahaman arsitektur oleh developer lain.*
- **Clean Architecture Principles:** Penerapan pola desain OOP, abstraksi, serta pemisahan tanggung jawab (*Separation of Concerns*). - *Membiasakan standar penulisan kode tingkat enterprise.*

---

## 📐 Arsitektur & Tech Stack

### Tech Stack
- **Runtime & Language:** .NET 8.0 | C# 12
- **Core Libraries & Tools:** xUnit, NUnit, System.Text.Json, System.Math

### Konsep Arsitektur / OOP
- **Object-Oriented Programming (OOP):** Penerapan Encapsulation, Polymorphism, Abstraction, dan Interfaces secara terstruktur.
- **Design Patterns & Conventions:** Extension Methods, Try-Pattern, Static Counter, serta Unit Testing terisolasi.

---

## 🗓️ Catatan Perkembangan & 🚧 Progress Tracker (Work in Progress)

### Catatan Perkembangan (Development History)
* **Day 01 - Person Registration App:** Implementasi *Try-Pattern*, *Encapsulation*, *Static Counter*, *Extension Methods*, dan *Title Case Formatting*.
* **Day 02 - MiniRPG Engine:** Implementasi *Abstract Class*, *Polymorphism*, *Interfaces*, dan *Status Effects* (`BleedEffect`, `RegenEffect`, `StunEffect`).

### Daftar Status & Pending Features
- [DONE] **Day 01 — Person Registration App:** Modul registrasi beserta pengujian unit (`PersonRegistration.Tests`).
- [PENDING] **Day 02 — MiniRPG Engine (Abstraksi & Status Effects):** Implementasi `BaseCharacter`, `StatusEffect`, dan `StatusManager` beserta `MiniRPG.Tests`.

---

## 🚀 Panduan Memulai & Cara Menjelaskan Project

### Prasyarat (Prerequisites)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) atau versi terbaru.
- **IDE / Text Editor:** Visual Studio 2022 / JetBrains Rider / Visual Studio Code (dengan ekstensi C# Dev Kit).

### Langkah Menjalankan & Menjelaskan Project
1. **Persiapan Environment & Clone Repo:**
   ```bash
   git clone [https://github.com/username/repository-name.git](https://github.com/username/repository-name.git)
   cd repository-name
   dotnet build learn-csharp.slnx

2. **Menjalankan Pengujian Unit (Unit Testing):**
    ```bash
        dotnet test

2. **Menjalankan Aplikasi Utama:**
    ```bash
        dotnet run --project [folder project]