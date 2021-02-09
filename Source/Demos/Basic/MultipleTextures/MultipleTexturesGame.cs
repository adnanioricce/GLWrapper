using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using GLWrapper.Graphics;

namespace MultipleTextures
{
    public class MultipleTexturesGame : BaseGame
    {
        private Model _model;
        public MultipleTexturesGame(int width,int height,string title) : base(WindowFactory.CreateDefaultWindow(width,height,title))
        {
        }

        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0.5f, 0.2f, 0.7f, 0.5f);
            Renderer.Draw(_model, time);
            base.Draw(time);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Setup()
        {
            var data = new ColoredTexturedVertex[]
            {
                new ColoredTexturedVertex(new Vector3(0.5f, 0.5f, 0.0f),Color4.AliceBlue,new Vector2(1.0f,1.0f)), //Top right
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, 0.0f),Color4.BlanchedAlmond,new Vector2(1.0f,0.0f)), //Bottom right
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, 0.0f),Color4.Fuchsia,new Vector2(0.0f,0.0f)), //Bottom left
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, 0.0f),Color4.Aqua,new Vector2(0.0f,1.0f)) //Top left
            };
            var indices = new[]
            {
                0,1,3,
                1,2,3
            };
            var textureContainer = Texture.LoadTexture("./Assets/Textures/container.png");
            var textureFace = Texture.LoadTexture("./Assets/Textures/awesomeface.png");
            var vertexAttributes = new []{
                new VertexAttribute("aPosition",3,VertexAttribPointerType.Float,ColoredTexturedVertex.Size,0),                
                new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 3),
                new VertexAttribute("aTexCoord", 2, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 7)
            };
            var shaderProgram = ShaderProgram.CreateShaderProgram("./Assets/Shaders/shader.vert","./Assets/Shaders/shader.frag",vertexAttributes);
            _model = Model.CreateModel(data,new  [] { textureContainer,textureFace},indices,shaderProgram);
            shaderProgram.SetInt("texture0",0);
            shaderProgram.SetInt("texture1",1);
            base.Setup();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }        

        public override void Update(float time)
        {
            if (_keyboardState.IsKeyDown(Keys.Escape))
            {
                Stop();
            }
            base.Update(time);
        }
    }
}