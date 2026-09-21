using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.Json;
using Humanizer;

namespace SmallApp;

public class Program
{
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
/// Model DTO untuk menerima data masukan registrasi pengguna.
/// </summary>
public class RegisterPersonRequest
{
    /// <summary>
    /// Nama lengkap pengguna.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Umur pengguna dalam format teks (digit).
    /// </summary>
    public string? RawAge { get; set; }

    /// <summary>
    /// Alamat surel pengguna.
    /// </summary>
    public string? Email { get; set; }
}

/// <summary>
/// Kumpulan method ekstensi untuk validasi dan transformasi string.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Mengecek apakah teks bernilai null, kosong, atau hanya berisi spasi.
    /// </summary>
    public static bool IsNull(this string? text) => string.IsNullOrWhiteSpace(text);

    /// <summary>
    /// Validasi format alamat email menggunakan sintaks MailAddress.
    /// </summary>
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
    /// Mengubah format teks menjadi Title Case (huruf kapital di awal kata).
    /// </summary>
    public static string ToTitleCase(this string? text)
    {
        if (text.IsNull()) { return string.Empty; }

        text = text!.Trim().ToLower();
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
    }

    /// <summary>
    /// Mengonversi teks berisi angka menjadi tipe data integer.
    /// </summary>
    public static int TextToNumber(this string? text)
    {
        if (text.IsNull()) { return 0; }

        text = text!.Trim().ToLower();

        if (int.TryParse(text, out int digit)) { return digit; }

        return 0;
    }
}

/// <summary>
/// Domain model yang merepresentasikan entitas Pengguna/Person.
/// </summary>
public class Person
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public int Age { get; private set; }
    public string Email { get; private set; }

    /// <summary>
    /// Menyimpan total registrasi pengguna yang berhasil dibuat selama runtime.
    /// </summary>
    public static int TotalRegister { get; private set; } = 0;

    private Person(string fullName, int age, string email)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Age = age;
        Email = email;
    }

    /// <summary>
    /// Validasi request dan instansiasi objek Person baru jika valid.
    /// </summary>
    /// <param name="request">Data permintaan pendaftaran pengguna.</param>
    /// <param name="person">Output objek Person jika registrasi berhasil.</param>
    /// <param name="errorMessage">Pesan error jika terdapat kegagalan validasi.</param>
    /// <returns>Nilai true jika berhasil dibuat, sebaliknya false.</returns>
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
    /// Mengubah data objek Person menjadi string berformat JSON.
    /// </summary>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}