using System;

namespace HelloTriangle
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(1280, 720, "Hello Triangle");
            game.Start();
        }
    }
}
