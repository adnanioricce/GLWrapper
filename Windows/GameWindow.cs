using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GLWrapper
{
    public class GameWindow : OpenTK.GameWindow
    {
        //Tem alguma maneira de definir uma lista de "comandos" que eu quero executar todo frame?        
        private readonly List<VertexArray> _vertexArrays = new List<VertexArray>();        
        private readonly Vector3[] positions = Program.CubePositions();        
        private double _time = 0.0;
        private double _velocity = 0;
        private bool _firstMove = true;
        public GameWindow(int width,int height,string title) : base(width,height,GraphicsMode.Default,title)
        {
            CursorVisible = false;
        }
        public void AddVertexArrays(params VertexArray[] vertexArrays)
        {
            _vertexArrays.AddRange(vertexArrays);       
        }       
        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback(Logger.MessageCallBack, (IntPtr)0);            
            base.OnLoad(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused) // check to see if the window is focused
            {
                return;
            }
            Logger.Log(this.GetType().Name, nameof(OnUpdateFrame));
            var state = Keyboard.GetState();            
            if (state.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (_velocity >= 1280) _velocity = 0;
            if (_time >= 720.0)
            {
                _time = 0.0;
            }
            _velocity += 0.1;
            _time += e.Time * _velocity;
            //_vertexArrays.ForEach(vertexArray =>
            //{
                //Ioc.Camera.Model = Matrix4.CreateRotationY(-(float)MathHelper.DegreesToRadians(_time)) * Matrix4.CreateRotationX(-(float)MathHelper.DegreesToRadians(_time));
            //});
            Ioc.Camera.Update(state, (float)e.Time);
            var mouseState = Mouse.GetState();
            if (_firstMove)
            {
                Ioc.Camera.LastPosition = new Vector2(mouseState.X, mouseState.Y);
                Console.WriteLine(Ioc.Camera.LastPosition);
                _firstMove = false;
            }
            else
            {
                Ioc.Camera.Rotate(mouseState, 1f);                
            }
            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Logger.Log(this.GetType().Name, nameof(OnRenderFrame));
            _vertexArrays.ForEach(vao =>
            {
                DrawFunctions.DrawCubeWithLightning(vao, vao.VertexBuffer.VerticesCount);
                //DrawFunctions.DrawCube(vao, vao.VertexBuffer.VerticesCount);                
            });
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }        
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            Ioc.Camera.AspectRatio = Width / (float)Height;
            base.OnResize(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused)
            {
                var state = Mouse.GetState();
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);                
                Console.WriteLine("mouse position: {0},{1}", state.X, state.Y);
            }
            
            base.OnMouseMove(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Ioc.Camera.FovValue -= e.DeltaPrecise;
            base.OnMouseWheel(e);
        }

    }
}
