using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
namespace GLWrapper.Windows
{
    public class BaseGame : IGame
    {
        private readonly GameWindow _window;
        public BaseGame(GameWindow window)
        {
            _window = window;
            _window.RenderFrame += (sender,e) =>
            {
                Draw((float)e.Time);
                
            };
            _window.UpdateFrame += (sender, e) =>
            {
                Update((float)e.Time);
            };
            _window.Load += (sender, e) =>
            {
                Setup();
                LoadContent();                
            };
            _window.MouseMove += (sender, e) =>
            {                
                MouseMove();
            };
            _window.MouseWheel += (sender, e) =>
            {
                Ioc.Camera.FovValue -= e.DeltaPrecise;
                MouseWheel();
            };
        }
        public virtual void LoadContent()
        {
            
        }
        public virtual void Setup()
        {
            //GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback(LogExtensions.MessageCallBack, (IntPtr)0);
            Ioc.Camera = Camera.CreateCamera(_window.Width, _window.Height);
            _window.CursorVisible = false;
        }
        public virtual void Update(float time)
        {

            var state = Keyboard.GetState();
            Ioc.Camera.Update(state, time);
            var mouseState = Mouse.GetState();
            Ioc.Camera.Rotate(mouseState, 1f);            
            LogExtensions.LogGLError();
        }
        public virtual void Draw(float time)
        {
            LogExtensions.LogGLError();
            _window.SwapBuffers();
        }
        public virtual void Start()
        {
            _window.Run(60.0);
        }
        public virtual void Stop()
        {
            _window.Exit();
        }
        public virtual void Dispose()
        {
            _window.Dispose();
        }

        public void MouseMove()
        {
            if (_window.Focused)
            {
                var state = Mouse.GetState();
                Mouse.SetPosition(_window.X + _window.Width / 2f, _window.Y + _window.Height / 2f);
            }
        }

        public void MouseWheel()
        {            
            
        }
    }
}
