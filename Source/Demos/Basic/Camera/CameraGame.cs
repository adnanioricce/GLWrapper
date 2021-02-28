using System.Collections.Generic;
using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using GLWrapper.Scene;

namespace CameraGame
{
    public class Game : BaseGame
    {
        VertexArray _vao;
        VertexBuffer _vbo;
        ElementBuffer _ebo;
        ShaderProgram _shader;
        List<Texture> textures = new List<Texture>();
        Model _model;
        float _time = 0.0f;
        public Game(int width, int height, string title) : base(width, height, title){}

        public override void Setup()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.4f, 0.7f, 0.9f, 0.5f);
            
            var vertices = new ColoredTexturedVertex[]{
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, 0.0f), Color4.Blue, new Vector2(1.0f,1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  -0.5f, 0.0f), Color4.Green, new Vector2(1.0f,0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, 0.0f), Color4.Red,new Vector2(0.0f,0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, 0.5f, 0.0f), Color4.Yellow,new Vector2(0.0f,1.0f))
            };
            var indices = new int []{
                0,1,3,
                1,2,3
            };
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            _vao = vao;
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertices);
            _vbo = vbo;
            var ebo = ElementBuffer.CreateElementBuffer();
            ebo.LoadData(indices);
            _ebo = ebo;
            var shader = ShaderProgramFactory.CreateDefault3DShaderProgramWithTexture();            
            _shader = shader;            
            var texture0 = Texture.LoadTexture("Assets/Textures/container.png");
            texture0.Bind(0);
            var texture1 = Texture.LoadTexture("Assets/Textures/awesomeface.png");
            texture1.Bind(1);            
            _shader.SetInt("texture1", 0);
            _shader.SetInt("texture2", 1);
            base.Setup();
        }
        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _time += 4.0f * time;
            Camera.Model = Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time));
            Renderer.Draw(_vao, 0, _ebo.IndicesCount);
            
            //Renderer.Draw(_model, time);
            base.Draw(time);
        }        
        
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void MouseMove()
        {
            base.MouseMove();
        }

        public override void MouseWheel()
        {
            base.MouseWheel();
        }        

        public override void Start()
        {
            base.Start();
        }                

        public override void Update(float time)
        {
            base.Update(time);
        }
    }
}