using GLWrapper;
using GLWrapper.Windows;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using GLWrapper.Graphics.Vertices;

namespace BasicLighting
{
    public class BasicLightingGame : BaseGame
    {
        private VertexArray _modelVao;
        private Matrix4 _lampMatrix = Matrix4.Identity;
        private Light Lamp;
        private ShaderProgram _modelShader;
        public BasicLightingGame(int width,int height,string title) : base(width,height,title)
        {
            
        }
        public override void Setup()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            var vertexAttributes = new VertexAttribute[]{
                new VertexAttribute("aPosition",3,VertexAttribPointerType.Float,ColoredTexturedVertex.Size, 0),
                new VertexAttribute("aNormal",3,VertexAttribPointerType.Float,ColoredVertex.Size,3 * sizeof(float))
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
            base.Update(time);
        }
        public override void MouseMove()
        {
            base.MouseMove();
        }
        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _modelVao.Bind();
            Lamp.Shader.Use();
            Lamp.Shader.SetMatrix4("model",Matrix4.Identity);
            Lamp.Shader.SetMatrix4("view",Camera.View);
            Lamp.Shader.SetMatrix4("projection",Camera.Projection);
            Lamp.Shader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            Lamp.Shader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
            Lamp.Shader.SetVector3("viewPos", Camera.Position);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            _modelVao.Bind();
            _modelShader.Use();
            var lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f);
            lampMatrix *= Matrix4.CreateTranslation(Lamp.Position);
            _modelShader.SetMatrix4("model", lampMatrix);
            _modelShader.SetMatrix4("view",Camera.View);
            _modelShader.SetMatrix4("projection",Camera.Projection);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            base.Draw(time);
        }        
        protected ColoredTexturedVertex[] CubeVertices()
        {
            return new ColoredTexturedVertex[] {
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f), Color4.Violet ,new Vector2( 0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Turquoise,new Vector2( 1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.SkyBlue, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Thistle, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Pink, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.MediumSeaGreen, new Vector2(0.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f), Color4.Navy,new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f), Color4.Lavender, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Khaki, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Gainsboro, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Honeydew, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.BurlyWood, new Vector2(0.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Crimson, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.FloralWhite, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Linen, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Maroon, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.PaleVioletRed, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Snow,new Vector2(1.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Wheat, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.SpringGreen, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.RoyalBlue,new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.SaddleBrown,new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.PapayaWhip, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.NavajoWhite, new Vector2(1.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.OrangeRed, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.PaleTurquoise, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Plum, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.RosyBrown, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Salmon, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.SteelBlue, new Vector2(0.0f, 1.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Yellow, new Vector2( 0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Peru, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.OldLace, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.MintCream, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Moccasin, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.LightPink, new Vector2(0.0f, 1.0f))
            };
        }
    }
}