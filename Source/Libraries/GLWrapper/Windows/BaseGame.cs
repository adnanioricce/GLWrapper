using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

namespace GLWrapper.Windows
{
    public class BaseGame : IGame
    {
        private readonly GameWindow _window;
        protected readonly KeyboardState _keyboardState;
        protected readonly MouseState _mouseState;
        public BaseGame(GameWindow window)
        {
            _window = window;
            _window.RenderFrame += (e) =>
            {
                Draw((float)e.Time);
                
            };
            _window.UpdateFrame += ( e) =>
            {                
                Update((float)e.Time);
            };
            _window.Load += () =>
            {
                Setup();
                LoadContent();                
            };
            _window.MouseMove += (e) =>
            {                
                MouseMove();
            };
            _window.MouseWheel += (e) =>
            {
                Ioc.Camera.FovValue -= e.OffsetY;
                MouseWheel();
            };
            _keyboardState = _window.KeyboardState;
            _mouseState = _window.MouseState;
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
            Ioc.Camera = Camera.CreateCamera(_window.Size.X, _window.Size.Y);
            _window.CursorVisible = false;
        }
        public virtual void Update(float time)
        {
            
            var state = _window.KeyboardState;
            Ioc.Camera.Update(state, time);
            var mouseState = _window.MouseState;
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
            _window.Run();
        }
        public virtual void Stop()
        {
            _window.Close();
        }
        public virtual void Dispose()
        {
            _window.Dispose();
        }

        public void MouseMove()
        {
            if (_window.IsFocused)
            {                
                // var state = 
                // Mouse.SetPosition(_window.MousePosition.X + _window.Size.X / 2f, _window.MousePosition.Y + _window.Size.Y / 2f);
            }
        }

        public void MouseWheel()
        {            
            
        }
    }
}
