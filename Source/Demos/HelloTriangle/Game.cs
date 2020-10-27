using GLWrapper;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Windows;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloTriangle
{
    public class Game : BaseGame
    {
        VertexArray VertexArrayObject;
        VertexBuffer VertexBufferObject;        
        ShaderProgram Shader;
        public Game(int width,int height,string title) : base(new GameWindow(width,height,GraphicsMode.Default,title))
        {
        }
        public override void Setup()
        {
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            var vertices = new Vertex[]
            {
                new Vertex(new Vector3(-0.5f, -0.5f, 0.0f)), //Bottom-left vertex
                new Vertex(new Vector3(0.5f, -0.5f, 0.0f)), //Bottom-right vertex
                new Vertex(new Vector3(0.0f,  0.5f, 0.0f)) //Top vertex                
            };
            
            
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.Bind();
            vbo.LoadData(vertices);                   
            var shader = ShaderProgram.CreateShaderProgram("Assets/vertex.shader", "Assets/frag.shader");
            shader.Use();
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            shader.SetVertexAttributes(new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, sizeof(float) * 3, 0));
            vbo.Bind();            
            VertexArrayObject = vao;
            VertexBufferObject = vbo;
            Shader = shader;
            base.Setup();
        }        
        public override void Update(float time)
        {
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
            {
                Stop();
            }
            base.Update(time);
        }
        public override void Draw(float time)
        {            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.UseProgram(Shader.Id);
            Shader.Use();
            VertexArrayObject.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            base.Draw(time);            
        }
        public override void Stop()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);            
            base.Stop();
        }
    }
}
