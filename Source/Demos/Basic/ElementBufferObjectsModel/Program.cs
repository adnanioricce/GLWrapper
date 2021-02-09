using System;

namespace ElementBufferObjectsModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new ElementBufferObjectGame(1280, 720, "Hello Element Buffer");
            game.Start();
        }
    }
}
