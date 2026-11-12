using ConsoleApp129.Data;
using ConsoleApp129.Models;
using System;

namespace ConsoleApp129.Logic
{
    internal class Map
    {
        Random rand = new Random();
        MapObject[,] map = new MapObject[25, 25];
        public bool LevelCompleted { get; set; }

        public MapObject[,] GetMap() { return map; }

        public void RestoreFromState(GameState state)
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i, j] = new Field();

            foreach (var wall in state.Walls) map[wall.X, wall.Y] = new Wall();
            foreach (var tree in state.Trees) map[tree.X, tree.Y] = new Tree();
            foreach (var enemy in state.Enemies) map[enemy.X, enemy.Y] = new Enemy(enemy.X, enemy.Y);

            Hero hero = new Hero(state.HeroX, state.HeroY);
            hero.Health = 3; // Восстанавливаем 3 сердца при загрузке
            map[state.HeroX, state.HeroY] = hero;
        }

        public void Map_generation()
        {
            LevelCompleted = false;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    int A = rand.Next(100);
                    map[i, j] = new Field();

                    if (A > 1 && A < 6) map[i, j] = new Wall();
                    if (A < 2) map[i, j] = new Enemy(i, j); // Чуть больше врагов
                    if (A > 5 && A < 10) map[i, j] = new Tree();

                    if (i == map.GetLength(0) / 2 && j == map.GetLength(1) / 2)
                    {
                        map[i, j] = new Hero(i, j);
                    }
                }
            }
        }

        public void Drawing_the_map()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j].Rendering_on_the_map() + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public void MovePersons() // Двигает врагов раз в секунду
        {
            MapObject[,] newMap = new MapObject[map.GetLength(0), map.GetLength(1)];
            Array.Copy(map, newMap, map.Length);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] is Enemy)
                    {
                        int direction = rand.Next(4);
                        int newX = i, newY = j;

                        switch (direction)
                        {
                            case 0: newX = (i - 1 + map.GetLength(0)) % map.GetLength(0); break;
                            case 1: newX = (i + 1) % map.GetLength(0); break;
                            case 2: newY = (j - 1 + map.GetLength(1)) % map.GetLength(1); break;
                            case 3: newY = (j + 1) % map.GetLength(1); break;
                        }

                        // Если враг наступает на героя - снимаем здоровье, враг умирает
                        if (newMap[newX, newY] is Hero h)
                        {
                            h.Health--;
                            newMap[i, j] = new Field(); // Враг исчезает после укуса
                        }
                        else if (newMap[newX, newY] is Field)
                        {
                            newMap[newX, newY] = map[i, j];
                            newMap[i, j] = new Field();
                        }
                    }
                }
            }
            Array.Copy(newMap, map, map.Length);
        }

        public void MovePersons(ConsoleKey key)
        {
            MapObject[,] newMap = new MapObject[map.GetLength(0), map.GetLength(1)];
            Array.Copy(map, newMap, map.Length);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] is Hero hero)
                    {
                        int newX = i, newY = j;

                        // Запоминаем направление взгляда
                        switch (key)
                        {
                            case ConsoleKey.UpArrow: newX = (i - 1 + map.GetLength(0)) % map.GetLength(0); hero.LastDx = -1; hero.LastDy = 0; break;
                            case ConsoleKey.DownArrow: newX = (i + 1) % map.GetLength(0); hero.LastDx = 1; hero.LastDy = 0; break;
                            case ConsoleKey.LeftArrow: newY = (j - 1 + map.GetLength(1)) % map.GetLength(1); hero.LastDx = 0; hero.LastDy = -1; break;
                            case ConsoleKey.RightArrow: newY = (j + 1) % map.GetLength(1); hero.LastDx = 0; hero.LastDy = 1; break;
                            default: return;
                        }

                        // Если герой сам врезается во врага - получает урон, враг умирает
                        if (newMap[newX, newY] is Enemy)
                        {
                            hero.Health--;
                            newMap[newX, newY] = new Field();
                        }
                        else if (newMap[newX, newY] is Field || newMap[newX, newY] is Ship)
                        {
                            newMap[newX, newY] = map[i, j];
                            newMap[i, j] = new Field();
                        }
                    }
                }
            }
            Array.Copy(newMap, map, map.Length);
        }

        public bool IsHeroAlive()
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] is Hero hero)
                        return hero.Health > 0;

            return false;
        }

        // --- СТРЕЛЬБА ГЕРОЯ (Мгновенная, как лазер) ---
        public void Shoot()
        {
            int heroX = -1, heroY = -1;
            Hero currentHero = null;

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] is Hero h) { heroX = i; heroY = j; currentHero = h; }

            if (currentHero == null) return;

            int dx = currentHero.LastDx;
            int dy = currentHero.LastDy;

            // Мгновенно летим до первого врага или стены
            for (int step = 1; step < 25; step++)
            {
                int targetX = heroX + dx * step;
                int targetY = heroY + dy * step;

                if (targetX < 0 || targetX >= map.GetLength(0) || targetY < 0 || targetY >= map.GetLength(1)) break;
                if (map[targetX, targetY] is Wall || map[targetX, targetY] is Tree) break;

                if (map[targetX, targetY] is Enemy)
                {
                    map[targetX, targetY] = new Field(); // Враг убит
                    break;
                }
            }
        }

        // --- СТРЕЛЬБА ВРАГОВ (Создает видимую медленную пулю) ---
        public void EnemyShoot()
        {
            int heroX = -1, heroY = -1;
            // Ищем героя
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] is Hero) { heroX = i; heroY = j; }

            if (heroX == -1) return;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] is Enemy)
                    {
                        // ШАНС 30% (если выпадет 0, 1 или 2 из 10)
                        if (rand.Next(10) < 0.3)
                        {
                            int dx = 0, dy = 0;

                            // Определяем направление на героя по прямой
                            if (Math.Abs(heroX - i) > Math.Abs(heroY - j)) dx = heroX > i ? 1 : -1;
                            else dy = heroY > j ? 1 : -1;

                            int nextX = i + dx;
                            int nextY = j + dy;

                            // Если перед врагом пустое поле, создаем пулю
                            if (nextX >= 0 && nextX < map.GetLength(0) && nextY >= 0 && nextY < map.GetLength(1))
                            {
                                if (map[nextX, nextY] is Field)
                                {
                                    map[nextX, nextY] = new Bullet(dx, dy, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        // --- ДВИЖЕНИЕ ВИДИМЫХ ПУЛЬ (Вражеских) ---
        public void MoveBullets()
        {
            MapObject[,] newMap = new MapObject[map.GetLength(0), map.GetLength(1)];
            Array.Copy(map, newMap, map.Length);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] is Bullet bullet && bullet.IsEnemy) // Двигаем только вражеские
                    {
                        int nextX = i + bullet.Dx;
                        int nextY = j + bullet.Dy;

                        if (nextX < 0 || nextX >= map.GetLength(0) || nextY < 0 || nextY >= map.GetLength(1))
                        {
                            newMap[i, j] = new Field(); // Улетела
                        }
                        else if (map[nextX, nextY] is Wall || map[nextX, nextY] is Tree)
                        {
                            newMap[i, j] = new Field(); // Врезалась в стену
                        }
                        else if (map[nextX, nextY] is Hero hero)
                        {
                            hero.Health--; // Попала в героя
                            newMap[i, j] = new Field();
                        }
                        else if (map[nextX, nextY] is Field)
                        {
                            newMap[i, j] = new Field();
                            newMap[nextX, nextY] = bullet; // Летит дальше
                        }
                    }
                }
            }
            Array.Copy(newMap, map, map.Length);
        }

        public void CheckLevelEnd()
        {
            int enemiesCount = 0;
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] is Enemy) enemiesCount++;

            if (enemiesCount == 0 && !LevelCompleted)
            {
                map[map.GetLength(0) / 2, map.GetLength(1) / 2] = new Ship();
                LevelCompleted = true;
            }
        }

        public bool IsHeroOnShip()
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] is Hero)
                    {
                        if (i > 0 && map[i - 1, j] is Ship) return true;
                        if (i < 24 && map[i + 1, j] is Ship) return true;
                        if (j > 0 && map[i, j - 1] is Ship) return true;
                        if (j < 24 && map[i, j + 1] is Ship) return true;
                    }
            return false;
        }
    }
}