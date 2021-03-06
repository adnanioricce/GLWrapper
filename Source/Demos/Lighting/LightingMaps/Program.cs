using System;

namespace LightingMaps
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new LightingMapGame(1280, 720, "Lighting Map");
            game.Start();
        }
    }
}
