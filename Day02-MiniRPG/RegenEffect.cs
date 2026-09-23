using System;

namespace RpgEngine
{
    /// <summary>
    /// Merepresentasikan efek status pemulihan (<c>Regen</c>) yang memberikan pemulihan kesehatan (*heal*) secara berkala kepada karakter di setiap giliran (*turn*).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kelas ini merupakan turunan dari <see cref="StatusEffect"/> (menerapkan prinsip *Inheritance / IS-A*).
    /// </para>
    /// <para>
    /// Berbeda dengan pendarahan (<c>BleedEffect</c>) yang mengurangi darah, efek ini berfokus pada regenerasi poin kesehatan 
    /// dengan memicu metode <see cref="BaseCharacter.Heal(int)"/> di setiap giliran aktifnya.
    /// </para>
    /// </remarks>
    public class RegenEffect : StatusEffect
    {
        private int _healtAmount;

        /// <summary>
        /// Jumlah poin pemulihan kesehatan (HP) yang akan diberikan kepada karakter di setiap giliran.
        /// </summary>
        /// <value>
        /// Nilai selalu dienkapsulasi agar bernilai minimal <c>1</c> untuk memastikan efek regenerasi selalu memberikan pemulihan yang valid.
        /// </value>
        public int HealthAmount
        {
            get => _healtAmount;
            private set => _healtAmount = Math.Max(1, value);
        }

        /// <summary>
        /// Menginisialisasi efek status regenerasi baru dengan durasi giliran dan besaran pemulihan tertentu.
        /// </summary>
        /// <param name="duration">Jumlah giliran (*turn*) aktif untuk efek pemulihan ini.</param>
        /// <param name="healthAmount">Jumlah poin kesehatan yang dipulihkan per giliran (harus bernilai minimal 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar jika parameter <paramref name="healthAmount"/> yang dimasukkan bernilai kurang dari 1.
        /// </exception>
        public RegenEffect(int duration, int healthAmount) : base("Regen", duration)
        {
            if (healthAmount < 1) 
            { 
                throw new ArgumentOutOfRangeException(nameof(healthAmount), "Jumlah health regen tidak boleh kurang dari 1");
            }

            HealthAmount = healthAmount;
        }

        /// <summary>
        /// Eksekusi langsung efek regenerasi terhadap target karakter.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang sedang menerima efek pemulihan.</param>
        /// <remarks>
        /// Metode ini di-override dari kelas induk <see cref="StatusEffect"/> untuk memicu <see cref="BaseCharacter.Heal(int)"/> sesuai dengan nilai <see cref="HealthAmount"/>.
        /// </remarks>
        public override void Apply(BaseCharacter target)
        {
            target.Heal(HealthAmount);
        }
    }
}