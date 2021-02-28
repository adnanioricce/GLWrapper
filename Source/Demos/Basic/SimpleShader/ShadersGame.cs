using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Graphics;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
namespace SimpleShaders
{
    public class Game : BaseGame
    {
        private Model _model;
        public Game(int width,int height,string title) : base(WindowFactory.CreateDefaultWindow(width,height,title))
        {

        }

        
        public override void Setup()
        {            
            var data = new ColoredVertex[]
            {
                new ColoredVertex(new Vector3(-0.5f, -0.5f, 0.0f),Color4.AliceBlue), //Bottom-left vertex
                new ColoredVertex(new Vector3(0.5f, -0.5f, 0.0f),Color4.BlanchedAlmond), //Bottom-right vertex
                new ColoredVertex(new Vector3(0.0f,  0.5f, 0.0f),Color4.Fuchsia) //Top vertex                
            };
            var vertexAttributes = new VertexAttribute[] {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, sizeof(float) * (3 + 4), 0),
                new VertexAttribute("aColor",4, VertexAttribPointerType.Float, sizeof(float) * (3 + 4),sizeof(float) * 3)
            };
            var shaderProgram = ShaderProgram.CreateShaderProgram("Assets/vertex.shader","Assets/frag.shader",vertexAttributes);
            _model = Model.CreateModel(data,shaderProgram);
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