using System;
using System.Threading;
using ConsoleApp129.Models;
using ConsoleApp129.Logic;
using ConsoleApp129.UI;
using ConsoleApp129.Data;

namespace ConsoleApp129
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            while (true)
            {
                int mainChoice = Menu.ShowMainMenu();
                if (mainChoice == 3) break;
                if (mainChoice == 4)
                {
                    Hero.HeroColor = Menu.showColorChoice();
                    continue;
                }

                Map map = new Map();
                if (mainChoice == 1) map.Map_generation();
                else if (mainChoice == 2)
                {
                    var loadedState = SaveManager.LoadGame();
                    if (loadedState == null)
                    {
                        Console.WriteLine("\nСохранение не найдено!");
                        Console.ReadKey(true);
                        continue;
                    }
                    map.RestoreFromState(loadedState);
                }

                int level = 1;
                bool isPlaying = true;
                DateTime lastEnemyMove = DateTime.Now;
                DateTime lastBulletMove = DateTime.Now;

                Console.Clear();

                while (isPlaying)
                {
                    // Отрисовка без мерцания
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"=== УРОВЕНЬ {level} ===");

                    Hero currentHero = null;
                    for (int i = 0; i < map.GetMap().GetLength(0); i++)
                        for (int j = 0; j < map.GetMap().GetLength(1); j++)
                            if (map.GetMap()[i, j] is Hero h) currentHero = h;

                    if (currentHero != null)
                        Console.WriteLine($"Здоровье: {new string('♥', currentHero.Health)}    ");

                    map.Drawing_the_map();
                    Console.WriteLine("\nСтрелки - ход | [Пробел] - стрельба | [ESC] - пауза            ");

                    if ((DateTime.Now - lastEnemyMove).TotalSeconds >= 1)
                    {
                        map.MovePersons();      // Враги ходят
                        map.EnemyShoot();       // Враги создают пули
                        lastEnemyMove = DateTime.Now;
                    }

                    // --- ДВИЖЕНИЕ ПУЛЬ (раз в 0.3 секунды) ---
                    if ((DateTime.Now - lastBulletMove).TotalMilliseconds >= 100)
                    {
                        map.MoveBullets();      // Пули летят
                        lastBulletMove = DateTime.Now;
                    }
                    // Проверка на смерть
                    if (!map.IsHeroAlive())
                    {
                        int deathChoice = Menu.ShowDeathMenu();
                        if (deathChoice == 1)
                        {
                            var loadedState = SaveManager.LoadGame();
                            if (loadedState != null) map.RestoreFromState(loadedState);
                            else map.Map_generation();
                            level = 1;
                        }
                        else isPlaying = false;

                        Console.Clear();
                        break;
                    }

                    // Проверка на корабль (переход на новый уровень)
                    map.CheckLevelEnd();
                    if (map.IsHeroOnShip())
                    {
                        level++;
                        map.Map_generation();
                        Console.Clear();
                        break;
                    }

                    // Обработка нажатий
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;

                        if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow ||
                            key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                        {
                            map.MovePersons(key);
                        }
                        else if (key == ConsoleKey.Spacebar)
                        {
                            map.Shoot();
                        }
                        else if (key == ConsoleKey.Escape)
                        {
                            Console.Clear();
                            int pauseChoice = Menu.ShowPauseMenu();
                            if (pauseChoice == 2) { isPlaying = false; break; }
                            if (pauseChoice == 3) map.RestoreFromState(SaveManager.LoadGame());
                            if (pauseChoice == 4) SaveManager.SaveGame(map);
                            Console.Clear();
                        }
                        else if (key == ConsoleKey.S) SaveManager.SaveGame(map);
                        else if (key == ConsoleKey.L)
                        {
                            var st = SaveManager.LoadGame();
                            if (st != null) map.RestoreFromState(st);
                        }
                    }

                    Thread.Sleep(10); // Задержка, чтобы не грузить процессор
                }
            }
        }
    }
}