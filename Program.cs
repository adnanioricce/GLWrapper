using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Linq;
using GLWrapper.Graphics.Vertices;

namespace GLWrapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            Ioc.ScreenSize = (1280, 720);
            Ioc.Camera = Camera.CreateCamera(Ioc.ScreenSize.Width, Ioc.ScreenSize.Height);
            using var game = new GameWindow(Ioc.ScreenSize.Width,Ioc.ScreenSize.Height,"TkCube");
            DrawCubeWithLightning(game);
            game.Run(60.0);
        }
        public static void DrawMultipleCubesWithTextures(GameWindow game)
        {
            var vertices = GetCubeData();
            var vertexArray = VertexArray.CreateVertexArray(vertices);
            var shaderProgram = ShaderProgram.CreateShaderProgram("./Assets/Shaders/vertex.shader", "./Assets/Shaders/lightingFragment.shader");
            var attributes = GetAttributes();
            var indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };
            var elementBuffer = ElementBuffer.CreateElementBuffer(indices);
            var containerTexture = Texture.LoadTexture("./Assets/Textures/container.jpg");
            var niceFaceTexture = Texture.LoadTexture("./Assets/Textures/awesomeface.png");
            shaderProgram.Use();
            shaderProgram.SetInt("texture1", 0);
            shaderProgram.SetInt("texture2", 1);
            
            shaderProgram.SetVertexAttributes(attributes);
            shaderProgram.Textures.AddRange(new Texture[] { containerTexture, niceFaceTexture });
            vertexArray.ElementBuffer = elementBuffer;
            vertexArray.Shader.Add(shaderProgram);
            vertexArray.Camera = Camera.CreateCamera(game.Width, game.Height);
            game.AddVertexArrays(vertexArray);            
        }
        public static void DrawCubeWithLightning(GameWindow game)
        {
            var vertices = GetCubeData().Select(v => new Vertex(v))
                                        .ToArray();
            VertexBuffer vbo = VertexBuffer.CreateVertexObject(vertices);
            ShaderProgram lightningShader = ShaderProgram.CreateShaderProgram("Assets/Shaders/lampVertex.shader", "Assets/Shaders/lightningFragment.shader");
            ShaderProgram lampShader = ShaderProgram.CreateShaderProgram("Assets/Shaders/lampVertex.shader", "Assets/Shaders/basicFrag.shader");
            VertexArray vaoModel = VertexArray.CreateVertexArray();
            vaoModel.Bind();
            vbo.Bind();
            lightningShader.SetVertexAttributes(GetLightedAttributes());            
            vaoModel.Camera = Ioc.Camera;
            vaoModel.Shader.AddRange(new[] { lightningShader });
            vaoModel.VertexBuffer = vbo;
            Ioc.Camera.LightPosition = new Vector3(1.2f, 1.0f, 2.0f);
            game.AddVertexArrays(vaoModel);            
            var lamp = Lamp.CreateLamp();
            lamp.Shader = lampShader;            
            lamp.Bind();
            vbo.Bind();
            lampShader.SetVertexAttributes(GetLightedAttributes());
            vaoModel.Lamp = lamp;
        }
        public static VertexAttribute[] GetLightedAttributes()
        {
            return new VertexAttribute[]
            {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, Vertex.Size,0)                
            };
        }        
        public static VertexAttribute[] GetAttributes()
        {
            return new VertexAttribute[] 
            {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, 3 * sizeof(float)),
                new VertexAttribute("aTexCoord", 2, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, 7 * sizeof(float))
            };
        }
        public static Vector3[] CubePositions()
        {
            return new Vector3[]
            {
                new Vector3(0.0f,0.0f,0.0f),
                new Vector3(2.0f,5.0f,-15.0f),
                new Vector3(-1.5f,-2.2f,-2.5f),
                new Vector3(-3.8f,-2.0f,-12.3f),
                new Vector3(-2.4f,-0.4f,-3.5f),
                new Vector3(-1.7f,3.0f,-7.5f),
                new Vector3(1.3f,3.0f,-7.5f),
                new Vector3(1.5f,2.0f,-2.5f),
                new Vector3(1.5f,0.2f,-1.5f),
                new Vector3(-1.3f,1.0f,-1.5f),
            };
        }
        static ColoredTexturedVertex[] GetCubeData()
        {
            return new ColoredTexturedVertex[]
            {
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Transparent,new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Transparent,new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Transparent,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Transparent,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Transparent,new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Transparent,new Vector2(0.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Transparent,new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Transparent,new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Transparent,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Transparent,new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Transparent,new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Transparent, new Vector2(0.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Transparent, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Transparent, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Transparent, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Transparent, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f,  0.5f),Color4.Transparent, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),

                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, -0.5f),Color4.Transparent, new Vector2(1.0f, 1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(1.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f,  0.5f),Color4.Transparent, new Vector2(0.0f, 0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, -0.5f),Color4.Transparent, new Vector2(0.0f, 1.0f))
            };                          
        }
        static ColoredTexturedVertex[] GetSimpleRectangleData()
        {
            return new ColoredTexturedVertex[]
            {
                new ColoredTexturedVertex(new Vector3(0.5f,  0.5f, 0.0f),Color4.Lime,new Vector2(1.0f,1.0f)),
                new ColoredTexturedVertex(new Vector3(0.5f, -0.5f, 0.0f),Color4.Magenta,new Vector2(1.0f,0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f, -0.5f, 0.0f),Color4.NavajoWhite,new Vector2(0.0f,0.0f)),
                new ColoredTexturedVertex(new Vector3(-0.5f,  0.5f, 0.0f ),Color4.Moccasin, new Vector2(0.0f,1.0f))
            };
        }
    }
}
