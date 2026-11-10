using System;
namespace ConsoleApp129.UI
{
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
            Console.WriteLine("2. Загрузить игру");
            Console.WriteLine("3. Выход");
            Console.WriteLine("4. Поменять скин");
            Console.Write("\nВыберите действие: ");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
                if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
                if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) return 3;
                if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad3) return 4;
                if (key == ConsoleKey.D5 || key == ConsoleKey.NumPad5) return 5;


            }
        }
        public static ConsoleColor showColorChoice()
        {
            Console.Clear();
            Console.WriteLine("Выберите цвет героя");
            Console.WriteLine("1. Желтый (по умолчанию)");
            Console.WriteLine("2. Зеленый");
            Console.WriteLine("3. Голубой");
            Console.WriteLine("4. Пурпурный");
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return ConsoleColor.Yellow;
                if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return ConsoleColor.Green;
                if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) return ConsoleColor.Cyan;
                if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4) return ConsoleColor.Magenta;
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
            Console.WriteLine("2. Выйти в главное меню");
            Console.WriteLine("3. Загрузиться");
            Console.WriteLine("4. Сохраниться");


            Console.Write("\nВыберите действие: ");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
                if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
                if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) return 3;
                if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4) return 4;
                if (key == ConsoleKey.D5 || key == ConsoleKey.NumPad4) return 5;



            }
        }
        public static int ShowDeathMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===================");
            Console.WriteLine("    ВЫ ПОГИБЛИ!    ");
            Console.WriteLine("===================");
            Console.ResetColor();
            Console.WriteLine("\n1. Загрузить последнее сохранение");
            Console.WriteLine("2. Выйти в главное меню");
            Console.Write("\nВыберите действие: ");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
                if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
            }
        }
    }
    }
