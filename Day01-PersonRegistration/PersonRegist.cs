using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.Json;
using Humanizer;

namespace SmallApp
{
    /// <summary>
    /// Menampung masukan data mentah pendaftaran pengguna dari luar sistem (seperti antarmuka pengguna atau API) sebelum diproses dan divalidasi oleh logika bisnis.
    /// </summary>
    public class RegisterPersonRequest
    {
        /// <summary>
        /// Nama lengkap pengguna dalam bentuk teks mentah.
        /// </summary>
        /// <value>
        /// Teks nama masukan pengguna. Bernilai <c>null</c> jika tidak diberikan oleh klien.
        /// </value>
        public string? FullName { get; set; }

        /// <summary>
        /// Usia pengguna yang dimasukkan dalam bentuk teks digit mentah (misalnya: "25").
        /// </summary>
        /// <value>
        /// Teks representasi angka usia. Bernilai <c>null</c> jika tidak diberikan oleh klien.
        /// </value>
        public string? RawAge { get; set; }

        /// <summary>
        /// Alamat surat elektronik (email) pengguna dalam bentuk teks mentah.
        /// </summary>
        /// <value>
        /// Teks alamat email masukan. Bernilai <c>null</c> jika tidak diberikan oleh klien.
        /// </value>
        public string? Email { get; set; }
    }

    /// <summary>
    /// Memfasilitasi sanitasi teks, penanganan nilai <c>null</c>, validasi format email, serta konversi tipe data masukan mentah.
    /// </summary>
    /// <remarks>
    /// Kelas pembantu statis ini menyediakan *extension methods* untuk memastikan pengolahan string dilakukan secara konsisten di seluruh aplikasi sebelum dimasukkan ke dalam objek domain.
    /// </remarks>
    public static class StringExtensions
    {
        /// <summary>
        /// Memeriksa apakah sebuah variabel teks bernilai <c>null</c>, kosong, atau hanya terdiri dari karakter spasi untuk mencegah kesalahan operasi manipulasi string.
        /// </summary>
        /// <param name="text">Teks masukan yang akan diperiksa keabsahannya. Dapat bernilai <c>null</c>.</param>
        /// <returns>
        /// Mengembalikan <c>true</c> jika teks bernilai <c>null</c>, <see cref="string.Empty"/>, atau hanya berisi karakter spasi; sebaliknya mengembalikan <c>false</c>.
        /// </returns>
        public static bool IsNull(this string? text) => string.IsNullOrWhiteSpace(text);

        /// <summary>
        /// Memvalidasi kesesuaian struktur alamat email pengguna dengan standar sintaks *Internet Message Format* guna menjamin data email dapat dihubungi.
        /// </summary>
        /// <param name="text">Teks alamat email mentah yang diuji (misalnya: "user@example.com"). Dapat bernilai <c>null</c>.</param>
        /// <returns>
        /// Mengembalikan <c>true</c> jika alamat email memiliki format yang sah menurut standar <see cref="MailAddress"/>; sebaliknya mengembalikan <c>false</c> jika format abnormal atau bernilai <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Operasi ini secara otomatis melakukan *trimming* spasi di awal dan akhir teks sebelum memvalidasi.
        /// </remarks>
        /// <example>
        /// <code>
        /// string input = " user@domain.com ";
        /// bool isValid = input.IsValidEmail(); // Hasil: true
        /// </code>
        /// </example>
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
        /// Mengubah format penulisan teks menjadi *Title Case* (huruf awal kapital pada setiap kata) guna standardisasi tampilan data nama pengguna.
        /// </summary>
        /// <param name="text">Teks nama mentah yang akan dirapikan (misalnya: "jOHN dOE"). Dapat bernilai <c>null</c>.</param>
        /// <returns>
        /// Teks yang telah dirapikan (misalnya: "John Doe"), atau <see cref="string.Empty"/> jika masukan bernilai <c>null</c> atau kosong.
        /// </returns>
        /// <remarks>
        /// Seluruh karakter diubah menjadi huruf kecil (*lowercase*) terlebih dahulu untuk mencegah kesalahan kapitalisasi acak sebelum mengaplikasikan <see cref="CultureInfo.CurrentCulture"/>.
        /// </remarks>
        public static string ToTitleCase(this string? text)
        {
            if (text.IsNull()) { return string.Empty; }

            text = text!.Trim().ToLower();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        /// <summary>
        /// Mengonversi teks yang merepresentasikan angka digit menjadi tipe data integer sah untuk keperluan kalkulasi dan validasi numerik.
        /// </summary>
        /// <param name="text">Teks berkarakter angka mentah yang akan dikonversi (misalnya: "25"). Dapat bernilai <c>null</c>.</param>
        /// <returns>
        /// Nilai numerik integer bertipe <see cref="int"/>. Mengembalikan angka <c>0</c> jika konversi gagal atau jika masukan bernilai <c>null</c>.
        /// </returns>
        public static int TextToNumber(this string? text)
        {
            if (text.IsNull()) { return 0; }

            text = text!.Trim().ToLower();

            if (int.TryParse(text, out int digit)) { return digit; }

            return 0;
        }
    }

    /// <summary>
    /// Merepresentasikan entitas domain pengguna terverifikasi di dalam sistem dengan perlindungan integritas data (*invariants*).
    /// </summary>
    /// <remarks>
    /// Menggunakan konstruktor privat dan pola *Factory Method* (<see cref="TryCreate"/>) untuk menjamin bahwa pembentukan objek hanya terjadi saat seluruh aturan validasi bisnis terpenuhi.
    /// Properti statis <see cref="TotalRegister"/> bersifat non-thread-safe untuk operasi inkremen konkuren tanpa penguncian eksternal.
    /// </remarks>
    /// <example>
    /// <code>
    /// var request = new RegisterPersonRequest { FullName = "budi santoso", RawAge = "28", Email = "budi@example.com" };
    /// if (Person.TryCreate(request, out Person? person, out string? errorMessage))
    /// {
    ///     Console.WriteLine($"Pengguna berhasil dibuat: {person.FullName} (ID: {person.Id})");
    /// }
    /// </code>
    /// </example>
    public class Person
    {
        /// <summary>
        /// Identitas unik acak berstandar global (GUID) yang diterbitkan secara otomatis saat entitas diciptakan.
        /// </summary>
        /// <value>
        /// Struktur <see cref="Guid"/> unik sebagai kunci utama penjelas identitas entitas.
        /// </value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Nama lengkap resmi pengguna yang telah disanitasi dan diformat menjadi *Title Case*.
        /// </summary>
        /// <value>
        /// Teks nama terstandar yang konsisten.
        /// </value>
        public string FullName { get; private set; }

        /// <summary>
        /// Usia sah pengguna dalam satuan tahun yang telah melalui batas rentang validasi bisnis.
        /// </summary>
        /// <value>
        /// Angka bulat positif dalam rentang 1 hingga 99 tahun.
        /// </value>
        public int Age { get; private set; }

        /// <summary>
        /// Alamat surat elektronik (email) resmi terverifikasi milik pengguna.
        /// </summary>
        /// <value>
        /// Teks alamat email yang memenuhi standar validitas sintaks.
        /// </value>
        public string Email { get; private set; }

        /// <summary>
        /// Menghitung akumulasi total pendaftaran pengguna yang berhasil diciptakan sepanjang sesi aplikasi berjalan.
        /// </summary>
        /// <value>
        /// Jumlah entitas <see cref="Person"/> yang berhasil dibuat melalui metode <see cref="TryCreate"/>.
        /// </value>
        /// <remarks>
        /// Properti statis ini bertambah (+1) secara otomatis setiap kali pembuatan objek baru berhasil dilakukan.
        /// </remarks>
        public static int TotalRegister { get; private set; } = 0;

        /// <summary>
        /// Konstruktor privat untuk menetapkan atribut dasar entitas <see cref="Person"/> secara internal.
        /// </summary>
        /// <param name="fullName">Nama lengkap pengguna yang sudah diformat.</param>
        /// <param name="age">Usia pengguna yang sudah divalidasi.</param>
        /// <param name="email">Alamat email pengguna yang sudah divalidasi.</param>
        private Person(string fullName, int age, string email)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Age = age;
            Email = email;
        }

        /// <summary>
        /// Memvalidasi seluruh masukan data pendaftaran dan membentuk entitas <see cref="Person"/> baru jika seluruh aturan bisnis terpenuhi tanpa melemparkan eksepsi (*exception*).
        /// </summary>
        /// <param name="request">Objek masukan data registrasi mentah pengguna (<see cref="RegisterPersonRequest"/>). Dapat bernilai <c>null</c>.</param>
        /// <param name="person">Parameter keluaran (<c>out</c>) yang menampung entitas <see cref="Person"/> baru jika pendaftaran berhasil; bernilai <c>null</c> jika terjadi kegagalan validasi.</param>
        /// <param name="errorMessage">Parameter keluaran (<c>out</c>) yang memuat deskripsi pesan kesalahan bisnis jika validasi gagal; bernilai <c>null</c> jika sukses.</param>
        /// <returns>
        /// Mengembalikan <c>true</c> apabila seluruh aturan bisnis terpenuhi dan objek berhasil diciptakan; sebaliknya mengembalikan <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Aturan validasi bisnis mencakup:
        /// <list type="number">
        /// <item><description>Objek <paramref name="request"/> dan properti <c>FullName</c> tidak boleh <c>null</c> atau kosong.</description></item>
        /// <item><description>Nilai usia (<c>Age</c>) harus berupa angka numerik valid dalam rentang > 0 dan &lt; 100 tahun.</description></item>
        /// <item><description>Format <c>Email</c> harus terverifikasi sah menurut standar sintaks email.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// if (!Person.TryCreate(req, out var person, out var err))
        /// {
        ///     Console.WriteLine($"Gagal membuat pengguna: {err}");
        /// }
        /// </code>
        /// </example>
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
        /// Mengonversi seluruh atribut entitas <see cref="Person"/> menjadi format teks terstruktur berbasis JSON untuk keperluan serialisasi, transmisi data, atau pembuatan log.
        /// </summary>
        /// <returns>
        /// Teks berformat JSON terindentasi rapi (*pretty-printed*).
        /// </returns>
        /// <remarks>
        /// Menggunakan <see cref="JsonSerializer"/> dengan opsi <see cref="JsonSerializerOptions.WriteIndented"/> dari namespace <see cref="System.Text.Json"/>.
        /// </remarks>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}