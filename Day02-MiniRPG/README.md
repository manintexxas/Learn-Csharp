# MiniRPG Engine 🚧 Work in Progress

> **Kategori:** Console Application / Core Game Engine  
> **Status Proyek:** Development (Work in Progress)  
> **Target Pengguna:** Game Developer / Technical Assessor / Peer Learners  

---

## 💡 Gambaran Umum & Tujuan Bisnis
**MiniRPG Engine** adalah simulasi *Core Engine* permainan bertema *Turn-Based RPG* berbasis konsol C# (.NET 8). Proyek ini dirancang untuk melatih dan menguji penerapan tingkat menengah dari **Object-Oriented Programming (OOP)**, pemisahan logika (*Separation of Concerns*), serta pengujian unit (*Unit Testing*).

Aplikasi ini menerima data konfigurasi karakter melalui format **JSON**, memprosesnya secara dinamis, serta menguji status dan efek pertarungan (seperti pendarahan, regenerasi, dan stun) selama pertarungan berlangsung.

---

## ✨ Fitur Utama (Key Features)
- **Abstraksi & Proteksi Atribut Karakter (`BaseCharacter`):** Pengisolasian status poin kesehatan (`HealthPoint`) dengan `Math.Clamp` agar tidak bernilai negatif atau melebihi batas maksimum (`MaxHealth`), serta pemodelan siklus hidup (`IsAlive`, `TakeDamage`, `Heal`).
- **Manajemen Efek Status Dinamis (`StatusEffect`):** Pengelolaan efek berkelanjutan berbasis giliran (*turns*) yang mencakup pendarahan (`BleedEffect`), regenerasi (`RegenEffect`), dan kelumpuhan (`StunEffect`).
- **Pengelola Efek Terpusat (`StatusManager`):** Mengatur penambahan, pembaruan durasi, eksekusi dampak efek, dan pembersihan efek yang kadaluwarsa pada karakter secara terisolasi.
- **Otomasi Pengujian Unit (`Test/`):** Pengujian unit otomatis (`BaseCharacterTest.cs` dan `EffectTest.cs`) untuk memverifikasi kalkulasi poin kesehatan dan durasi efek status berjalan presisi.

---

## 📐 Arsitektur & Tech Stack

### Tech Stack
- **Runtime & Language:** .NET 8.0 | C# 12
- **Core Libraries & Tools:** `System.Text.Json` (pembacaan dan deserialisasi data DTO karakter), `System.Math` (`Math.Clamp` untuk isolasi nilai atribut numerik)

### Konsep Arsitektur / OOP
- **IS-A (Inheritance / Pewarisan):** `Hero` dan `Monster` adalah turunan dari `BaseCharacter`. Kelas `BleedEffect`, `RegenEffect`, dan `StunEffect` merupakan turunan dari `StatusEffect`.
- **HAS-A (Composition / Komposisi):** `BaseCharacter` memiliki pengelola efek (`StatusManager`), yang menampung koleksi efek (`StatusEffect`) aktif pada karakter tersebut.

---

## 🗓️ Catatan Perkembangan & 🚧 Progress Tracker (Work in Progress)

### Catatan Perkembangan (Development History)
* **Hari 1:** Fondasi Entitas & Abstraksi (`BaseCharacter`, proteksi `Math.Clamp`, siklus hidup karakter, dan deklarasi method abstrak `UniqueSkill()`).
* **Hari 2:** Sistem Efek Status (`IUseable`, `StatusEffect`, `BleedEffect`, `RegenEffect`, `StunEffect`, `StatusManager`, serta penambahan Unit Test).

### Daftar Status & Pending Features
- [x] **Hari 1 — Abstraksi Dasar:** `BaseCharacter.cs` & validasi proteksi HP (`Math.Clamp`).
- [x] **Hari 2 — Sistem Efek Status:** `IUseable.cs`, `StatusEffect.cs`, `StatusManager.cs`, serta kelas efek spesifik (`Bleed`, `Regen`, `Stun`).
- [x] **Hari 2 — Pengujian Unit:** Pembuatan pengujian otomatis di direktori `Test/`.
- [ ] **Pending Step 1 — Concrete Classes:** Implementasi kelas turunan konkret `Hero.cs` dan `Monster.cs` beserta logika `UniqueSkill()`.
- [ ] **Pending Step 2 — DTO & JSON Spawning:** Pembuatan `CharacterDto.cs` dan integrasi `System.Text.Json` di `Program.cs` untuk membaca masukan JSON.
- [ ] **Pending Step 3 — Factory Pattern:** Pembuatan `CharacterFactory.cs` untuk memuat entitas karakter secara otomatis dari DTO.
- [ ] **Pending Step 4 — Turn-Based Battle Engine:** Pembuatan `BattleSystem.cs` untuk mengelola alur giliran bertarung antar karakter hingga salah satu kalah.

---
