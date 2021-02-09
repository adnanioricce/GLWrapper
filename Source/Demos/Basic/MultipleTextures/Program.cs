using System;

namespace MultipleTextures
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new MultipleTexturesGame(1280,720,"Multiple Textures");
            game.Start();
        }
    }
}
