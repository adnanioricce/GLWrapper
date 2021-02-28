using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Graphics;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloTriangleModel
{
    public class TriangleModelGame : BaseGame
    {
        private Model _model;
        public TriangleModelGame(int width,int height,string title) : base(WindowFactory.CreateDefaultWindow(width,height,title))
        {

        }

        
        public override void Setup()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            _model = Model.CreateModel(new ColoredVertex[]
            {
                new ColoredVertex(new Vector3(-0.5f, -0.5f, 0.0f),Color4.AliceBlue), //Bottom-left vertex
                new ColoredVertex(new Vector3(0.5f, -0.5f, 0.0f),Color4.BlanchedAlmond), //Bottom-right vertex
                new ColoredVertex(new Vector3(0.0f,  0.5f, 0.0f),Color4.Fuchsia) //Top vertex                
            });
            
            base.Setup();
        }
        public override void Update(float time)
        {                        
            base.Update(time);
        }

        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Renderer.Draw(_model,time);            
            base.Draw(time);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }        

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {            
            base.Stop();
        }

        
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
