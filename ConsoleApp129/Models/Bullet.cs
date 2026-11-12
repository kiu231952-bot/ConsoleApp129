using System;

namespace ConsoleApp129.Models
{
    internal class Bullet : MapObject
    {
        public int Dx { get; set; }
        public int Dy { get; set; }
        public bool IsEnemy { get; set; } // Флаг: true - пуля врага, false - героя

        public Bullet(int dx, int dy, bool isEnemy)
        {
            Dx = dx;
            Dy = dy;
            IsEnemy = isEnemy;
        }

        public override char Rendering_on_the_map()
        {
            if (IsEnemy)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                return 'o'; // Пуля врага (медленная, видимая)
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                return '*'; // Пуля героя (если когда-нибудь сделаешь её видимой)
            }
        }
    }
}