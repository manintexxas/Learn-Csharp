# SmallApp - Person Registration Console App

Aplikasi konsol sederhana menggunakan C# (.NET) untuk melakukan validasi dan registrasi data entitas pengguna (Person). Aplikasi ini memanfaatkan *extension methods* untuk pemrosesan string serta pola penciptaan objek aman (`TryCreate`).

---

## 📦 Library & Dependency

Proyek ini menggunakan library pendukung berikut:

- **[Humanizer](https://github.com/Humanizr/Humanizer)** (`Humanizer.Core`)
  - **Fungsi:** Digunakan untuk manipulasi dan format teks/string agar lebih ramah dibaca (*human-friendly*).
  - **Cara Install:**
    Jalankan perintah berikut di terminal/CLI direktori proyek:
    ```bash
    dotnet add package Humanizer.Core
    ```
    *Atau install versi paket lengkapnya:*
    ```bash
    dotnet add package Humanizer
    ```

- **`System.Text.Json`**
  - **Fungsi:** Library bawaan .NET untuk serialisasi objek `Person` menjadi format JSON.

- **`System.Net.Mail`**
  - **Fungsi:** Menggunakan class `MailAddress` bawaan .NET untuk validasi format email.

---

## 🛠️ Ringkasan Fitur & Fungsi

### 1. String Extensions (`StringExtensions`)
Kumpulan method pembantu untuk validasi dan penataan teks:

- `IsNull` : Mengecek apakah teks bernilai null, kosong, atau spasi.
- `IsValidEmail` : Mengecek keabsahan format alamat email.
- `ToTitleCase` : Mengubah teks menjadi format huruf kapital di setiap awal kata.
- `TextToNumber` : Mengonversi teks representasi angka menjadi tipe data `int`.

---

### 2. Pendaftaran Pengguna (`Person.TryCreate`)
Memproses pembuatan entitas `Person` dari `RegisterPersonRequest` dengan batasan validasi berikut:

- **Nama**: Tidak boleh kosong/null.
- **Umur**: Harus berupa angka dalam rentang `1` sampai `99`.
- **Email**: Harus sesuai dengan format surel yang valid.
- **Counter**: Menambahkan nilai `TotalRegister` secara otomatis jika pendaftaran berhasil.

---

## 🚀 Cara Menjalankan

1. Pastikan [.NET SDK 8.0](https://dotnet.microsoft.com/) atau versi terbaru sudah terpasang.
2. Install library yang dibutuhkan (Humanizer):
   ```bash
   dotnet add package Humanizer.Core