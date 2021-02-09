using System;

namespace Transformations
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new TransformationsGame(1280,720,"Transformations");
            game.Start();
        }
    }
}
