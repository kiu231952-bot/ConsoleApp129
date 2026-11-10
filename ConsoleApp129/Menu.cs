using System;

public static class Menu
{
    // Главное меню
    public static int ShowMainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("======================");
        Console.WriteLine("       ДОБРО ПОЖАЛОВАТЬ         ");
        Console.WriteLine("=====================");
        Console.ResetColor();
        Console.WriteLine("\n1. Начать игру");
        Console.WriteLine("2. Выйти");
        Console.Write("\nВыберите действие: ");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
            if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
        }
    }

    // Меню паузы
    public static int ShowPauseMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=================================");
        Console.WriteLine("            ПАУЗА                ");
        Console.WriteLine("=================================");
        Console.ResetColor();
        Console.WriteLine("\n1. Продолжить");
        Console.WriteLine("2. Начать заново");
        Console.WriteLine("3. Выйти в главное меню");
        Console.Write("\nВыберите действие: ");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
            if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
            if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) return 3;
        }
    }
}
