using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Windows
{
    public interface IGame : IDisposable
    {
        void Setup();
        void LoadContent();
        void Update(double time);
        void Draw(double time);
    }
}
