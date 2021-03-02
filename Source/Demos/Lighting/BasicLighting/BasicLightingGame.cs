using GLWrapper;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using GLWrapper.Graphics.Vertices;
using System.Linq;
using System;

namespace BasicLighting
{
    public class BasicLightingGame : BaseGame
    {
        private VertexArray _modelVao;
        private Matrix4 _lampMatrix = Matrix4.Identity;
        private Light Lamp;
        private ShaderProgram _modelShader;
        private float _time = 0.0f;
        private bool goBack = false;
        public BasicLightingGame(int width,int height,string title) : base(width,height,title)
        {
            
        }
        public override void Setup()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            var vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("aPosition",3,VertexAttribPointerType.Float,Vertex.Size, 0),
                new VertexAttribute("aNormal",3,VertexAttribPointerType.Float,Vertex.Size,3 * sizeof(float))
            };
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(CubeVertices());
            _modelVao = VertexArray.CreateVertexArray();
            _modelVao.Bind();
            
            _modelShader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/vertex.vert","./Assets/Shaders/fragment.frag",vertexAttributes);
            _modelShader.Use();
            _modelShader.SetVertexAttributes();
            
            var lamp = Light.CreateLight();
            lamp.Bind();
            var lampShader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/vertex.vert","./Assets/Shaders/lighting.frag",vertexAttributes);
            lampShader.Use();
            lampShader.SetVertexAttributes();            
            lamp.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, vbo.Id);
            lamp.Position = new Vector3(1.2f,1.0f,2.0f);
            lamp.Shader = lampShader;
            lamp.VertexBuffer = vbo;
            Lamp = lamp;
        }
         public override void Update(float time)
        {
            // Lamp.Shader.Use();
            
            base.Update(time);
        }
        public override void MouseMove()
        {
            base.MouseMove();
        }
        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _time += goBack ? -time : time;
            
            Lamp.Shader.Use();
            Lamp.Shader.SetMatrix4("model",Matrix4.Identity);
            Lamp.Shader.SetMatrix4("view",Camera.View);
            Lamp.Shader.SetMatrix4("projection",Camera.Projection);
            Lamp.Shader.SetVector3("objectColor", Lamp.Position);
            Lamp.Shader.SetVector3("lightColor", new Vector3(_time)); 
            Lamp.Shader.SetFloat("time",_time);
            Lamp.Shader.SetVector3("viewPos", Camera.Position);
            Lamp.Shader.SetVector3("lightPos", Lamp.Position);
            Lamp.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);            
            _modelShader.Use();
            var lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.5f);
            lampMatrix *= Matrix4.CreateTranslation(Lamp.Position) * Matrix4.CreateRotationY((Lamp.Position * _time).X);
            
            _modelShader.SetMatrix4("model", lampMatrix);
            _modelShader.SetMatrix4("view",Camera.View);
            _modelShader.SetMatrix4("projection",Camera.Projection);
            _modelVao.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            if(_time >= 360.0f || _time < 0.0f){
                goBack = !goBack;
            }
            base.Draw(time);
        }        
        protected Vertex[] CubeVertices()
        {
            return new Vertex[] {
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f,0.0f,-1.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f)),

                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f),new Vector3(0.0f, 0.0f,1.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f)),

                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f),new Vector3(-1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f),new Vector3(-1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(-1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(-1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f),new Vector3(-1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f),new Vector3(-1.0f, 0.0f,0.0f)),

                new Vertex(new Vector3(0.5f,  0.5f,  0.5f),new Vector3(1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f),new Vector3(1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f),new Vector3(1.0f, 0.0f,0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f),new Vector3(1.0f, 0.0f,0.0f)),

                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f, -1.0f,0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(0.0f, -1.0f,0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f),new Vector3(0.0f, -1.0f,0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f),new Vector3(0.0f, -1.0f,0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0.0f, -1.0f,0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f,0.0f)),

                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(0.0f, 1.0f,0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3(0.0f, 1.0f,0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 1.0f,0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 1.0f,0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(0.0f, 1.0f,0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(0.0f, 1.0f,0.0f))
            };
        }
    }
}