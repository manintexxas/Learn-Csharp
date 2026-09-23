using System;

namespace RpgEngine
{
    /// <summary>
    /// Kelas abstrak dasar yang berfungsi sebagai cetak biru (blueprint) untuk setiap entitas karakter dalam Game RPG.
    /// </summary>
    /// <remarks>
    /// Kelas ini mengelola atribut dasar seperti nama, kesehatan (<see cref="HealthPoint"/>), kekuatan serangan (<see cref="AttackPower"/>),
    /// serta integrasi dengan sistem efek status (<see cref="StatusManager"/>).
    /// </remarks>
    public abstract class BaseCharacter
    {
        private int _healthPoint;

        /// <summary>
        /// Menandakan apakah karakter masih dalam kondisi hidup atau aktif bertarung.
        /// </summary>
        /// <value>
        /// Bernilai <c>true</c> jika <see cref="HealthPoint"/> lebih besar dari 0; sebaliknya <c>false</c>.
        /// </value>
        public bool IsAlive => HealthPoint > 0;

        /// <summary>
        /// Nama identitas unik milik karakter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Batas maksimum poin kesehatan (<see cref="HealthPoint"/>) yang dapat dimiliki karakter.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Nilai dasar kekuatan serangan fisik karakter.
        /// </summary>
        public int AttackPower { get; protected set; }

        /// <summary>
        /// Pengelola kumpulan efek status (seperti pendarahan, stun, atau regenerasi) yang sedang aktif pada karakter.
        /// </summary>
        public StatusManager StatusManager { get; } = new StatusManager();

        /// <summary>
        /// Poin kesehatan (HP) karakter saat ini.
        /// </summary>
        /// <remarks>
        /// Nilai ini dienkapsulasi dan dilindungi menggunakan <see cref="Math.Clamp(int, int, int)"/> 
        /// agar tidak pernah bernilai negatif (kurang dari 0) atau melebihi batas <see cref="MaxHealth"/>.
        /// </remarks>
        public int HealthPoint
        {
            get => _healthPoint;
            protected set => _healthPoint = Math.Clamp(value, 0, MaxHealth);
        }

        /// <summary>
        /// Inisialisasi properti dasar dan aturan validasi saat membuat objek karakter baru.
        /// </summary>
        /// <param name="name">Nama unik karakter (tidak boleh kosong atau hanya berisi spasi).</param>
        /// <param name="maxHealth">Jumlah poin kesehatan maksimum awal (harus bernilai lebih dari 0).</param>
        /// <param name="attackPower">Jumlah poin kekuatan serangan dasar (tidak boleh bernilai negatif/minus).</param>
        /// <exception cref="ArgumentException">Dilempar jika variabel <paramref name="name"/> bernilai <c>null</c>, kosong, atau hanya spasi.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Dilempar jika nilai <paramref name="maxHealth"/> kurang dari atau sama dengan 0, atau jika <paramref name="attackPower"/> kurang dari 0.</exception>
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
        /// Eksekusi kemampuan khusus (*Unique Skill*) karakter terhadap target tertentu.
        /// </summary>
        /// <param name="Target">Objek karakter yang menjadi sasaran efek atau serangan dari kemampuan ini.</param>
        /// <remarks>
        /// Metode ini bersifat <c>abstract</c> dan **wajib** diimplementasikan secara spesifik oleh kelas turunan (misal: <c>Hero</c> atau <c>Monster</c>).
        /// </remarks>
        public abstract void UniqueSkill(BaseCharacter Target); 

        /// <summary>
        /// Mengurangi poin kesehatan karakter berdasarkan nilai damage yang diterima.
        /// </summary>
        /// <param name="damage">Jumlah poin kerusakan yang diberikan kepada karakter.</param>
        /// <returns>
        /// Bernilai <c>true</c> jika karakter masih bertahan hidup setelah menerima kerusakan; sebaliknya <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Pengurangan HP hanya diproses jika karakter dalam kondisi hidup (<see cref="IsAlive"/>). Nilai damage negatif akan otomatis dikonversi menjadi 0.
        /// </remarks>
        public virtual bool TakeDamage(int damage)
        {
            if (IsAlive)
            {
                HealthPoint -= Math.Max(0, damage);
            }

            return IsAlive;
        }

        /// <summary>
        /// Memulihkan poin kesehatan karakter berdasarkan jumlah yang ditentukan.
        /// </summary>
        /// <param name="amount">Jumlah poin pemulihan kesehatan (HP) yang diberikan.</param>
        /// <returns>
        /// Bernilai <c>true</c> jika karakter masih dalam kondisi hidup; sebaliknya <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Pemulihan HP hanya berlaku jika karakter masih hidup. Nilai pemulihan tidak akan membuat <see cref="HealthPoint"/> melebihi <see cref="MaxHealth"/>.
        /// </remarks>
        public virtual bool Heal(int amount)
        {
            if (IsAlive)
            {
                HealthPoint += Math.Max(0, amount);
            }

            return IsAlive;
        }

        /// <summary>
        /// Menambahkan efek status baru (seperti Bleed, Stun, atau Regen) ke dalam pengelola efek karakter.
        /// </summary>
        /// <param name="effect">Instansi dari efek status yang akan diterapkan pada karakter.</param>
        /// <exception cref="ArgumentNullException">Dilempar jika parameter <paramref name="effect"/> bernilai <c>null</c>.</exception>
        public void ApplyStatus(StatusEffect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect), "Efek status tidak boleh null.");
            }

            StatusManager.AddEffect(effect);
        }

        /// <summary>
        /// Memperbarui dan mengeksekusi seluruh efek status yang aktif pada awal giliran (*turn*) karakter.
        /// </summary>
        /// <remarks>
        /// Metode ini biasa dipanggil oleh siklus mesin pertarungan (*Battle Engine*) sebelum karakter melakukan tindakan.
        /// </remarks>
        public void OnTurnUpdate()
        {
            StatusManager.UpdateTurn(this);
        }
    }
}