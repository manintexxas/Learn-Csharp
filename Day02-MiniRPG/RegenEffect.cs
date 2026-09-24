using System;

namespace RpgEngine
{
    /// <summary>
    /// Mengelola efek status pemulihan (<c>Regen</c>) yang memberikan peningkatan poin kesehatan secara berkala (*Heal over Time*) pada setiap giliran aksi karakter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kelas ini merupakan spesialisasi dari <see cref="StatusEffect"/> yang menerapkan prinsip *Inheritance / IS-A*.
    /// </para>
    /// <para>
    /// Berbeda dengan efek pendarahan (<see cref="BleedEffect"/>) yang mengurangi kesehatan, efek ini bertindak sebagai mekanisme dukungan (*support*) 
    /// dengan memicu pemanggilan <see cref="BaseCharacter.Heal(int)"/> secara otomatis untuk memulihkan kondisi target hingga batas maksimum kesehatannya.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var regen = new RegenEffect(hero, duration: 3, maxDuration: 5, healthAmount: 15);
    /// regen.Apply(hero); // Memulihkan 15 HP pada karakter hero
    /// </code>
    /// </example>
    public class RegenEffect : StatusEffect
    {
        private int _healthAmount;

        /// <summary>
        /// Besaran nilai pemulihan poin kesehatan (*heal*) yang diberikan kepada karakter target pada setiap giliran.
        /// </summary>
        /// <value>
        /// Angka bulat (<see cref="int"/>) yang selalu dienkapsulasi agar bernilai minimal <c>1</c> untuk menjamin efek pemulihan memberikan dampak nyata.
        /// </value>
        public int HealthAmount
        {
            get => _healthAmount;
            private set => _healthAmount = Math.Max(1, value);
        }

        /// <summary>
        /// Membentuk instansi baru dari efek pemulihan (<see cref="RegenEffect"/>) dengan batas durasi dan besaran regenerasi yang ditentukan.
        /// </summary>
        /// <param name="character">Subjek karakter (<see cref="BaseCharacter"/>) yang menjadi target penerima efek pemulihan.</param>
        /// <param name="duration">Sisa durasi aktif efek pemulihan dalam hitungan giliran (*turn*).</param>
        /// <param name="maxDuration">Batas maksimum akumulasi durasi giliran yang diizinkan untuk efek ini.</param>
        /// <param name="healthAmount">Jumlah poin kesehatan yang dipulihkan pada setiap giliran (minimal bernilai 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar apabila nilai parameter <paramref name="healthAmount"/> yang diberikan kurang dari 1.
        /// </exception>
        public RegenEffect(BaseCharacter character, int duration, int maxDuration,int healthAmount) : base(character, "Regen", duration, maxDuration)
        {
            if (healthAmount < 1) 
            { 
                throw new ArgumentOutOfRangeException(nameof(healthAmount), "Jumlah health regen tidak boleh kurang dari 1");
            }

            HealthAmount = healthAmount;
        }

        /// <summary>
        /// Menerapkan eksekusi dampak regenerasi secara langsung terhadap kondisi kesehatan karakter target.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang mengalami pemulihan poin kesehatan akibat efek regenerasi.</param>
        /// <remarks>
        /// Metode ini meng-override <see cref="StatusEffect.Apply(BaseCharacter)"/> untuk mengeksekusi metode <see cref="BaseCharacter.Heal(int)"/> 
        /// dengan memasukkan nilai <see cref="HealthAmount"/>.
        /// </remarks>
        public override void Apply(BaseCharacter target)
        {
            target.Heal(HealthAmount);
        }
    }
}