using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GLWrapper.Graphics;

namespace Textures
{
    public class TextureGame : BaseGame
    {
        private Model _model;
        public TextureGame(int width, int height, string title) : base(WindowFactory.CreateDefaultWindow(width, height, title))
        {

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
            var texture = Texture.LoadTexture("./Assets/Resources/container.png");            
            _model = Model.CreateModel(texture,data,indices);
            base.Setup();
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
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0.5f, 0.2f, 0.7f, 0.5f);
            Renderer.Draw(_model, time);
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