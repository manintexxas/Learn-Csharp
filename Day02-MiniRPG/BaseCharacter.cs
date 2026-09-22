using System;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace RpgEngine
{
    /// <summary>
    /// Kelas abstrak dasar yang mewakili entitas karakter dalam Game RPG.
    /// Menyediakan atribut dasar, enkapsulasi status kesehatan, serta aksi dasar (Serang, Heal, Skill).
    /// </summary>
    public abstract class BaseCharacter
    {
        private int _healthPoint;

        /// <summary>
        /// Menandakan apakah karakter masih hidup berdasarkan nilai HealthPoint.
        /// </summary>
        public bool IsAlive => HealthPoint > 0;

        /// <summary>
        /// Nama dari karakter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Batas maksimum HealthPoint yang dimiliki karakter.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Nilai kekuatan serangan dasar karakter.
        /// </summary>
        public int AttackPower { get; protected set; }

        /// <summary>
        /// Poin kesehatan karakter saat ini. Nilai dibatasi secara otomatis antara 0 hingga MaxHealth.
        /// </summary>
        public int HealthPoint
        {
            get => _healthPoint;
            protected set => _healthPoint = Math.Clamp(value, 0, MaxHealth);
        }

        /// <summary>
        /// Menginisialisasi nilai dasar untuk atribut karakter.
        /// </summary>
        /// <param name="name">Nama karakter (tidak boleh null atau spasi).</param>
        /// <param name="maxHealth">Batas HP maksimum (harus lebih dari 0).</param>
        /// <param name="attackPower">Kekuatan serangan dasar (tidak boleh negatif).</param>
        /// <exception cref="ArgumentException">Dilempar jika nama bernilai null atau spasi.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Dilempar jika maxHealth/attackPower berada di luar jangkauan yang valid.</exception>
        protected BaseCharacter(string? name, int maxHealth, int attackPower)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            { 
                throw new ArgumentException("Nama Karakter tidak boleh kosong.", nameof(name)); 
            }

            if (maxHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHealth), "Health awal harus lebih dari 0");
            }

            if (attackPower < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(attackPower), "Attack Power tidak boleh minus");
            }

            Name = name;
            MaxHealth = maxHealth;
            AttackPower = attackPower;
            HealthPoint = maxHealth;
        }

        /// <summary>
        /// Skill unik yang wajib diimplementasikan oleh setiap kelas turunan.
        /// </summary>
        /// <param name="Target">Karakter yang menjadi target skill.</param>
        public abstract void UniqueSkill(BaseCharacter Target); 

        /// <summary>
        /// Menerima damage dan menguraikan poin kesehatan karakter.
        /// </summary>
        /// <param name="damage">Jumlah damage yang diterima.</param>
        /// <returns>Mengembalikan status apakah karakter masih hidup setelah menerima damage.</returns>
        public virtual bool TakeDamage(int damage)
        {
            if (IsAlive)
            {
                HealthPoint -= damage;
            }

            return IsAlive;
        }

        /// <summary>
        /// Memulihkan kesehatan karakter berdasarkan jumlah tertentu.
        /// </summary>
        /// <param name="amount">Jumlah pemulihan HP.</param>
        /// <returns>Mengembalikan status apakah karakter masih hidup.</returns>
        public virtual bool Heal(int amount)
        {
            if (IsAlive)
            {
                HealthPoint += amount;
            }

            return IsAlive;
        }
    }
}