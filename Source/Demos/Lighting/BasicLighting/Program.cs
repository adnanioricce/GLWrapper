using System;

namespace BasicLighting
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new BasicLightingGame(1280,720,"Basic Lighting");
            game.Start();
        }
    }
}
