using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgEngine
{
    /// <summary>
    /// Pengelola (*manager*) yang bertanggung jawab menyimpan, menjalankan, dan membersihkan seluruh efek status yang sedang menempel pada suatu karakter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kelas ini menerapkan prinsip **Komposisi (konsep *HAS-A*)**, di mana setiap entitas karakter (<see cref="BaseCharacter"/>) 
    /// memiliki satu instansi <see cref="StatusManager"/> untuk mengatur perputaran giliran (*turn cycle*) dari efek-efek aktif seperti Bleed, Regen, atau Stun.
    /// </para>
    /// </remarks>
    public class StatusManager
    {
        /// <summary>
        /// Daftar internal yang menyimpan seluruh objek efek status (<see cref="StatusEffect"/>) yang sedang aktif pada karakter.
        /// </summary>
        private readonly List<StatusEffect> _effects = new List<StatusEffect>();

        /// <summary>
        /// Menandakan apakah karakter sedang berada dalam kondisi terparalisis atau kehilangan giliran akibat efek Stun.
        /// </summary>
        /// <value>
        /// Bernilai <c>true</c> jika setidaknya ada satu efek bertipe <see cref="StunEffect"/> di dalam daftar aktif; sebaliknya <c>false</c>.
        /// </value>
        public bool IsStunned => _effects.Any(e => e is StunEffect);

        /// <summary>
        /// Menambahkan efek status baru ke dalam daftar efek aktif karakter.
        /// </summary>
        /// <param name="effect">Instansi efek status (<see cref="StatusEffect"/>) yang akan ditambahkan.</param>
        /// <exception cref="ArgumentNullException">
        /// Dilempar jika parameter <paramref name="effect"/> bernilai <c>null</c>.
        /// </exception>
        public void AddEffect(StatusEffect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect), "Efek tidak boleh null.");
            }

            _effects.Add(effect);
        }

        /// <summary>
        /// Memperbarui seluruh efek status yang aktif pada awal atau pergantian giliran (*turn*) karakter.
        /// </summary>
        /// <param name="target">Objek karakter (<see cref="BaseCharacter"/>) pemilik efek yang akan menerima dampak status.</param>
        /// <exception cref="ArgumentNullException">
        /// Dilempar jika parameter <paramref name="target"/> bernilai <c>null</c>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Metode ini menjalankan dua tahap eksekusi otomatis:
        /// <list type="bullet">
        /// <item><description><b>Tahap 1:</b> Menjalankan dampak efek via <see cref="StatusEffect.Apply(BaseCharacter)"/> dan mengurangi sisa durasinya via <see cref="StatusEffect.TickDuration"/>.</description></item>
        /// <item><description><b>Tahap 2:</b> Membersihkan (*garbage collect*) seluruh efek yang durasinya telah habis (<see cref="StatusEffect.IsOnUse"/> bernilai <c>false</c>).</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public void UpdateTurn(BaseCharacter target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target), "Target tidak boleh null.");
            }

            // Step A: Jalankan efek & kurangi durasi tiap status di dalam list
            foreach (var effect in _effects)
            {
                effect.Apply(target);
                effect.TickDuration();
            }

            // Step B: Bersihkan/hapus status yang sudah mati (!IsOnUse)
            _effects.RemoveAll(effect => !effect.IsOnUse);
        }
    }
}