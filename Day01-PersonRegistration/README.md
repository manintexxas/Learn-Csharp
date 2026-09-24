# Person Registration Module 🟢 Completed

> **Kategori:** Console Application / Input Validation Module  
> **Status Proyek:** Completed  
> **Target Pengguna:** C#/.NET Developers, Technical Recruiters, Peer Learners  

---

## 💡 Gambaran Umum & Tujuan Bisnis
**Person Registration Module** adalah modul aplikasi konsol C# yang dirancang untuk memproses, memvalidasi, dan merapikan data registrasi pengguna sebelum disimpan ke dalam model domain aplikasi. Modul ini mengatasi masalah inkonsistensi data masukan dari klien (seperti penulisan nama yang tidak teratur, format email tidak sah, atau nilai usia tidak valid).

Dengan mengadopsi pola **Try-Pattern** (`TryCreate`), modul ini menjamin integritas data domain sehingga tidak ada objek `Person` yang berhasil dibuat apabila datanya tidak memenuhi aturan validasi bisnis.

---

## ✨ Fitur Utama (Key Features)
- **Pembersihan & Pemformatan Nama Otomatis:** Mengonversi teks nama mentah menjadi format *Title Case* yang konsisten (misalnya `john DOE` menjadi `John Doe`) menggunakan *extension method* `ToTitleCase`.
- **Validasi Usia Aman:** Memastikan angka usia berada dalam batas rentang bisnis yang sah (1 hingga 99 tahun)[cite: 5].
- **Verifikasi Format Email:** Memeriksa kesesuaian struktur sintaks alamat email pengirim menggunakan `System.Net.Mail` (`MailAddress`)[cite: 5].
- **Penghitung Registrasi Real-time:** Menghitung total pendaftaran pengguna yang berhasil secara terpusat melalui properti statis `TotalRegister`[cite: 5].
- **Ekspor JSON:** Menyediakan metode `ToJson()` untuk mengonversi entitas domain pengguna menjadi format teks JSON terstruktur (*pretty-printed*)[cite: 5].

---

## 📐 Arsitektur & Tech Stack

### Tech Stack
- **Runtime & Language:** .NET 8.0 | C# 12
- **Core Libraries & Tools:** 
  - `System.Text.Json` (Serialisasi entitas domain menjadi teks JSON terindentasi)[cite: 5]
  - `System.Net.Mail` (Validasi sintaksis alamat email)[cite: 5]
  - `Humanizer` (Pustaka pembantu manipulasi dan pemformatan teks)[cite: 5]
  - `xUnit` / `NUnit` (Framework pengujian unit otomatis)

### Konsep Arsitektur / OOP
- **Encapsulation & Factory Method Pattern:** Konstruktor privat pada `Person` memastikan instansiasi objek hanya dapat dilakukan melalui metode statis `Person.TryCreate` setelah seluruh validasi terpenuhi[cite: 5].
- **Extension Methods & Try-Pattern:** Menyediakan fungsi utilitas sanitasi string pada `StringExtensions` (`IsNull`, `IsValidEmail`, `ToTitleCase`, `TextToNumber`)[cite: 5] serta menerapkan pola `TryCreate` berparameter `out` tanpa melempar *exception*[cite: 5].

---

## 🗓️ Catatan Perkembangan & 🚧 Progress Tracker (Work in Progress)

### Catatan Perkembangan (Development History)
* **Hari 1:** Implementasi DTO `RegisterPersonRequest`, pustaka utilitas `StringExtensions`, entitas domain `Person` dengan `TryCreate`, serta pembuatan proyek pengujian unit (`PersonRegistration.Tests`)[cite: 5].

### Daftar Status & Pending Features
- [x] **Hari 1 — Extension Methods:** Implementasi `IsNull`, `IsValidEmail`, `ToTitleCase`, dan `TextToNumber`[cite: 5].
- [x] **Hari 1 — Domain Entity & Factory:** Implementasi `Person.cs` dengan *private constructor*, validasi `TryCreate`, dan penghitung `TotalRegister`[cite: 5].
- [x] **Hari 1 — Ekspor JSON:** Implementasi metode `ToJson()` dengan `JsonSerializerOptions`[cite: 5].
- [x] **Hari 1 — Pengujian Unit:** Pembuatan proyek pengujian unit terisolasi pada `PersonRegistration.Tests`.

---
