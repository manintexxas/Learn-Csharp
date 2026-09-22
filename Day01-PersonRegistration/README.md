# 👤 Day 01 - Person Registration App

Aplikasi konsol sederhana menggunakan C# (.NET 8) untuk melakukan validasi dan registrasi data entitas pengguna (`Person`). Proyek ini berfokus pada penerapan *Extension Methods*, enkapsulasi domain model dengan *private constructor*, serta pola penciptaan objek aman (`TryCreate`).

---

## 📦 Library & Dependency

Proyek ini menggunakan library dan namespace pendukung berikut:

- **[Humanizer](https://github.com/Humanizr/Humanizer)** (`Humanizer.Core`)
  - **Fungsi:** Manipulasi dan penataan format teks/string agar lebih ramah dibaca (*human-friendly*).
  - **Cara Install:**
    ```bash
    dotnet add package Humanizer.Core
    ```

- **`System.Text.Json`**
  - **Fungsi:** Library bawaan .NET untuk serialisasi objek `Person` menjadi format JSON.

- **`System.Net.Mail`**
  - **Fungsi:** Menggunakan kelas `MailAddress` untuk validasi keabsahan format email.

---

## 🛠️ Ringkasan Fitur & Fungsi

### 1. String Extensions (`StringExtensions`)
Kumpulan *extension method* pembantu untuk validasi dan transformasi teks:

- `IsNull` : Mengecek apakah teks bernilai null, kosong, atau spasi.
- `IsValidEmail` : Validasi format alamat email menggunakan `MailAddress`.
- `ToTitleCase` : Mengubah teks menjadi format huruf kapital di setiap awal kata.
- `TextToNumber` : Mengonversi teks angka menjadi tipe data `int`.

---

### 2. Pendaftaran Pengguna (`Person.TryCreate`)
Memproses pembuatan entitas `Person` dari `RegisterPersonRequest` dengan aturan validasi:

- **Nama**: Tidak boleh kosong/null.
- **Umur**: Harus berupa angka dalam rentang `1` sampai `99`.
- **Email**: Harus sesuai dengan format surel yang valid.
- **Counter**: Menambahkan nilai `TotalRegister` secara otomatis jika registrasi berhasil.

---

## 🚀 Cara Menjalankan

1. Pastikan [.NET SDK 8.0](https://dotnet.microsoft.com/) sudah terpasang.
2. Jalankan perintah berikut dari direktori root repositori:

```bash
dotnet run --project Day01-PersonRegistration