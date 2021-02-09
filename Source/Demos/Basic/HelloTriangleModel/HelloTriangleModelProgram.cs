using System;

namespace HelloTriangleModel
{
    class HelloTriangleModelProgram
    {
        static void Main(string[] args)
        {
            var game = new TriangleModelGame(1280, 720, "Hello Triangle");
            game.Start();
        }
    }
}
