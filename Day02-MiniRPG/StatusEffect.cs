using System;
using System.Data.Common;

namespace RpgEngine
{
    /// <summary>
    /// Cetak biru (<c>abstract class</c>) dasar untuk semua jenis efek status yang dapat dialami oleh karakter dalam permainan.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Efek status (seperti pendarahan/Bleed, pemulihan/Regen, atau Stun) memiliki batas durasi waktu/giliran tertentu. 
    /// Kelas ini menerapkan konsep **Abstraksi** dan **Polimorfisme** (konsep *IS-A*), di mana setiap efek spesifik 
    /// wajib menurunkan kelas ini dan mengimplementasikan metode <see cref="Apply(BaseCharacter)"/>.
    /// </para>
    /// </remarks>
    public abstract class StatusEffect
    {
        /// <summary>
        /// Nama identitas dari efek status (misalnya: "Bleed", "Regen", atau "Stun").
        /// </summary>
        public string Name { get; set; }

        private int _duration;

        /// <summary>
        /// Menandakan apakah efek status ini masih aktif berlaku pada karakter.
        /// </summary>
        /// <value>
        /// Bernilai <c>true</c> jika sisa giliran (<see cref="Duration"/>) lebih dari 0; sebaliknya <c>false</c> (efek sudah habis/kadaluwarsa).
        /// </value>
        public bool IsOnUse => Duration > 0;

        /// <summary>
        /// Sisa durasi giliran (*turn*) aktif untuk efek status ini.
        /// </summary>
        /// <value>
        /// Nilai selalu dienkapsulasi agar tidak pernah bernilai negatif (minimal <c>0</c>).
        /// </value>
        public int Duration
        {
            get => _duration;
            private set => _duration = Math.Max(0, value);
        }

        /// <summary>
        /// Menginisialisasi nilai dasar untuk efek status baru.
        /// </summary>
        /// <param name="name">Nama efek status (tidak boleh kosong atau hanya berisi spasi).</param>
        /// <param name="duration">Sisa jumlah giliran (*turn*) efek ini akan aktif (tidak boleh bernilai negatif).</param>
        /// <exception cref="ArgumentException">
        /// Dilempar jika parameter <paramref name="name"/> bernilai <c>null</c>, kosong, atau hanya berisi spasi.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar jika parameter <paramref name="duration"/> bernilai kurang dari 0.
        /// </exception>
        protected StatusEffect(string? name, int duration)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            { 
                throw new ArgumentException("Nama effect tidak boleh kosong", nameof(name)); 
            }

            if (duration < 0) 
            { 
                throw new ArgumentOutOfRangeException(nameof(duration), "Durasi tidak boleh negative");
            }

            Name = name;
            Duration = duration;
        }

        /// <summary>
        /// Menjalankan dampak atau aksi utama dari efek status terhadap target karakter.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang sedang menerima efek status ini.</param>
        /// <remarks>
        /// Metode ini bersifat <c>abstract</c> dan **wajib** diimplementasikan secara spesifik oleh kelas turunan 
        /// (misalnya: mengurangi HP pada <c>BleedEffect</c> atau menambah HP pada <c>RegenEffect</c>).
        /// </remarks>
        public abstract void Apply(BaseCharacter target);

        /// <summary>
        /// Mengurangi sisa durasi giliran (<see cref="Duration"/>) efek status sebanyak 1 poin.
        /// </summary>
        /// <remarks>
        /// Metode ini dipanggil oleh <see cref="StatusManager"/> setiap kali giliran (*turn*) karakter berganti atau diperbarui.
        /// </remarks>
        public void TickDuration() => Duration--;
    }
}