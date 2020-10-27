using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
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
    public class CustomGameWindow : OpenTK.GameWindow
    {
        //Tem alguma maneira de definir uma lista de "comandos" que eu quero executar todo frame?        
        private readonly List<VertexArray> _vertexArrays = new List<VertexArray>();
        private readonly List<Model> _models = new List<Model>();
        private readonly Vector3[] positions = Program.CubePositions();
        private readonly List<ColoredVertex> Cubes = Program.GetCubeData().Select(v => new ColoredVertex(v)).ToList();
        private double _time = 0.0;
        private double _velocity = 0;
        private bool _firstMove = true;
        public CustomGameWindow(int width,int height,string title) : base(width,height,GraphicsMode.Default,title)
        {
            CursorVisible = false;
        }
        public void AddVertexArrays(params VertexArray[] vertexArrays)
        {
            _vertexArrays.AddRange(vertexArrays);       
        }
        public void AddModel(Model model)
        {
            _models.Add(model);
        }
        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback(LogExtensions.MessageCallBack, (IntPtr)0);            
            base.OnLoad(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused) // check to see if the window is focused
            {
                return;                
            }
            LogExtensions.LogGLError(this, nameof(OnUpdateFrame));
            HandleInput((float)e.Time);
            var resolution = 100;
            var step = 2f / 100;
            for (int i = 0, z = 0; z < resolution; ++z){
                var v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < resolution; ++x, ++i){
                    var u = (x + 0.5f) * step - 1f;                    
                    Program.Translations[i] = Program.Translations[i].WaveFunction(u, v, (float)_time);                    
                }
            }
            
            base.OnUpdateFrame(e);
        }

        private void HandleInput(float time)
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (_velocity >= 5) _velocity = 0;
            if (_time >= 5.0f)
            {
                _time = 0.0;
            }
            _velocity += 0.1;
            _time += time * _velocity;
            Ioc.Camera.Update(state, time);
            var mouseState = Mouse.GetState();
            if (_firstMove)
            {
                Ioc.Camera.LastPosition = new Vector2(mouseState.X, mouseState.Y);
                _firstMove = false;
            }
            else
            {
                Ioc.Camera.Rotate(mouseState, 1f);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.9f, 0.6f, 0.4f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _vertexArrays.ForEach(vao =>
            {
                vao.Bind();
                vao.Shaders.ForEach(shader => {
                    shader.Use();
                    shader.SetProjection(Ioc.Camera);
                    GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, 36, 100);
                });
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
