using System;

namespace RpgEngine
{
    /// <summary>
    /// Merepresentasikan efek status pendarahan (<c>Bleed</c>) yang memberikan kerusakan (*damage*) secara berkala kepada karakter di setiap giliran (*turn*).
    /// </summary>
    /// <remarks>
    /// Kelas ini merupakan turunan dari <see cref="StatusEffect"/> (konsep *IS-A*). 
    /// Kerusakan yang dihasilkan akan langsung mengurangi poin kesehatan karakter melalui metode <see cref="BaseCharacter.TakeDamage(int)"/>.
    /// </remarks>
    public class BleedEffect : StatusEffect
    {
        private int _healtAmount;

        /// <summary>
        /// Jumlah poin kerusakan (damage) pendarahan yang akan diterima oleh karakter di setiap giliran.
        /// </summary>
        /// <value>
        /// Nilai selalu dienkapsulasi agar bernilai minimal <c>1</c> untuk memastikan efek pendarahan selalu memberikan dampak.
        /// </value>
        public int HealthAmount
        {
            get => _healtAmount;
            private set => _healtAmount = Math.Max(1, value);
        }

        /// <summary>
        /// Menginisialisasi efek status pendarahan baru dengan durasi dan besaran kerusakan tertentu.
        /// </summary>
        /// <param name="duration">Jumlah giliran (*turn*) aktif untuk efek pendarahan ini.</param>
        /// <param name="healthAmount">Jumlah pengurangan poin kesehatan per giliran (harus bernilai minimal 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar jika parameter <paramref name="healthAmount"/> yang dimasukkan bernilai kurang dari 1.
        /// </exception>
        public BleedEffect(int duration, int healthAmount) : base("Bleed", duration)
        {
            if (healthAmount < 1) 
            { 
                throw new ArgumentOutOfRangeException(nameof(healthAmount), "Jumlah bleed effect tidak boleh kurang dari 1");
            }

            HealthAmount = healthAmount;
        }

        /// <summary>
        /// Eksekusi langsung efek pendarahan terhadap target karakter.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang sedang terkena efek pendarahan.</param>
        /// <remarks>
        /// Metode ini di-override dari kelas induk <see cref="StatusEffect"/> untuk memicu <see cref="BaseCharacter.TakeDamage(int)"/> sesuai dengan nilai <see cref="HealthAmount"/>.
        /// </remarks>
        public override void Apply(BaseCharacter target)
        {
            target.TakeDamage(HealthAmount);
        }
    }
}