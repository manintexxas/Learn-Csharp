# ⚔️ Day 02 - MiniRPG Engine 🚧 (Work in Progress)

> **Catatan Proyek (Overtime Project):**  
> Proyek ini berkembang menjadi proyek multi-tahap. Pengembangan diawali dari abstraksi entitas dasar karakter (Hari 1) hingga perluasan sistem efek status secara dinamis menggunakan interface dan komposisi objek (Hari 2).

---

## 🎯 Gambaran Umum Proyek

**MiniRPG Engine** adalah simulasi *Core Engine* permainan bertema *Turn-Based RPG* berbasis konsol C# (.NET 8). Proyek ini dirancang untuk melatih dan menguji penerapan tingkat menengah dari **Object-Oriented Programming (OOP)**, pemisahan logika (*Separation of Concerns*), serta pengujian unit (*Unit Testing*).

Aplikasi ini menerima data konfigurasi karakter melalui format **JSON**, memprosesnya secara dinamis, serta menguji status dan efek pertarungan (seperti pendarahan, regenerasi, dan stun) selama pertarungan berlangsung.

---

## 💡 Konsep OOP & Arsitektur Utama

Proyek ini mendemonstrasikan hubungan antarkelas menggunakan dua prinsip utama OOP:
1. **IS-A (Inheritance / Pewarisan):** `Hero` dan `Monster` adalah turunan dari `BaseCharacter`. Begitu pula `BleedEffect`, `RegenEffect`, dan `StunEffect` yang merupakan turunan dari `StatusEffect`.
2. **HAS-A (Composition / Komposisi):** `BaseCharacter` memiliki pengelola efek (`StatusManager`), yang menampung kumpulan efek (`StatusEffect`) aktif pada karakter tersebut.

---

## 🗓️ Catatan Perkembangan (Development History)

### 🔹 Hari 1: Fondasi Entitas & Abstraksi (`BaseCharacter`)
* **Abstraksi & Encapsulation:** Membuat *abstract class* `BaseCharacter` sebagai cetak biru seluruh karakter (Hero/Monster).
* **Proteksi Nilai Sakelar (`Math.Clamp`):** Memastikan nilai kesehatan (`HealthPoint`) tidak pernah bernilai negatif atau melebihi batas maksimum (`MaxHealth`).
* **Siklus Hidup Karakter:** Implementasi *read-only property* `IsAlive` serta metode *virtual* `TakeDamage()` dan `Heal()`.
* **Polimorfisme:** Mendeklarasikan *abstract method* `UniqueSkill()` yang wajib diimplementasikan oleh kelas turunan.

### 🔹 Hari 2: Sistem Efek Status (`Interface` & Komposisi)
* **Penerapan Interface (`IUseable`):** Mendefinisikan kontrak perilaku untuk item atau efek yang dapat diaplikasikan/digunakan pada karakter.
* **Efek Berkelanjutan (Status Effect System):**
  * `StatusEffect` *(Abstract Base)*: Menjadi dasar seluruh efek status yang memiliki durasi giliran (*turns*).
  * `BleedEffect` *(Damage Over Time)*: Mengurangi HP karakter di setiap giliran.
  * `RegenEffect` *(Heal Over Time)*: Memulihkan HP karakter di setiap giliran.
  * `StunEffect` *(Crowd Control)*: Menyebabkan karakter kehilangan giliran beraksi.
* **Pengelola Efek (`StatusManager`):** Mengatur penambahan, pembaruan durasi, eksekusi efek, dan pembersihan efek yang telah habis masa berlakunya pada karakter.
* **Pengujian Unit (`Test/`):** Penambahan *Unit Test* (`BaseCharacterTest.cs` dan `EffectTest.cs`) untuk memverifikasi logika HP dan efek status berjalan dengan tepat.

---

## 🚧 Progress Tracker & Daftar Status (Pending Features)

- [x] **Hari 1 — Abstraksi Dasar:** `BaseCharacter.cs` & validasi proteksi HP (`Math.Clamp`).
- [x] **Hari 2 — Sistem Efek Status:** `IUseable.cs`, `StatusEffect.cs`, `StatusManager.cs`, serta kelas efek spesifik (`Bleed`, `Regen`, `Stun`).
- [x] **Hari 2 — Pengujian Unit:** Pembuatan pengujian otomatis di direktori `Test/`.
- [ ] **Pending Step 1 — Concrete Classes:** Implementasi kelas turunan konkret `Hero.cs` dan `Monster.cs` beserta logika `UniqueSkill()`.
- [ ] **Pending Step 2 — DTO & JSON Spawning:** Pembuatan `CharacterDto.cs` dan integrasi `System.Text.Json` di `Program.cs` untuk membaca masukan JSON.
- [ ] **Pending Step 3 — Factory Pattern:** Pembuatan `CharacterFactory.cs` untuk memuat entitas karakter secara otomatis dari DTO.
- [ ] **Pending Step 4 — Turn-Based Battle Engine:** Pembuatan `BattleSystem.cs` untuk mengelola alur giliran bertarung antar karakter hingga salah satu kalah.

---

## 📦 Pustaka & Dependensi

* **`System.Text.Json`** *(Untuk integrasi lanjutan)*: Membaca dan mengonversi masukan teks JSON menjadi data DTO karakter.
* **`System.Math`**: Digunakan pada metode `Math.Clamp` guna menjaga konsistensi kalkulasi statistik karakter.

---

## 🚀 Cara Menjalankan & Pengujian

### 1. Menjalankan Simulasi Utama
Pastikan Anda berada di direktori root repositori, lalu jalankan perintah:

```bash
dotnet run --project lab/Day02-MiniRPG