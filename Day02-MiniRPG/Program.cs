using RpgEngine.Test;
using RpgEngine.Tests;

namespace RpgEngine;

class Program
{
    static void Main(string[] args)
    {
        // Panggil pengujian BaseCharacter
        EffectTest.Run();

        // Nanti kalau Hero & Monster sudah siap, tinggal panggil di sini:
        // HeroTests.Run();
        // MonsterTests.Run();
    }
}