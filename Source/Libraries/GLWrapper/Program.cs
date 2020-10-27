using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Linq;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;
using Serilog;
using GLWrapper.Support;
using System.Collections.Generic;
using System;

namespace GLWrapper
{
    public static class Extensions
    {
        public static Vector3 WaveFunction(this Vector3 position, float u, float v, float time)
        {
            position.X = u;
            position.Y = (float)Math.Sin(Math.PI * (u + time));
            position.Z = v;
            return position;
        }
    }
    public class Program
    {
        public static List<Vector3> Translations = new List<Vector3>();
        static void Main(string[] args)
        {
            InitializeSerilog();
            Ioc.Settings = Setting.CreateSettings(settings =>
            {
                settings.ScreenSize = (1600, 900);
                return settings;
            });            
            var (width,height) = Ioc.Settings.ScreenSize;
            Ioc.Camera = Camera.CreateCamera(width, height);
            
        }
        
        
        static void InitializeSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();
        }
        public static void DrawInstanced()
        {
            var (width, height) = Ioc.Settings.ScreenSize;
            using var game = new CustomGameWindow(width, height, "TkCube");
            var rng = new Random(22222222);
            var vertices = GetCubeData().Select(v => new ColoredVertex(v.Position * 0.09f, new Color4(v.Color.R * (float)rng.NextDouble(), v.Color.G * (float)rng.NextDouble(), v.Color.B * (float)rng.NextDouble(), v.Color.A)))
                                        .ToArray();
            var resolution = 100;
            var step = 2f / 100;
            for (int i = 0, z = 0; z < resolution; ++z)
            {
                var v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < resolution; ++x, ++i)
                {
                    var u = (x + 0.5f) * step - 1f;
                    Translations.Add(Vector3.Multiply(Vector3.Zero.WaveFunction(u, v, 0.4f), 4f));
                }
            }
            var vao = VertexArray.CreateVertexArray();
            vao.Bind();
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertices);
            var shader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/basicVertexWithColor.shader", "./Assets/Shaders/basicFragWithInputColor.shader");
            shader.SetVertexAttributes(new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredVertex.Size, 0),
                                       new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredVertex.Size, ColoredVertex.PositionStride));
            shader.Use();
            shader.SetFloat("u_PI", MathHelper.Pi);
            Translations.ForEach((translation) => shader.SetVector3($"offsets[{Translations.IndexOf(translation)}]", translation));
            vao.Shaders.Add(shader);
            game.AddVertexArrays(vao);
            game.Run(60.0);
        }
        public static void DrawWithModel(CustomGameWindow game)
        {
            var model = Model.CreateModel(GetCubeData().Select((v,i) => {
                v.Color = new Color4(v.Color.A * (i / 10f),v.Color.B * (i / 100f),v.Color.G * (i / 5f),v.Color.A);
                return v;
            }).Select(v => new ColoredVertex(v))
                                                       .ToArray());
            var shader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/basicVertexWithColor.shader", "./Assets/Shaders/basicFragWithInputColor.shader");            
            shader.SetVertexAttributes(new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredVertex.Size, 0),
                                       new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredVertex.Size, ColoredVertex.PositionStride));
            shader.SetProjection(Ioc.Camera);
            model.SetShaderProgram(shader);            
            model.OnDraw = (vbo, shader) =>
            {                               
                GL.DrawArrays(PrimitiveType.Triangles, 0, vbo.VerticesCount);
            };
            game.AddModel(model);            
        }
        public static void DrawPoint(CustomGameWindow game)
        {
            var data = GetCubeData().Select(d => {
                d.Color = Color4.Azure;
                return new ColoredVertex(d);
                }).ToArray();
            var vertexArray = VertexArray.CreateVertexArray();
            vertexArray.Bind();
            var vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(data);
            vbo.Bind();
            var shaderProgram = ShaderProgram.CreateShaderProgram("./Assets/Shaders/basicVertexWithColor.shader", "./Assets/Shaders/basicFragWithInputColor.shader");
            shaderProgram.SetVertexAttributes(new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredVertex.Size, 0),
                                              new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredVertex.Size, ColoredVertex.PositionStride));
            vertexArray.Camera = Camera.CreateCamera(game.Width, game.Height);
            vertexArray.Shaders.Add(shaderProgram);
            vertexArray.VertexBuffer = vbo;
            game.AddVertexArrays(vertexArray);
        }
        public static void DrawMultipleCubesWithTextures(CustomGameWindow game)
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
            vertexArray.Shaders.Add(shaderProgram);
            vertexArray.Camera = Camera.CreateCamera(game.Width, game.Height);
            game.AddVertexArrays(vertexArray);            
        }
        public static void DrawCubeWithLightning(CustomGameWindow game)
        {
            var lightPosition = new Vector3(1.2f, 1.0f, 2.0f);
            var vertices = GetCubeDataWithNormals();
            VertexBuffer vbo = VertexBuffer.CreateVertexBuffer();
            vbo.LoadData(vertices);
            ShaderProgram lightningShader = ShaderProgram.CreateShaderProgram("Assets/Shaders/basicVertex.shader", "Assets/Shaders/basicFrag.shader");
            ShaderProgram lampShader = ShaderProgram.CreateShaderProgram("Assets/Shaders/lampVertex.shader", "Assets/Shaders/lightningFragment.shader");
            VertexArray vaoModel = VertexArray.CreateVertexArray();
            vaoModel.Bind();
            vbo.Bind();
            lightningShader.Use();
            lightningShader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            lightningShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
            lightningShader.SetVector3("lightPos", lightPosition);
            lightningShader.SetVertexAttributes(new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, Vertex.Size, 0));
            vaoModel.Camera = Ioc.Camera;
            vaoModel.Shaders.AddRange(new[] { lightningShader });
            vaoModel.VertexBuffer = vbo;
            game.AddVertexArrays(vaoModel);
            var vaoLight = Light.CreateLight();
            vaoLight.Bind();
            vbo.Bind();
            lampShader.Use();
            lampShader.SetVertexAttributes(new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, Vertex.Size, 0),
                                           new VertexAttribute("aNormal", 3, VertexAttribPointerType.Float, Vertex.Size, 3 * sizeof(float)));
            vaoLight.Shader = lampShader;
            vaoLight.VertexBuffer = vbo;
            vaoLight.Position = lightPosition;
            vaoModel.Lamp = vaoLight; 
        }
        public static VertexAttribute[] GetLightedAttributes()
        {
            return new VertexAttribute[]
            {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, Vertex.Size,0),
                new VertexAttribute("aNormal", 3, VertexAttribPointerType.Float, Vertex.Size,3 * sizeof(float))
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
        static Vertex[] GetCubeDataWithNormals()
        {
           return new Vertex [] 
           { 
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f,  0.0f, -1.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.0f,  0.0f, -1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3(0.0f,  0.0f, -1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3( 0.0f,  0.0f, -1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(0.0f,  0.0f, -1.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f,  0.0f, -1.0f)),

                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0.0f,  0.0f,  1.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(0.0f,  0.0f,  1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f,  0.0f,  1.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f,  0.0f,  1.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(0.0f,  0.0f,  1.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0.0f,  0.0f,  1.0f)),

                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(-1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3( -1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(-1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(-1.0f,  0.0f,  0.0f)),

                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3(1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(1.0f,  0.0f,  0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(1.0f,  0.0f,  0.0f)),

                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f),new Vector3(0.0f, -1.0f,  0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, -0.5f),new Vector3(0.0f, -1.0f,  0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f),new Vector3(0.0f, -1.0f,  0.0f)),
                new Vertex(new Vector3(0.5f, -0.5f,  0.5f),new Vector3(0.0f, -1.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f),new Vector3(0.0f, -1.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, -1.0f,  0.0f)),

                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(0.0f,  1.0f,  0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3(0.0f,  1.0f,  0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f,  1.0f,  0.0f)),
                new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(0.0f,  1.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(0.0f,  1.0f,  0.0f)),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(0.0f,  1.0f,  0.0f))
           };
    }
        public static ColoredTexturedVertex[] GetCubeData()
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
