using System;

namespace Materials
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new MaterialGame(1280,720, "Material Game");
            game.Start();
        }
    }
}
