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
        protected Vector2 _lastPos;
        protected bool _firstMove = true;
        private readonly GameWindow _window;        
        protected readonly KeyboardState KeyboardState;
        protected readonly MouseState MouseState;
        protected readonly Camera Camera;
        protected readonly Renderer Renderer;
        public bool IsFocused { get {return _window.IsFocused;}}
        public bool IsCursorGrabbed { get {return _window.CursorGrabbed;}}
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
            KeyboardState = _window.KeyboardState;
            MouseState = _window.MouseState;
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
            _window.CursorGrabbed = true;
        }
        public virtual void Update(float time)
        {
            if(!IsFocused){
                _window.CursorGrabbed = false;
                return;
            }
            if(KeyboardState.IsKeyDown(Keys.Escape)){
                this.Stop();
            }
            if(_firstMove){
                _lastPos = new Vector2(MouseState.X,MouseState.Y);
                _firstMove = false;
            }else {
                var deltaX = MouseState.X - _lastPos.X;
                var deltaY = MouseState.Y - _lastPos.Y;
                _lastPos = new Vector2(MouseState.X, MouseState.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                Camera.Yaw += deltaX * Camera.Sensivity;
                Camera.Pitch -= deltaY * Camera.Sensivity;
            }
            Camera.Update(KeyboardState, MouseState, time);
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
            Camera.Update(KeyboardState,MouseState);
        }

        public virtual void MouseWheel()
        {            
            
        }
        public virtual void Resize()
        {
            GL.Viewport(0, 0, this._window.ClientSize.X,this._window.ClientSize.Y);
            Camera.AspectRatio = this._window.ClientSize.X / (float)this._window.ClientSize.Y;
        }
    }
}
