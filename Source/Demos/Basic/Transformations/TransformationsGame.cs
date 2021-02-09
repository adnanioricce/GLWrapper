using GLWrapper;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Transformations
{
    public class TransformationsGame : BaseGame
    {        
        private Model _model;
        private float _time = 0.0f;
        public TransformationsGame(int width,int height,string title) : base(width,height,title)
        {
            
        }
        public override void Setup()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
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
        public override void Update(float time)
        {                        
            base.Update(time);
        }
        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _time += 4.0f * time;                        
            Camera.Model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));            
            Renderer.Draw(_model,Camera,_time);
            base.Draw(time);
        }
        public override void MouseMove()
        {
            // Camera.Rotate(_mouseState,_time);
            // Camera.Update(_keyboardState,_time);
            base.MouseMove();
        }
    }
}