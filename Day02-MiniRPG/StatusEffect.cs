using System;

namespace RpgEngine
{
    /// <summary>
    /// Cetak biru abstrak (<c>abstract class</c>) dasar yang merepresentasikan efek status (*status effect*) pada karakter dalam sistem pertarungan RPG.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Efek status (seperti pendarahan/Bleed, pemulihan/Regen, atau Stun) memiliki masa berlaku berbasis giliran (*turn-based duration*). 
    /// Kelas ini menerapkan konsep **Abstraksi** dan **Polimorfisme** (prinsip *IS-A*), di mana setiap efek spesifik 
    /// wajib merintisi kelas ini dan mengimplementasikan metode eksekusi dampak <see cref="Apply(BaseCharacter)"/>.
    /// </para>
    /// </remarks>
    public abstract class StatusEffect
    {
        /// <summary>
        /// Karakter target utama yang terikat dan menerima dampak dari efek status ini.
        /// </summary>
        /// <value>
        /// Objek <see cref="BaseCharacter"/> penderita atau penerima manfaat efek.
        /// </value>
        public BaseCharacter Character { get; set; }

        /// <summary>
        /// Nama identitas resmi dari efek status (misalnya: "Bleed", "Regen", atau "Stun").
        /// </summary>
        /// <value>
        /// String nama deskriptif efek status.
        /// </value>
        public string Name { get; set; }

        private int _duration;

        /// <summary>
        /// Menandakan status keaktifan efek terhadap karakter.
        /// </summary>
        /// <value>
        /// Bernilai <c>true</c> jika sisa giliran (<see cref="Duration"/>) lebih besar dari 0; sebaliknya bernilai <c>false</c> jika durasi telah habis.
        /// </value>
        public bool IsOnUse => Duration > 0;

        /// <summary>
        /// Batas maksimum akumulasi durasi giliran (*turn*) yang diizinkan untuk efek status ini.
        /// </summary>
        /// <value>
        /// Angka bulat positif yang menjadi acuan batas atas saat melakukan pembaruan durasi via <see cref="RefreshDuration"/>.
        /// </value>
        public int MaxDuration { get; private set; }

        /// <summary>
        /// Sisa durasi giliran (*turn*) aktif untuk efek status ini.
        /// </summary>
        /// <value>
        /// Nilai selalu dienkapsulasi menggunakan <see cref="Math.Clamp(int, int, int)"/> agar nilainya terisolasi dalam rentang <c>0</c> hingga <see cref="MaxDuration"/>.
        /// </value>
        public int Duration
        {
            get => _duration;
            private set => _duration = Math.Clamp(value, 0, MaxDuration);
        }

        /// <summary>
        /// Inisialisasi atribut dasar untuk objek efek status baru.
        /// </summary>
        /// <param name="character">Objek karakter (<see cref="BaseCharacter"/>) penerima efek status.</param>
        /// <param name="name">Nama identitas efek status (tidak boleh <c>null</c>, kosong, atau hanya berisi spasi).</param>
        /// <param name="duration">Sisa jumlah giliran (*turn*) awal saat efek diaktifkan (harus bernilai tidak negatif).</param>
        /// <param name="maxDuration">Batas maksimum giliran yang diperbolehkan untuk efek ini (harus bernilai tidak negatif).</param>
        /// <exception cref="ArgumentException">
        /// Dilempar jika parameter <paramref name="name"/> bernilai <c>null</c>, kosong, atau hanya berisi spasi.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar jika parameter <paramref name="duration"/> atau <paramref name="maxDuration"/> bernilai kurang dari 0.
        /// </exception>
        protected StatusEffect(BaseCharacter character, string? name, int duration, int maxDuration)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            { 
                throw new ArgumentException("Nama effect tidak boleh kosong", nameof(name)); 
            }

            if (duration < 0) 
            { 
                throw new ArgumentOutOfRangeException(nameof(duration), "Durasi tidak boleh negative");
            }

            if (maxDuration < 0) 
            { 
                throw new ArgumentOutOfRangeException(nameof(maxDuration), "Max Durasi tidak boleh negative");
            }

            Character = character;
            Name = name;
            MaxDuration = maxDuration;
            Duration = duration;
        }

        /// <summary>
        /// Mengeksekusi dampak atau aksi utama dari efek status terhadap target karakter.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang sedang menerima eksekusi dampak efek status.</param>
        /// <remarks>
        /// Metode ini bersifat <c>abstract</c> dan **wajib** diimplementasikan secara spesifik oleh kelas turunan 
        /// (misalnya: mengurangi HP pada <c>BleedEffect</c> atau memulihkan HP pada <c>RegenEffect</c>).
        /// </remarks>
        public abstract void Apply(BaseCharacter target);

        /// <summary>
        /// Mengurangi sisa durasi giliran (<see cref="Duration"/>) efek status sebanyak 1 poin.
        /// </summary>
        /// <remarks>
        /// Metode ini dipanggil oleh <see cref="StatusManager"/> secara otomatis pada setiap pergantian giliran aksi karakter.
        /// </remarks>
        public void TickDuration() => Duration--;

        /// <summary>
        /// Memulihkan sisa durasi giliran (<see cref="Duration"/>) kembali ke batas maksimumnya (<see cref="MaxDuration"/>).
        /// </summary>
        /// <remarks>
        /// Digunakan saat efek status serupa diaplikasikan ulang ke karakter untuk memperpanjang usia efek tanpa membuat objek efek baru (*refresh duration*).
        /// </remarks>
        public void RefreshDuration() => Duration = MaxDuration;
    }
}