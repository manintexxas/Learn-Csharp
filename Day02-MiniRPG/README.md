# ⚔️ Day 02 - MiniRpg Engine (Ongoing)

MiniRpg Engine adalah proyek latihan pembuatan *Core Engine* game Turn-Based RPG berbasis konsol menggunakan C# (.NET 8). Proyek ini berfokus pada penerapan konsep Object-Oriented Programming (OOP) tingkat menengah, seperti *Abstract Class*, *Inheritance*, *Polymorphism*, serta penggunaan *Design Pattern* dasar.

---

## 📦 Library & Dependency

Proyek ini menggunakan dependensi dan modul bawaan .NET berikut:

- **`System.Text.Json`**
  - **Fungsi:** Deserialisasi *payload* JSON masukan menjadi objek DTO (`CharacterDto`) untuk *spawning* karakter secara dinamis.

- **`System.Math`**
  - **Fungsi:** Menggunakan metode `Math.Clamp` untuk validasi dan proteksi rentang nilai poin kesehatan (`HealthPoint`) secara aman.

---

## 🛠️ Ringkasan Fitur & Abstraksi Utama

### 1. Base Character (`BaseCharacter`)
Kelas abstrak dasar yang membungkus semua atribut dan perilaku umum karakter (Hero / Monster):

- `HealthPoint` : Menggunakan `Math.Clamp` pada *setter* `protected` untuk memastikan nilai HP selalu berada dalam rentang `0` hingga `MaxHealth`.
- `IsAlive` : *Read-only property* untuk mengecek status hidup karakter berdasarkan HP (`HealthPoint > 0`).
- `TakeDamage` & `Heal` : Method `virtual` yang memproses perubahan HP dan mengembalikan status keberlangsungan hidup karakter.
- `UniqueSkill` : Method `abstract` yang wajib diimplementasikan oleh setiap kelas spesifik turunan.

---

### 2. Peta Arsitektur & Kelas (`RpgEngine`)

- **`CharacterDto`** : Model penampung data mentah masukan JSON dari `Program.cs`.
- **`Hero` & `Monster`** *(Pending)* : Kelas turunan yang meng-override `UniqueSkill` dengan efek spesifik.
- **`BattleSystem`** *(Pending)* : *Engine* simulasi pertarungan bergantian (*turn-based*) memanfaatkan kata kunci `ref` dan `out`.
- **`CharacterFactory`** *(Pending)* : Mengimplementasikan *Factory Pattern* berbasis `static` untuk membuat objek karakter dari DTO.

---

## 🚧 Status Pengerjaan (Progress Tracker)

- [x] **Langkah 1:** Abstraksi Dasar `BaseCharacter.cs` & validasi nilai properti (`Math.Clamp`).
- [ ] **Langkah 2:** Pembuatan entitas turunan `Hero.cs` dan `Monster.cs`.
- [ ] **Langkah 3:** Implementasi sistem alur pertarungan `BattleSystem.cs`.
- [ ] **Langkah 4:** Integrasi *Factory Pattern* `CharacterFactory.cs` dan pengolahan JSON di `Program.cs`.

---

## 🚀 Cara Menjalankan

1. Pastikan [.NET SDK 8.0](https://dotnet.microsoft.com/) sudah terpasang.
2. Jalankan perintah berikut dari direktori root repositori:

```bash
dotnet run --project Day02-RpgEngine