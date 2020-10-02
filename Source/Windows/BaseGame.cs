using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Windows
{
    public class BaseGame : IGame
    {
        private readonly CustomGameWindow _window;
        public BaseGame(CustomGameWindow window)
        {
            _window = window;
            _window.RenderFrame += (sender,e) =>
            {
                Draw(e.Time);
            };
            _window.UpdateFrame += (sender, e) =>
            {
                Update(e.Time);
            };
            _window.Load += (sender, e) =>
            {
                Setup();
                LoadContent();
            };            
        }
        public void LoadContent()
        {
            
        }
        public void Setup()
        {
            
        }
        public void Update(double time)
        {
            
        }
        public void Draw(double time)
        {
            
        }
        public void Start()
        {
            _window.Run(60.0);
        }
        public void Dispose()
        {
            _window.Dispose();
        }
    }
}
