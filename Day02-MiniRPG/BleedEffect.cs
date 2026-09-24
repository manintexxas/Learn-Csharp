using System;

namespace RpgEngine
{
    /// <summary>
    /// Mengelola efek status pendarahan (<c>Bleed</c>) yang memberikan pengurangan poin kesehatan secara berkala (*Damage over Time*) pada setiap giliran aksi karakter.
    /// </summary>
    /// <remarks>
    /// Kelas ini merupakan spesialisasi dari <see cref="StatusEffect"/> yang memicu pemanggilan <see cref="BaseCharacter.TakeDamage(int)"/> 
    /// secara otomatis ketika giliran berjalan. Efek ini bertindak sebagai mekanisme pengurangan HP berkala dalam kalkulasi pertarungan.
    /// </remarks>
    public class BleedEffect : StatusEffect
    {
        private int _healtAmount;

        /// <summary>
        /// Besaran nilai kerusakan (*damage*) pendarahan yang dikenakan ke karakter pada setiap giliran.
        /// </summary>
        /// <value>
        /// Angka bulat (<see cref="int"/>) yang selalu bernilai minimal <c>1</c> untuk menjamin efek pendarahan memberikan dampak nyata.
        /// </value>
        public int HealthAmount
        {
            get => _healtAmount;
            private set => _healtAmount = Math.Max(1, value);
        }

        /// <summary>
        /// Membentuk instansi baru dari efek pendarahan (<see cref="BleedEffect"/>) dengan batas durasi dan besaran pendarahan yang ditentukan.
        /// </summary>
        /// <param name="character">Subjek karakter (<see cref="BaseCharacter"/>) yang menjadi target penerima efek status.</param>
        /// <param name="duration">Sisa durasi aktif efek pendarahan dalam hitungan giliran (*turn*).</param>
        /// <param name="makDuration">Batas maksimum akumulasi durasi giliran yang diizinkan untuk efek ini.</param>
        /// <param name="healthAmount">Jumlah pengurangan poin kesehatan yang dihasilkan pada setiap giliran (minimal bernilai 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar apabila nilai parameter <paramref name="healthAmount"/> yang diberikan kurang dari 1.
        /// </exception>
        public BleedEffect(BaseCharacter character, int duration, int makDuration, int healthAmount) : base(character, "Bleed", duration, makDuration)
        {
            if (healthAmount < 1) 
            { 
                throw new ArgumentOutOfRangeException(nameof(healthAmount), "Jumlah bleed effect tidak boleh kurang dari 1");
            }

            HealthAmount = healthAmount;
        }

        /// <summary>
        /// Menerapkan eksekusi dampak pendarahan secara langsung terhadap kondisi kesehatan karakter target.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang mengalami pengurangan poin kesehatan akibat pendarahan.</param>
        /// <remarks>
        /// Metode ini meng-override <see cref="StatusEffect.Apply(BaseCharacter)"/> untuk mengeksekusi metode <see cref="BaseCharacter.TakeDamage(int)"/> 
        /// dengan memasukkan nilai <see cref="HealthAmount"/>.
        /// </remarks>
        public override void Apply(BaseCharacter target)
        {
            target.TakeDamage(HealthAmount);
        }
    }
}