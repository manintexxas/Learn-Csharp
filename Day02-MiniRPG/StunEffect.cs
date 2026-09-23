using System;

namespace RpgEngine
{
    /// <summary>
    /// Merepresentasikan efek status kelumpuhan (<c>Stun</c>) yang menyebabkan karakter kehilangan giliran beraksi.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kelas ini merupakan turunan dari <see cref="StatusEffect"/> (menerapkan prinsip *Inheritance / IS-A*).
    /// </para>
    /// <para>
    /// Berbeda dengan <c>BleedEffect</c> atau <c>RegenEffect</c> yang mengubah nilai HP, <c>StunEffect</c> berfokus pada kontrol kondisi (*Crowd Control*).
    /// Keberadaan kelas ini di dalam <see cref="StatusManager"/> akan memicu nilai <see cref="StatusManager.IsStunned"/> menjadi <c>true</c>.
    /// </para>
    /// </remarks>
    public class StunEffect : StatusEffect
    {
        /// <summary>
        /// Menginisialisasi efek status kelumpuhan (<c>Stun</c>) baru dengan durasi giliran tertentu.
        /// </summary>
        /// <param name="duration">Jumlah giliran (*turn*) aktif di mana karakter akan terparalisis/kehilangan giliran.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar melalui kelas induk <see cref="StatusEffect"/> jika nilai <paramref name="duration"/> bernilai negatif.
        /// </exception>
        public StunEffect(int duration) : base("Stun", duration) { }

        /// <summary>
        /// Menjalankan pemrosesan khusus saat giliran karakter diperbarui.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang sedang terkena efek Stun.</param>
        /// <remarks>
        /// <para>
        /// Metode ini sengaja dibiarkan kosong karena dampak dari efek Stun tidak mengubah poin kesehatan (HP) secara langsung di tiap giliran.
        /// </para>
        /// <para>
        /// Pengecekan status lumpuh dilakukan secara terpusat oleh <see cref="StatusManager.IsStunned"/> saat sistem pertarungan (<c>BattleEngine</c>) mengecek apakah karakter boleh mengambil tindakan atau tidak.
        /// </para>
        /// </remarks>
        public override void Apply(BaseCharacter target) { }
    }
}