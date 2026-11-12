using System;

namespace ConsoleApp129.Models
{
    internal class Person : MapObject
    {
        int pointX;
        int pointY;

        public Person(int X, int Y)
        {
            pointX = X; pointY = Y;
        }
        public override char Rendering_on_the_map()
        {
            return '☺';
        }
    }

    internal class Hero : Person
    {
        public Hero(int X, int Y) : base(X, Y) { }

        public int Health { get; set; } = 3; // По умолчанию 3 жизни
        public static ConsoleColor HeroColor { get; set; } = ConsoleColor.Yellow;

        // Память о том, куда герой смотрит (по умолчанию вправо)
        public int LastDx { get; set; } = 1;
        public int LastDy { get; set; } = 0;

        public override char Rendering_on_the_map()
        {
            Console.ForegroundColor = HeroColor;
            return '☻';
        }
    }

    internal class Enemy : Person
    {
        public Enemy(int X, int Y) : base(X, Y) { }

        public override char Rendering_on_the_map()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return base.Rendering_on_the_map();
        }
    }
}