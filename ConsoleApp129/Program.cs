using System;
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
                if (mainChoice == 3) break; // Выход

                Map map = new Map();

                if (mainChoice == 1)
                {
                    map.Map_generation();
                }
                else if (mainChoice == 2)
                {
                    var loadedState = SaveManager.LoadGame();
                    if (loadedState == null)
                    {
                        Console.WriteLine("\nСохранение не найдено! Нажмите любую клавишу...");
                        Console.ReadKey(true);
                        continue;
                    }
                    map.RestoreFromState(loadedState);
                }
                else if (mainChoice == 4)
                {
                    Hero.HeroColor = Menu.showColorChoice();
                    continue;
                }
                bool isPlaying = true;
                while (isPlaying)
                {
                    Console.Clear();
                    map.Drawing_the_map();
                    Console.WriteLine("\n[S] - быстрое сохранение | [L] - быстрая загрузка | [ESC] - Пауза");

                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.L) // load ON L
                    {
                        var loadedState = SaveManager.LoadGame();
                        if (loadedState != null)
                        {
                            map.RestoreFromState(loadedState);
                            Console.WriteLine("\n[ИГРА ЗАГРУЖЕНА!]");
                              Console.ReadKey(true);
                            continue;


                        }
                        else
                        {
                            Console.WriteLine("\n[СОХРАНЕНИЕ НЕ НАЙДЕНО!]");
                        }
                        continue;
                    }

                    if (key == ConsoleKey.S)
                    {
                        SaveManager.SaveGame(map);
                        Console.WriteLine("\n[ИГРА СОХРАНЕНА!]");
                        Console.ReadKey(true);
                        continue;
                    }

                    // ПАУЗА
                    if (key == ConsoleKey.Escape)
                    {
                        int pauseChoice = Menu.ShowPauseMenu();
                        if (pauseChoice == 1) continue; // продолжить
                        if (pauseChoice == 2) { isPlaying = false; break; } // main menuu

                        
                        if (pauseChoice == 3)
                        {
                                SaveManager.SaveGame(map);
                                Console.WriteLine("\n[ИГРА СОХРАНЕНА!]");
                                continue;
                        }
                          //ЗАГРУЗКА ИЗ МЕНЮ ПАУЗЫ
                        if (pauseChoice == 4)
                        {
                            var loadedState = SaveManager.LoadGame();
                            if (loadedState != null)
                            {
                                map.RestoreFromState(loadedState);
                                Console.WriteLine("\n[Игра загружена]");
                                Console.ReadKey(true);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("\n[СОХРАНЕНИЕ НЕ НАЙДЕНО!]");
                            }
                            Console.ReadKey(true);
                            continue;
                        }
                    }

                    // ХОДЫ
                    if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow ||
                        key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                    {
                        map.MovePersons(key);
                        map.MovePersons();
                    }

                    // ПРОВЕРКА НА СМЕРТЬ
                    if (!map.IsHeroAlive())
                    {
                        int deathChoice = Menu.ShowDeathMenu();
                        if (deathChoice == 1) // загрузить ласт сохранение
                        {
                            var loadedState = SaveManager.LoadGame();
                            if (loadedState != null)
                            {
                                map.RestoreFromState(loadedState);
                            }
                            else
                            {
                                Console.WriteLine("\nСохранение не найдено! Начинаем заново...");
                                Console.ReadKey(true);
                                map.Map_generation();
                            }
                        }
                        else
                        {
                            isPlaying = false;
                        }
                    }
                }
            }
        }
    }
}