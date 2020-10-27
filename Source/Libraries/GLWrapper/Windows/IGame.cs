using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Windows
{
    public interface IGame : IDisposable
    {
        void Setup();
        void LoadContent();
        void Update(float time);
        void MouseMove();
        void MouseWheel();
        void Draw(float time);
    }
}
