using System;

namespace RpgEngine
{
    /// <summary>
    /// Mengelola efek status kelumpuhan (<c>Stun</c>) yang membatasi kemampuan bertindak karakter (*Crowd Control*) dengan menyebabkan karakter kehilangan giliran aksi (*turn*).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kelas ini merupakan spesialisasi dari <see cref="StatusEffect"/> yang menerapkan prinsip *Inheritance / IS-A*.
    /// </para>
    /// <para>
    /// Berbeda dengan efek modifikasi poin kesehatan seperti <c>BleedEffect</c> atau <c>RegenEffect</c>, <c>StunEffect</c> berfokus pada pengendalian status kondisi karakter. 
    /// Keberadaan objek efek ini di dalam <see cref="StatusManager"/> secara otomatis mengaktifkan kondisi <see cref="StatusManager.IsStunned"/> menjadi <c>true</c> 
    /// guna menghentikan fase eksekusi perintah karakter pada sistem pertarungan.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var stun = new StunEffect(monsterTarget, duration: 1, maxDuration: 2);
    /// character.StatusManager.AddEffect(stun);
    /// </code>
    /// </example>
    public class StunEffect : StatusEffect
    {
        /// <summary>
        /// Membentuk instansi baru dari efek kelumpuhan (<see cref="StunEffect"/>) dengan menetapkan karakter target serta durasi giliran yang ditentukan.
        /// </summary>
        /// <param name="character">Subjek karakter (<see cref="BaseCharacter"/>) yang menjadi target penerima efek Stun.</param>
        /// <param name="duration">Sisa durasi aktif efek kelumpuhan dalam hitungan giliran (*turn*).</param>
        /// <param name="maxDuration">Batas maksimum akumulasi durasi giliran yang diizinkan untuk efek Stun ini.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Dilempar melalui konstruktor kelas induk <see cref="StatusEffect"/> apabila nilai <paramref name="duration"/> atau <paramref name="maxDuration"/> bernilai negatif.
        /// </exception>
        public StunEffect(BaseCharacter character, int duration, int maxDuration) : base(character, "Stun", duration, maxDuration) { }

        /// <summary>
        /// Menerapkan dampak kelumpuhan terhadap karakter target pada saat pergantian giliran.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) yang sedang dalam kondisi terparalisis/Stun.</param>
        /// <remarks>
        /// <para>
        /// Implementasi metode ini sengaja dibiarkan kosong (*no-op*) karena efek Stun tidak merubah nilai numerik Atribut (seperti HP) secara bertahap di setiap giliran.
        /// </para>
        /// <para>
        /// Pemblokiran fase aksi dilakukan secara pasif melalui pengecekan status <see cref="StatusManager.IsStunned"/> pada mesin alur pertarungan (*BattleEngine*).
        /// </para>
        /// </remarks>
        public override void Apply(BaseCharacter target) { }
    }
}