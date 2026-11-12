using ConsoleApp129.Logic;
using ConsoleApp129.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp129.Data
{
    public class GameState
    {
        public int HeroX { get; set; }
        public int HeroY { get; set; }
        public List<PointData> Enemies { get; set; } = new List<PointData>();
        public List<PointData> Walls { get; set; } = new List<PointData>();
        public List<PointData> Trees { get; set; } = new List<PointData>();
    }

    public class PointData
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

     static class SaveManager
    {
        private const string SaveFilePath = "savegame.json";

        public static void SaveGame(Map gameMap)
        {
            try
            {
                var state = new GameState();
                MapObject[,] map = gameMap.GetMap();

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[i, j] is Hero) { state.HeroX = i; state.HeroY = j; }
                        else if (map[i, j] is Enemy) state.Enemies.Add(new PointData { X = i, Y = j });
                        else if (map[i, j] is Wall) state.Walls.Add(new PointData { X = i, Y = j });
                        else if (map[i, j] is Tree) state.Trees.Add(new PointData { X = i, Y = j });
                    }
                }

                string json = JsonConvert.SerializeObject(state, Formatting.Indented);

                // Сохраняем прямо в папку, где лежит игра (bin\Debug)
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА]: {ex.Message}");
            }
        }

        public static GameState LoadGame()
        {
            if (!File.Exists(SaveFilePath)) return null;
            string json = File.ReadAllText(SaveFilePath);
            return JsonConvert.DeserializeObject<GameState>(json);
        }
    }
}