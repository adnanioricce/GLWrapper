using GLWrapper;
using GLWrapper.Factories;
using GLWrapper.Graphics;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Windows;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
        public Game(int width,int height,string title) : base(WindowFactory.CreateDefaultWindow(width,height,title))
        {
        }
        public override void Setup()
        {
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            var vertices = new ColoredVertex[]
            {
                new ColoredVertex(new Vector3(-0.5f, -0.5f, 0.0f),Color4.AliceBlue), //Bottom-left vertex
                new ColoredVertex(new Vector3(0.5f, -0.5f, 0.0f),Color4.BlanchedAlmond), //Bottom-right vertex
                new ColoredVertex(new Vector3(0.0f,  0.5f, 0.0f),Color4.Fuchsia) //Top vertex                
            };
            
            
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.Bind();
            vbo.LoadData(vertices);
            var attributes = new [] {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, sizeof(float) * (3 + 4), 0),
                new VertexAttribute("aColor",4, VertexAttribPointerType.Float, sizeof(float) * (3 + 4),sizeof(float) * 3)             
            };
            var shader = ShaderProgram.CreateShaderProgram("Assets/vertex.shader", "Assets/frag.shader",attributes);
            shader.Use();            
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            shader.Use();
            shader.SetVertexAttributes();
            vbo.Bind();
            VertexArrayObject = vao;
            VertexBufferObject = vbo;
            Shader = shader;
            base.Setup();
        }        
        public override void Update(float time)
        {            
            base.Update(time);
        }
        public override void Draw(float time)
        {            
            GL.Clear(ClearBufferMask.ColorBufferBit);            
            Shader.Use();
            //VertexArrayObject.Bind();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            Renderer.Draw(VertexArrayObject, 0, 3);
            base.Draw(time);            
        }
        public override void Stop()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);            
            base.Stop();
        }
    }
}
