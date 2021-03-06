using GLWrapper;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using GLWrapper.Graphics.Vertices;
using System;

namespace LightingMaps
{
    //TODO: Refactor Light and Model class to include the new lighting concerns
    public class LightingMapGame : BaseGame
    {
        private VertexArray _modelVao;        
        private Light Lamp;
        private ShaderProgram _modelShader;
        private Texture _diffuseMap;
        private Texture _specularMap;
        public LightingMapGame(int width, int height, string title) : base(width, height, title)
        {

        }
        public override void Setup()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            var vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("aPosition",3,VertexAttribPointerType.Float,TexturedVertex.Size, 0),
                new VertexAttribute("aNormal",3,VertexAttribPointerType.Float,TexturedVertex.Size ,3 * sizeof(float)),
                new VertexAttribute("aTexCoords",2,VertexAttribPointerType.Float,TexturedVertex.Size ,6 * sizeof(float))
            };
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(CubeVertices());
            _modelVao = VertexArray.CreateVertexArray();
            _modelVao.Bind();

            _modelShader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/vertex.vert", "./Assets/Shaders/fragment.frag", new[] { vertexAttributes[0] });
            _modelShader.Use();
            _modelShader.SetVertexAttributes();

            var lamp = Light.CreateLight();
            lamp.Bind();
            var lampShader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/vertex.vert", "./Assets/Shaders/lighting.frag", vertexAttributes);
            lampShader.Use();
            lampShader.SetVertexAttributes();
            _diffuseMap = Texture.LoadTexture("./Assets/Textures/container2.png");            
            _specularMap = Texture.LoadTexture("./Assets/Textures/container2_specular.png");            
            lampShader.SetInt("material.diffuse", 0);
            lampShader.SetInt("material.specular", 1);
            lampShader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            lampShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            lampShader.SetFloat("material.shininess", 32.0f);
            lampShader.SetVector3("light.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            lampShader.SetVector3("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
            lampShader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            lamp.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, vbo.Id);
            lamp.Position = new Vector3(1.2f, 1.0f, 2.0f);
            lamp.Shader = lampShader;
            lamp.VertexBuffer = vbo;
            Lamp = lamp;
        }
        public override void Update(float time)
        {
            base.Update(time);
        }
        public override void MouseMove()
        {
            base.MouseMove();
        }
        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);            
            Lamp.Shader.Use();
            _diffuseMap.Bind(0);
            Lamp.Shader.SetInt("material.diffuse", 0);
            _specularMap.Bind(1);
            Lamp.Shader.SetInt("material.specular", 1);
            
            Lamp.Shader.SetMatrix4("model", Matrix4.Identity);
            Lamp.Shader.SetMatrix4("view", Camera.View);
            Lamp.Shader.SetMatrix4("projection", Camera.Projection);
            Lamp.Shader.SetVector3("light.position", Lamp.Position);
            Lamp.Shader.SetVector3("light.ambient", new Vector3(0.2f));
            Lamp.Shader.SetVector3("light.diffuse", new Vector3(.5f));
            Lamp.Shader.SetVector3("light.specular", new Vector3(1.0f));
            Lamp.Shader.SetVector3("viewPos", Camera.Position);
            
            Lamp.Bind();
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            _modelShader.Use();
            var lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f);
            lampMatrix *= Matrix4.CreateTranslation(Lamp.Position);

            _modelShader.SetMatrix4("model", lampMatrix);
            _modelShader.SetMatrix4("view", Camera.View);
            _modelShader.SetMatrix4("projection", Camera.Projection);
            _modelVao.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            base.Draw(time);
        }
        protected TexturedVertex[] CubeVertices()
        {
            return new TexturedVertex[] {
                new TexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f,0.0f,-1.0f),new Vector2(0.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f, 0.0f,-1.0f),new Vector2(0.0f,0.0f)),

                new TexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),new Vector3(0.0f, 0.0f,1.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0.0f, 0.0f,1.0f),new Vector2(0.0f,0.0f)),

                new TexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),new Vector3(-1.0f, 0.0f,0.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),new Vector3(-1.0f, 0.0f,0.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(-1.0f, 0.0f,0.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(-1.0f, 0.0f,0.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),new Vector3(-1.0f, 0.0f,0.0f),new Vector2(0.0f,0.0f)),
                new TexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),new Vector3(-1.0f, 0.0f,0.0f),new Vector2(1.0f,0.0f)),

                new TexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),new Vector3(1.0f, 0.0f,0.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),new Vector3(1.0f, 0.0f,0.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(1.0f, 0.0f,0.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(1.0f, 0.0f,0.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),new Vector3(1.0f, 0.0f,0.0f),new Vector2(0.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),new Vector3(1.0f, 0.0f,0.0f),new Vector2(1.0f,0.0f)),

                new TexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f, -1.0f,0.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f,0.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(0.0f, -1.0f,0.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(0.0f, -1.0f,0.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),new Vector3(0.0f, -1.0f,0.0f),new Vector2(0.0f,0.0f)),
                new TexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f, -1.0f,0.0f),new Vector2(0.0f,0.0f)),

                new TexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),new Vector3(0.0f, 1.0f,0.0f),new Vector2(0.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3(0.0f, 1.0f,0.0f),new Vector2(1.0f,1.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 1.0f,0.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f, 1.0f,0.0f),new Vector2(1.0f,0.0f)),
                new TexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),new Vector3(0.0f, 1.0f,0.0f),new Vector2(0.0f,0.0f)),
                new TexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),new Vector3(0.0f, 1.0f,0.0f),new Vector2(0.0f,1.0f))
            };
        }
    }
}