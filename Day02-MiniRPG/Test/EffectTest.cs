using System;

namespace RpgEngine.Test;

// Dummy class khusus pengujian BaseCharacter & Status Effects
public class DummyCharacter : BaseCharacter
{
    public DummyCharacter(string name, int maxHealth, int attackPower) 
        : base(name, maxHealth, attackPower) { }

    public override void UniqueSkill(BaseCharacter target)
    {
        Console.WriteLine($"[SKILL] {Name} menggunakan skill ke {target.Name}.");
    }
}

public static class EffectTest
{
    public static void Run()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("     TEST SUITE: STATUS EFFECTS & MANAGER  ");
        Console.WriteLine("===========================================\n");

        int passedTests = 0;
        int totalTests = 0;

        void Assert(bool condition, string testName, string detail = "")
        {
            totalTests++;
            if (condition)
            {
                passedTests++;
                Console.WriteLine($"[PASS] {testName}");
            }
            else
            {
                Console.WriteLine($"[FAIL] {testName} -> {detail}");
            }
        }

        // 1. GUARD CLAUSES & NULL CHECKS
        Console.WriteLine("--- 1. TESTING GUARD CLAUSES & NULL CHECKS ---");
        var hero = new DummyCharacter("Arthur", 100, 20);

        bool threwNullEffect = false;
        try { hero.ApplyStatus(null!); }
        catch (ArgumentNullException) { threwNullEffect = true; }
        Assert(threwNullEffect, "ApplyStatus: Reject null effect");

        bool threwInvalidDuration = false;
        try { var invalidRegen = new RegenEffect(-1, 10); }
        catch (ArgumentOutOfRangeException) { threwInvalidDuration = true; }
        Assert(threwInvalidDuration, "Constructor RegenEffect: Reject duration <= 0");

        // 2. REGEN EFFECT TEST
        Console.WriteLine("\n--- 2. TESTING REGEN EFFECT ---");
        var regenHero = new DummyCharacter("Arthur", 100, 20);
        regenHero.TakeDamage(40); // HP awal = 60

        regenHero.ApplyStatus(new RegenEffect(duration: 2, healthAmount: 10));
        regenHero.OnTurnUpdate(); // Turn 1
        Assert(regenHero.HealthPoint == 70, "Regen Turn 1: HP bertambah dari 60 ke 70");

        regenHero.OnTurnUpdate(); // Turn 2
        Assert(regenHero.HealthPoint == 80, "Regen Turn 2: HP bertambah dari 70 ke 80");

        regenHero.OnTurnUpdate(); // Turn 3 (Efek habis)
        Assert(regenHero.HealthPoint == 80, "Regen Expired: HP tidak bertambah lagi di Turn 3");

        // 3. BLEED EFFECT TEST
        Console.WriteLine("\n--- 3. TESTING BLEED EFFECT ---");
        var bleedHero = new DummyCharacter("Arthur", 100, 20);

        bleedHero.ApplyStatus(new BleedEffect(duration: 2, healthAmount: 15));
        bleedHero.OnTurnUpdate(); // Turn 1
        Assert(bleedHero.HealthPoint == 85, "Bleed Turn 1: HP berkurang dari 100 ke 85");

        bleedHero.OnTurnUpdate(); // Turn 2
        Assert(bleedHero.HealthPoint == 70, "Bleed Turn 2: HP berkurang dari 85 ke 70");

        bleedHero.OnTurnUpdate(); // Turn 3 (Efek habis)
        Assert(bleedHero.HealthPoint == 70, "Bleed Expired: HP tidak berkurang lagi di Turn 3");

        // 4. STUN EFFECT TEST
        Console.WriteLine("\n--- 4. TESTING STUN EFFECT ---");
        var stunHero = new DummyCharacter("Arthur", 100, 20);

        stunHero.ApplyStatus(new StunEffect(duration: 1));
        Assert(stunHero.StatusManager.IsStunned == true, "IsStunned bernilai true saat Stun dipasang");

        stunHero.OnTurnUpdate(); // Turn 1
        Assert(stunHero.HealthPoint == 100, "Stun Effect: Tidak mengubah HP karakter");
        Assert(stunHero.StatusManager.IsStunned == false, "IsStunned bernilai false setelah durasi 1 turn habis");

        // 5. COMBINED EFFECTS TEST
        Console.WriteLine("\n--- 5. TESTING COMBINED EFFECTS ---");
        var comboHero = new DummyCharacter("Arthur", 100, 20);
        comboHero.TakeDamage(50); // HP awal = 50

        comboHero.ApplyStatus(new RegenEffect(duration: 3, healthAmount: 10)); // +10 / turn
        comboHero.ApplyStatus(new BleedEffect(duration: 2, healthAmount: 5));   // -5 / turn
        comboHero.ApplyStatus(new StunEffect(duration: 1));                     // Skip turn

        comboHero.OnTurnUpdate(); // Turn 1 (+5 HP -> 55 HP)
        Assert(comboHero.HealthPoint == 55, "Combined Turn 1: Perubahan HP bersih +5 (Regen 10 - Bleed 5)");
        
        comboHero.OnTurnUpdate(); // Turn 2 (+5 HP -> 60 HP)
        Assert(comboHero.HealthPoint == 60, "Combined Turn 2: HP menjadi 60");
        Assert(comboHero.StatusManager.IsStunned == false, "Combined Turn 2: Stun terhapus dari manager");

        comboHero.OnTurnUpdate(); // Turn 3 (Bleed habis, sisa Regen +10 HP -> 70 HP)
        Assert(comboHero.HealthPoint == 70, "Combined Turn 3: Bleed kadaluarsa, sisa Regen (+10 HP)");

        // 6. OVERKILL & OVERHEAL VIA STATUS EFFECTS
        Console.WriteLine("\n--- 6. TESTING OVERKILL & OVERHEAL ---");

        // Overheal Test
        var overhealHero = new DummyCharacter("Arthur", 100, 20);
        overhealHero.TakeDamage(10); // HP = 90
        overhealHero.ApplyStatus(new RegenEffect(duration: 1, healthAmount: 30)); // Regen +30
        overhealHero.OnTurnUpdate();
        Assert(overhealHero.HealthPoint == 100, "Overheal: Regen (+30 HP) pada HP 90 mentok di MaxHealth (100)");

        // Overkill Test
        var overkillHero = new DummyCharacter("Arthur", 100, 20);
        overkillHero.TakeDamage(90); // HP = 10
        overkillHero.ApplyStatus(new BleedEffect(duration: 1, healthAmount: 50)); // Bleed -50
        overkillHero.OnTurnUpdate();
        Assert(overkillHero.HealthPoint == 0, "Overkill: Bleed (-50 HP) pada HP 10 mentok di 0");
        Assert(overkillHero.IsAlive == false, "Overkill: Karakter otomatis terdeteksi mati (IsAlive = false)");

        // SUMMARY
        Console.WriteLine("\n===========================================");
        Console.WriteLine($"RESULT: {passedTests}/{totalTests} TESTS PASSED");
        Console.WriteLine("===========================================\n");
    }
}