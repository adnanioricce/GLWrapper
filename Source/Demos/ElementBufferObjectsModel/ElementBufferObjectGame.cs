using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Graphics;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ElementBufferObjectsModel
{
    public class ElementBufferObjectGame : BaseGame
    {
        private Model _model;        
        private uint[] indices = new uint[]{
                0,1,3,
                1,2,3,
                // 3,4,5
        };
        public ElementBufferObjectGame(int width,int height,string title) : base(WindowFactory.CreateDefaultWindow(width,height,title))
        {
        }
        public override void Setup()
        {                           
            base.Setup();                     
            var vertices = new ColoredVertex[]
            {
                new ColoredVertex(new Vector3(0.5f, 0.5f, 0.0f),Color4.AliceBlue), //Bottom-left vertex
                new ColoredVertex(new Vector3(0.5f, -0.5f, 0.0f),Color4.BlanchedAlmond), //Bottom-right vertex
                new ColoredVertex(new Vector3(-0.5f,  -0.5f, 0.0f),Color4.Fuchsia), //Top vertex
                new ColoredVertex(new Vector3(-0.5f,  -0.5f, 0.0f),Color4.Red),
                new ColoredVertex(new Vector3(-0.5f,  0.5f, 0.0f),Color4.Blue),
                new ColoredVertex(new Vector3(0.5f,  0.5f, 0.0f),Color4.Yellow)
            };                        
            _model = Model.CreateModel(vertices);
            
            
        }        
        public override void Update(float time)
        {
            if (_keyboardState.IsKeyDown(Keys.Escape))
            {
                Stop();
            }
            base.Update(time);
        }
        public override void Draw(float time)
        {               
            base.Draw(time);
            Renderer.Draw(_model,time);            
        }        
    }
}