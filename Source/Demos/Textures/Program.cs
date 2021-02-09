using System;

namespace Textures
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new TextureGame(1280, 720, "Hello Texture");
            game.Start();
        }
    }
}
