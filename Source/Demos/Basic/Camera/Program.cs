﻿using System;

namespace CameraGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(1280,720, "Hello Camera");
            game.Start();
        }
    }
}
