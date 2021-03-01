using System;

namespace Colors
{
    class Program
    {
        static void Main(string[] args)
        {
            var colorsGame = new LightingGame(1280,720,"Colors");
            colorsGame.Start();
        }
    }
}
