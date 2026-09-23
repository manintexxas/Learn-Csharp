# 📝 Day 01 - Person Registration Module

Aplikasi konsol C# sederhana yang dirancang untuk memproses, memvalidasi, dan merapikan data registrasi pengguna sebelum disimpan ke dalam domain model aplikasi.

---

## 🎯 Gambaran Umum Proyek

Modul ini menyelesaikan masalah data masukan pengguna yang sering kali tidak konsisten (misalnya penulisan nama yang berantakan, format email yang salah, atau angka usia yang tidak masuk akal). 

Dengan menerapkan pola **Try-Pattern** (`TryCreate`), sistem memastikan bahwa tidak ada objek pengguna (`Person`) yang berhasil dibuat jika datanya tidak memenuhi standar keabsahan bisnis.

---

## ✨ Fitur Utama

* **Pembersihan & Pemformatan Nama Otomatis:** Mengubah input nama seperti `john DOE` menjadi format konsisten `John Doe` (*Title Case*).
* **Validasi Usia Aman:** Memastikan usia berupa angka yang masuk akal (rentang antara 1 hingga 99 tahun).
* **Verifikasi Format Email:** Memeriksa keabsahan format alamat email pengirim menggunakan pustaka standar .NET.
* **Penghitung Registrasi Real-time:** Menghitung berapa banyak pengguna yang berhasil mendaftar selama aplikasi berjalan via properti statis.
* **Ekspor JSON:** Menyediakan metode instan untuk mengubah data domain pengguna menjadi format teks JSON yang rapi.

---

## 🛠️ Arsitektur & Pustaka Terkait

| Komponen / Pustaka | Versi / Fungsi |
| :--- | :--- |
| **.NET SDK** | .NET 8.0 (atau lebih baru) |
| **`System.Text.Json`** | Serialisasi data domain menjadi teks JSON |
| **`Humanizer`** | Ekstensi bantuan pemformatan teks |
| **`System.Net.Mail`** | Validasi struktur sintaks email |

---

## 🚀 Langkah Memulai (Getting Started)

Ikuti langkah-langkah berikut untuk menjalankan proyek di komputer lokal Anda:

### 1. Prasyarat
Pastikan Anda sudah menginstal [.NET SDK](https://dotnet.microsoft.com/download) di perangkat Anda.

### 2. Buka Terminal / Command Prompt
Navigasikan direktori terminal Anda ke folder proyek `Day01-PersonRegistration`:

```bash
cd lab/Day01-PersonRegistration