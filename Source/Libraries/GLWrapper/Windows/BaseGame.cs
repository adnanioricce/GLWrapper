using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using GLWrapper.Factories;
using GLWrapper.Graphics;
using OpenTK.Mathematics;

namespace GLWrapper.Windows
{
    public class BaseGame : IGame
    {
        private readonly GameWindow _window;        
        protected readonly KeyboardState _keyboardState;
        protected readonly MouseState _mouseState;
        protected readonly Camera Camera;
        protected readonly Renderer Renderer;
        public bool IsFocused { get {return _window.IsFocused;}}        
        public BaseGame(GameWindow window)
        {
            _window = window;
            _window.RenderFrame += (e) =>
            {
                Update((float)e.Time);
                Draw((float)e.Time);
                
            };
            _window.UpdateFrame += (e) =>
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
                Camera.FovValue -= e.OffsetY;
                MouseWheel();
            };
            _keyboardState = _window.KeyboardState;
            _mouseState = _window.MouseState;
            Camera = Camera.CreateCamera(_window.Size.X,_window.Size.Y);
            Renderer = new Renderer();
        }
        public BaseGame(int width,int height,string title) : this(WindowFactory.CreateDefaultWindow(width,height,title))
        {            
        }
        public virtual void LoadContent()
        {
            
        }
        public virtual void Setup()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback(LogExtensions.MessageCallBack, (IntPtr)0);
            _window.CursorVisible = false;
        }
        public virtual void Update(float time)
        {            
            Camera.Update(_keyboardState, _mouseState, time);            
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

        public virtual void MouseMove()
        {
            if(!IsFocused){                
                return;
            }
            Camera.Update(_keyboardState,_mouseState);            
        }

        public virtual void MouseWheel()
        {            
            
        }
    }
}
