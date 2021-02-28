using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
using GLWrapper.Windows;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Linq;

namespace GraphDemo
{
    public class Game : BaseGame
    {
        Graph<ColoredVertex> _graph;
        VertexBuffer _vbo;
        VertexArray _vao;
        ShaderProgram _shader;
        public Game(int width,int height,string title) : base(WindowFactory.CreateDefaultWindow(width,height,title))
        {
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Setup()
        {            
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.5f, 0.4f, 1.0f);
            var cubeData = GetCubeData().Select(d => new ColoredVertex(d)).ToArray();                              
            var positions = Enumerable.Range(0, 10).Select(i =>
            {
                float x = MathF.PI * i;
                float y = 4 * MathF.Sin(x);
                float z = -1.0f;
                var position = new Vector3(x, y, z);
                return position;
            }).ToArray();
            var attributes = new [] { 
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, sizeof(float) * (3 + 4), 0),
                new VertexAttribute("aColor",4, VertexAttribPointerType.Float, sizeof(float) * (3 + 4),sizeof(float) * 3)
            };
            var shader = ShaderProgram.CreateShaderProgram("Assets/Shaders/vertex.shader", "Assets/Shaders/frag.shader",attributes);
            _shader = shader;            
            _graph = Graph<ColoredVertex>.CreateGraph(cubeData, positions, shader);
            base.Setup();
            LogExtensions.LogGLError();
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(float time)
        {            
            HandleInput(time);            
            base.Update(time);            
        }
        public override void Draw(float time)
        {            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _graph.Bind();            
            _shader.SetProjection(Camera);
            var newPositions = _graph.Positions.Select(p =>
            {
                var position = new Vector3(p.X, MathF.Sin(MathF.PI * ((p.X + (_graph.Positions.IndexOf(p) + 1) * time) + time)), -1.0f);
                return position;
            }).ToArray();
            for (int i = 0;i < newPositions.Length;++i)
            {
                _graph.Positions[i] = newPositions[i];
                var model = Camera.Model;
                model = model * Matrix4.CreateScale(0.2f, 0.2f, 0.2f) * Matrix4.CreateTranslation(newPositions[i]);
                _shader.SetMatrix4(nameof(Camera.Model).ToLower(), model);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }
            base.Draw(time);
            LogExtensions.LogGLError();
        }
        public override void Stop()
        {
            base.Stop();
        }
        void HandleInput(float time)
        {
            _graph.Shader.SetFloat("u_time",time);
        }
        ColoredTexturedVertex[] GetCubeData()
        {
            return new ColoredTexturedVertex[]
            {
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Blue,new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Blue,new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Blue,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Blue,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Blue,new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Blue,new Vector2(0.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Blue,new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Blue,new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Blue,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Blue,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Blue,new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Blue, new Vector2(0.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Blue, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Blue, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Blue, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Blue, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Blue, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Blue, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Blue, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Blue, new Vector2(0.0f, 1.0f))
            };
        }
    }
}
