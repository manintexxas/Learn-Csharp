using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.Json;
using Humanizer;

namespace SmallApp;

/// <summary>
/// Kelas utama program yang berfungsi sebagai titik masuk (entry point) aplikasi CLI.
/// Bertanggung jawab untuk menyimulasikan skenario pendaftaran pengguna (sukses dan gagal).
/// </summary>
public class Program
{
    /// <summary>
    /// Eksekusi utama program untuk menguji alur validasi dan registrasi data pengguna.
    /// </summary>
    /// <param name="args">Argumen baris perintah (tidak digunakan dalam simulasi ini).</param>
    public static void Main(string[] args)
    {
        // Pengujian 1: Pendaftaran dengan data valid
        var request1 = new RegisterPersonRequest
        {
            FullName = "john DOE",
            RawAge = "25",
            Email = "john.doe@example.com"
        };

        if (Person.TryCreate(request1, out var person1, out var error1))
        {
            Console.WriteLine("=== Registrasi Berhasil ===");
            Console.WriteLine(person1!.ToJson());
        }
        else
        {
            Console.WriteLine($"Gagal: {error1}");
        }

        Console.WriteLine();

        // Pengujian 2: Pendaftaran dengan data tidak valid (Email salah & umur melebihi batas)
        var request2 = new RegisterPersonRequest
        {
            FullName = "jane doe",
            RawAge = "150",
            Email = "invalid-email"
        };

        if (Person.TryCreate(request2, out var person2, out var error2))
        {
            Console.WriteLine(person2!.ToJson());
        }
        else
        {
            Console.WriteLine($"=== Registrasi Gagal ===");
            Console.WriteLine($"Error: {error2}");
        }

        Console.WriteLine($"\nTotal Terdaftar: {Person.TotalRegister}");
    }
}

/// <summary>
/// Data Transfer Object (DTO) yang menampung data mentah masukan registrasi dari pengguna sebelum divalidasi.
/// </summary>
/// <remarks>
/// Kelas ini mengizinkan nilai <c>null</c> untuk mengantisipasi masukan pengguna yang tidak lengkap dari antarmuka/CLI.
/// </remarks>
public class RegisterPersonRequest
{
    /// <summary>
    /// Nama lengkap pengguna dalam format teks mentah.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Usia pengguna yang dimasukkan dalam bentuk teks digit (misal: "25").
    /// </summary>
    public string? RawAge { get; set; }

    /// <summary>
    /// Alamat surel (email) pengguna dalam format teks mentah.
    /// </summary>
    public string? Email { get; set; }
}

/// <summary>
/// Koleksi metode ekstensi untuk mempermudah validasi teks, konversi angka, dan pemformatan nama.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Memeriksa apakah sebuah variabel teks bernilai <c>null</c>, kosong, atau hanya berisi karakter spasi.
    /// </summary>
    /// <param name="text">Teks yang akan diperiksa (boleh null).</param>
    /// <returns><c>true</c> jika teks kosong/null; sebaliknya <c>false</c>.</returns>
    public static bool IsNull(this string? text) => string.IsNullOrWhiteSpace(text);

    /// <summary>
    /// Memvalidasi kelayakan format alamat email menggunakan kelas standar <see cref="MailAddress"/>.
    /// </summary>
    /// <param name="text">Teks alamat email yang akan diuji.</param>
    /// <returns><c>true</c> jika format email sesuai standar umum; sebaliknya <c>false</c>.</returns>
    public static bool IsValidEmail(this string? text)
    {
        if (text.IsNull()) { return false; }

        try
        {
            var address = new MailAddress(text!.Trim());
            return address.Address == text.Trim();
        }
        catch { return false; }
    }

    /// <summary>
    /// Mengubah teks menjadi format Title Case (setiap awal kata menggunakan huruf kapital).
    /// </summary>
    /// <param name="text">Teks nama mentah (misal: "john DOE").</param>
    /// <returns>Teks yang sudah dirapikan (misal: "John Doe"), atau string kosong jika masukan null.</returns>
    public static string ToTitleCase(this string? text)
    {
        if (text.IsNull()) { return string.Empty; }

        text = text!.Trim().ToLower();
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
    }

    /// <summary>
    /// Mengonversi teks berisi karakter angka menjadi tipe data numerik integer.
    /// </summary>
    /// <param name="text">Teks angka mentah.</param>
    /// <returns>Nilai angka dalam tipe <see cref="int"/>. Mengembalikan angka <c>0</c> jika gagal dikonversi.</returns>
    public static int TextToNumber(this string? text)
    {
        if (text.IsNull()) { return 0; }

        text = text!.Trim().ToLower();

        if (int.TryParse(text, out int digit)) { return digit; }

        return 0;
    }
}

/// <summary>
/// Domain model utama yang merepresentasikan pengguna sah di dalam sistem.
/// </summary>
/// <remarks>
/// Instansiasi kelas ini dibatasi melalui metode pabrik <see cref="TryCreate"/> 
/// guna memastikan objek <see cref="Person"/> yang tercipta selalu berada dalam kondisi valid.
/// </remarks>
public class Person
{
    /// <summary>
    /// Identitas unik global (GUID) yang dibuat otomatis untuk setiap pengguna terdaftar.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Nama lengkap pengguna yang sudah dirapikan ke format Title Case.
    /// </summary>
    public string FullName { get; private set; }

    /// <summary>
    /// Usia pengguna yang sah dalam hitungan tahun.
    /// </summary>
    public int Age { get; private set; }

    /// <summary>
    /// Alamat email terverifikasi milik pengguna.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Menghitung total pendaftaran pengguna yang berhasil dilakukan selama aplikasi berjalan.
    /// </summary>
    public static int TotalRegister { get; private set; } = 0;

    /// <summary>
    /// Konstruktor privat untuk mencegah pembuatan objek langsung dari luar tanpa melalui tahap validasi.
    /// </summary>
    private Person(string fullName, int age, string email)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Age = age;
        Email = email;
    }

    /// <summary>
    /// Mencoba memvalidasi data masukan dan membuat objek <see cref="Person"/> baru jika seluruh syarat terpenuhi.
    /// </summary>
    /// <param name="request">Objek berisi masukan data mentah registrasi.</param>
    /// <param name="person">Variabel output yang memuat objek <see cref="Person"/> jika registrasi berhasil; bernilai <c>null</c> jika gagal.</param>
    /// <param name="errorMessage">Variabel output berisi pesan penjelasan jika terjadi kegagalan validasi.</param>
    /// <returns><c>true</c> jika seluruh data valid dan objek berhasil dibuat; sebaliknya <c>false</c>.</returns>
    public static bool TryCreate(RegisterPersonRequest? request, out Person? person, out string? errorMessage)
    {
        if (request == null)
        {
            person = null;
            errorMessage = "Request must have value (Not Null)";
            return false;
        }

        if (request.FullName.IsNull())
        {
            person = null;
            errorMessage = "Invalid Name";
            return false;
        }

        string name = request.FullName!.ToTitleCase();
        int age = request.RawAge.TextToNumber();
        string email = request.Email ?? string.Empty;

        if (age <= 0 || age >= 100)
        {
            person = null;
            errorMessage = "Invalid Age";
            return false;
        }

        if (!email.IsValidEmail())
        {
            person = null;
            errorMessage = "Invalid Email";
            return false;
        }

        person = new Person(name, age, email);
        TotalRegister++;
        errorMessage = null;
        return true;
    }

    /// <summary>
    /// Mengonversi properti objek <see cref="Person"/> menjadi format teks JSON berseri.
    /// </summary>
    /// <returns>Teks string berformat JSON rapi (indented).</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}