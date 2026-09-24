using System;
using Xunit;
using RpgEngine;

namespace MiniRPG.Tests
{
    public class StatusEffectTests
    {
        #region Dummy Classes untuk Kebutuhan Pengujian

        // Class buatan untuk mewakili BaseCharacter (karena BaseCharacter bernilai abstract)
        private class DummyCharacter : BaseCharacter
        {
            public DummyCharacter() : base("Hero Dummy", 100, 10) { }

            public override void UniqueSkill(BaseCharacter Target) { }
        }

        // Class buatan untuk mewakili StatusEffect yang abstrak
        private class TestableStatusEffect : StatusEffect
        {
            public bool IsApplyCalled { get; private set; } = false;

            public TestableStatusEffect(BaseCharacter character, string name, int duration, int maxDuration)
                : base(character, name, duration, maxDuration) { }

            public override void Apply(BaseCharacter target)
            {
                IsApplyCalled = true;
            }
        }

        #endregion

        #region 1. Skenario Normal (Input & Penggunaan Standar)

        [Fact]
        public void Constructor_InputValid_BerhasilMembuatEfekStatus()
        {
            // ARRANGE (Persiapan)
            var character = new DummyCharacter();

            // ACT (Eksekusi)
            var effect = new TestableStatusEffect(character, "Poison", duration: 3, maxDuration: 5);

            // ASSERT (Pemeriksaan)
            Assert.Equal("Poison", effect.Name);
            Assert.Equal(3, effect.Duration);
            Assert.Equal(5, effect.MaxDuration);
            Assert.True(effect.IsOnUse); // Karena durasi 3 (> 0)
            Assert.Same(character, effect.Character); // Memastikan karakter yang terhubung sesuai
        }

        [Fact]
        public void Apply_KetikaDipanggil_MenjalankanLogikaEfekStatus()
        {
            // ARRANGE
            var character = new DummyCharacter();
            var effect = new TestableStatusEffect(character, "Poison", duration: 3, maxDuration: 5);

            // ACT
            effect.Apply(character);

            // ASSERT
            // Memastikan method Apply benar-benar terpanggil (Menjawab bagian yang sebelumnya terlewat)
            Assert.True(effect.IsApplyCalled);
        }

        [Fact]
        public void TickDuration_KurangiDurasiSatuPoin()
        {
            // ARRANGE
            var character = new DummyCharacter();
            var effect = new TestableStatusEffect(character, "Poison", duration: 3, maxDuration: 5);

            // ACT
            effect.TickDuration(); // Durasi jadi 2

            // ASSERT
            Assert.Equal(2, effect.Duration);
        }

        [Fact]
        public void RefreshDuration_KembalikanDurasiKeMaxDuration()
        {
            // ARRANGE
            var character = new DummyCharacter();
            var effect = new TestableStatusEffect(character, "Poison", duration: 1, maxDuration: 5);

            // ACT
            effect.RefreshDuration(); // Durasi di-reset dari 1 menjadi 5

            // ASSERT
            Assert.Equal(5, effect.Duration);
        }

        #endregion

        #region 2. Skenario Batas (Boundary Case)

        [Fact]
        public void IsOnUse_KetikaDurasiNol_BernilaiFalse()
        {
            // ARRANGE
            var character = new DummyCharacter();
            var effect = new TestableStatusEffect(character, "Stun", duration: 0, maxDuration: 3);

            // ACT & ASSERT
            Assert.False(effect.IsOnUse); // Efek dianggap habis/selesai
        }

        [Fact]
        public void TickDuration_DurasiNol_TidakBisaMinus()
        {
            // ARRANGE
            var character = new DummyCharacter();
            var effect = new TestableStatusEffect(character, "Stun", duration: 0, maxDuration: 3);

            // ACT
            effect.TickDuration(); // Mencoba mengurangi durasi yang sudah 0

            // ASSERT
            Assert.Equal(0, effect.Duration); // Harus tetap 0, tidak boleh -1 karena di-clamp
        }

        [Fact]
        public void Constructor_DurationMelebihiMaxDuration_TerpotongSesuaiMaxDuration()
        {
            // ARRANGE
            var character = new DummyCharacter();

            // ACT
            // Menginput duration 10 padahal maxDuration cuma 5
            var effect = new TestableStatusEffect(character, "Stun", duration: 10, maxDuration: 5);

            // ASSERT
            // Menjawab skenario batas yang sebelumnya terlewat (Math.Clamp membatasi di angka MaxDuration)
            Assert.Equal(5, effect.Duration);
        }

        #endregion

        #region 3. Skenario Anomali & Error Case

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NamaKosongAtauNull_MelemparArgumentException(string? invalidName)
        {
            // ARRANGE
            var character = new DummyCharacter();

            // ACT & ASSERT
            Assert.Throws<ArgumentException>(() =>
            {
                new TestableStatusEffect(character, invalidName!, duration: 3, maxDuration: 5);
            });
        }

        [Fact]
        public void Constructor_DurasiNegatif_MelemparArgumentOutOfRangeException()
        {
            // ARRANGE
            var character = new DummyCharacter();

            // ACT & ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new TestableStatusEffect(character, "Bleed", duration: -1, maxDuration: 5);
            });
        }

        [Fact]
        public void Constructor_MaxDurasiNegatif_MelemparArgumentOutOfRangeException()
        {
            // ARRANGE
            var character = new DummyCharacter();

            // ACT & ASSERT
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new TestableStatusEffect(character, "Bleed", duration: 2, maxDuration: -1);
            });

            // Memastikan nama parameter yang error adalah maxDuration
            Assert.Equal("maxDuration", exception.ParamName);
        }

        #endregion
    }
}