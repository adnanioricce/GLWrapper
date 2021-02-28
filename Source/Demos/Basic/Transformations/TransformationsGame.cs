using System.Linq;
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
        private Vector3[] _positions = new Vector3[]{
            new Vector3( 0.0f,  0.0f,  0.0f), 
            new Vector3( 2.0f,  5.0f, -15.0f), 
            new Vector3(-1.5f, -2.2f, -2.5f),  
            new Vector3(-3.8f, -2.0f, -12.3f),  
            new Vector3( 2.4f, -0.4f, -3.5f),  
            new Vector3(-1.7f,  3.0f, -7.5f),  
            new Vector3( 1.3f, -2.0f, -2.5f),  
            new Vector3( 1.5f,  2.0f, -2.5f), 
            new Vector3( 1.5f,  0.2f, -1.5f), 
            new Vector3(-1.3f,  1.0f, -1.5f)  
        };
        public TransformationsGame(int width,int height,string title) : base(width,height,title)
        {
            
        }
        public override void Setup()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            var data = CubeVertices();
            var indices = Enumerable.Range(0,36).ToArray();            
            var textureContainer = Texture.LoadTexture("./Assets/Textures/container.png");
            var textureFace = Texture.LoadTexture("./Assets/Textures/awesomeface.png");
            var vertexAttributes = new []{
                new VertexAttribute("aPosition",3,VertexAttribPointerType.Float,ColoredTexturedVertex.Size,0),
                new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 3),
                new VertexAttribute("aTexCoord", 2, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 7)
            };
            var shaderProgram = ShaderProgram.CreateShaderProgram("./Assets/Shaders/shader.vert","./Assets/Shaders/shader.frag",vertexAttributes);
            _model = Model.CreateModel(data,new  [] { textureContainer,textureFace},indices,shaderProgram);
            shaderProgram.SetInt("texture1",0);
            shaderProgram.SetInt("texture2",1);            
            base.Setup();
        }
        public override void Update(float time)
        {   
            
            Camera.Model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));                     
            base.Update(time);
        }
        public override void Draw(float time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _time += 4.0f * time;                        
            for (int i = 0; i < 10; i++)
            {
                var model = Matrix4.Identity;
                model = Camera.Model * Matrix4.CreateTranslation(_positions[i]);
                var angle = (float)MathHelper.Sin(20.0f * i ) * _time;
                model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f,0.3f,0.5f),MathHelper.DegreesToRadians(angle));
                Camera.Model = model;
                Renderer.Draw(_model,Camera,_time);
            }
            base.Draw(time);
        }
        public override void MouseMove()
        {            
            base.MouseMove();
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