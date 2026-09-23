using System;

namespace RpgEngine
{
    /// <summary>
    /// Kontrak (*interface*) dasar untuk semua objek yang dapat digunakan pada atau oleh sebuah karakter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Interface ini menerapkan konsep **Abstraksi** dan **Polimorfisme** untuk mempermudah penanganan item atau efek. 
    /// Segala objek yang mengimplementasikan <see cref="IUseable"/> (seperti Ramuan/Potion, Gulungan Sihir/Scroll, hingga Efek Status) 
    /// dapat dipicu aksi utamanya melalui metode <see cref="Use(BaseCharacter)"/> tanpa perlu tahu detail spesifik objek tersebut.
    /// </para>
    /// </remarks>
    public interface IUseable
    {
        /// <summary>
        /// Nama identitas dari objek atau efek yang dapat digunakan (misal: "Health Potion", "Antidote", "Bleed").
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Mengaplikasikan atau menggunakan fungsi utama dari objek ini terhadap target karakter tertentu.
        /// </summary>
        /// <param name="target">
        /// Karakter sasaran (<see cref="BaseCharacter"/>) yang akan menerima dampak/efek dari penggunaan objek ini.
        /// </param>
        /// <returns>
        /// Bernilai <c>true</c> jika objek atau efek berhasil diterapkan pada target; 
        /// bernilai <c>false</c> jika penggunaan gagal (misal: syarat kondisi target tidak terpenuhi).
        /// </returns>
        public bool Use(BaseCharacter target);
    }
}