# 🚀 C# & .NET Daily Learning Journey

Repository ini berisi kumpulan *mini project*, latihan logika, dan catatan implementasi konsep C# / .NET yang dikerjakan secara berkala.

---

## 🛠️ Requirements & Tech Stack

* **Language:** C#
* **Framework:** .NET 8.0 SDK (atau versi terbaru)
* **IDE/Editor:** VS Code / Visual Studio 2022
* **Package Manager:** NuGet

---

## 📂 Progress & Mini Projects

Tabel di bawah diperbarui setiap kali ada penambahan *mini project* baru.

| Day | Project Name | Konsep Utama / Key Features | Link Folder |
| :---: | :--- | :--- | :---: |
| **01** | **Person Registration App** | Constructor, Static Keyword, Private Setter, Extension Methods, Enkapsulasi | [DailyProject01](./Day01-PersonRegistration) |
<!-- Tambahkan baris baru di bawah ini setiap ada task baru -->

---

## 🏗️ Struktur Repository

```text
learn-csharp/
├── learn-csharp.sln                   <-- Solution file
├── README.md                          <-- Dokumentasi utama
├── .gitignore
└── Day01-PersonRegistration/          <-- Mini project Day 01
    ├── Program.cs
    └── Day01-PersonRegistration.csproj
```

---

## 🚀 Cara Menjalankan Project

1. **Clone Repository**
   ```bash
   git clone [https://github.com/username-kamu/learn-csharp.git](https://github.com/username-kamu/learn-csharp.git)
   cd learn-csharp
   ```

2. **Jalankan Project Spesifik**
   Gunakan flag `--project` diikuti nama folder project yang ingin dijalankan:
   ```bash
   # Menjalankan project Day 01
   dotnet run --project Day01-PersonRegistration
   ```

---

## 📝 Catatan Perkembangan

* **Day 01:** Mempelajari enkapsulasi objek `Person` dengan *private constructor*, pembuatan method validasi via *extension methods*, dan menghitung total registrasi menggunakan *static property*.