using System;

namespace RpgEngine.Tests;

// Dummy class khusus pengujian BaseCharacter
public class DummyCharacter : BaseCharacter
{
    public DummyCharacter(string name, int maxHealth, int attackPower) 
        : base(name, maxHealth, attackPower) { }

    public override void UniqueSkill(BaseCharacter target)
    {
        Console.WriteLine($"[SKILL] {Name} menggunakan skill ke {target.Name}.");
    }
}

public static class BaseCharacterTests
{
    public static void Run()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("    TEST SUITE: BASE CHARACTER             ");
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

        // 1. GUARD CLAUSES TEST
        Console.WriteLine("--- 1. TESTING GUARD CLAUSES ---");
        bool threwNameError = false;
        try { var c = new DummyCharacter("", 100, 10); }
        catch (ArgumentException) { threwNameError = true; }
        Assert(threwNameError, "Constructor: Reject empty/null name");

        bool threwHpError = false;
        try { var c = new DummyCharacter("Hero", 0, 10); }
        catch (ArgumentOutOfRangeException) { threwHpError = true; }
        Assert(threwHpError, "Constructor: Reject maxHealth <= 0");

        bool threwAtkError = false;
        try { var c = new DummyCharacter("Hero", 100, -5); }
        catch (ArgumentOutOfRangeException) { threwAtkError = true; }
        Assert(threwAtkError, "Constructor: Reject attackPower < 0");

        // 2. INITIAL STATE TEST
        Console.WriteLine("\n--- 2. TESTING INITIAL STATE ---");
        var hero = new DummyCharacter("Arthur", 100, 20);
        Assert(hero.Name == "Arthur", "Property Name terisi benar");
        Assert(hero.MaxHealth == 100, "Property MaxHealth terisi benar");
        Assert(hero.HealthPoint == 100, "HealthPoint awal = MaxHealth");
        Assert(hero.AttackPower == 20, "Property AttackPower terisi benar");
        Assert(hero.IsAlive == true, "IsAlive bernilai true saat HP > 0");

        // 3. NORMAL TAKEDAMAGE & HEAL TEST
        Console.WriteLine("\n--- 3. TESTING TAKEDAMAGE & HEAL ---");
        hero.TakeDamage(30);
        Assert(hero.HealthPoint == 70, "TakeDamage(30): HP dari 100 berkurang jadi 70");
        hero.Heal(20);
        Assert(hero.HealthPoint == 90, "Heal(20): HP dari 70 bertambah jadi 90");

        // 4. BOUNDARY & CLAMPING TEST
        Console.WriteLine("\n--- 4. TESTING BOUNDARY & CLAMPING ---");
        hero.Heal(500);
        Assert(hero.HealthPoint == 100, "Overheal: Heal(500) mentok di MaxHealth (100)");
        hero.TakeDamage(500);
        Assert(hero.HealthPoint == 0, "Overkill: TakeDamage(500) mentok di 0");
        Assert(hero.IsAlive == false, "Overkill: IsAlive otomatis bernilai false");

        // 5. EDGE CASES TEST
        Console.WriteLine("\n--- 5. TESTING EDGE CASES (NEGATIVE & ZERO) ---");
        var dummy = new DummyCharacter("Target", 100, 10);
        dummy.TakeDamage(-50);
        Assert(dummy.HealthPoint == 100, "TakeDamage(-50): HP tidak boleh bertambah");
        dummy.TakeDamage(0);
        Assert(dummy.HealthPoint == 100, "TakeDamage(0): HP tidak berubah");
        dummy.TakeDamage(40);
        dummy.Heal(-30);
        Assert(dummy.HealthPoint == 60, "Heal(-30): HP tidak boleh berkurang");
        dummy.Heal(0);
        Assert(dummy.HealthPoint == 60, "Heal(0): HP tidak berubah");

        // 6. POST-MORTEM ACTIONS TEST
        Console.WriteLine("\n--- 6. TESTING POST-MORTEM ACTIONS ---");
        var corpse = new DummyCharacter("Zombie", 50, 5);
        corpse.TakeDamage(50);
        corpse.Heal(30);
        Assert(corpse.HealthPoint == 0 && !corpse.IsAlive, "Post-Mortem: Karakter mati TIDAK BISA di-heal");
        corpse.TakeDamage(20);
        Assert(corpse.HealthPoint == 0, "Post-Mortem: Karakter mati tetap di HP 0 saat diserang");

        // SUMMARY
        Console.WriteLine("\n===========================================");
        Console.WriteLine($"RESULT: {passedTests}/{totalTests} TESTS PASSED");
        Console.WriteLine("===========================================\n");
    }
}