using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Linq;
using GLWrapper.Graphics.Vertices;
using GLWrapper.Scene;

namespace GLWrapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            Ioc.ScreenSize = (1280, 720);
            Ioc.Camera = Camera.CreateCamera(Ioc.ScreenSize.Width, Ioc.ScreenSize.Height);
            using var game = new GameWindow(Ioc.ScreenSize.Width,Ioc.ScreenSize.Height,"TkCube");
            //DrawCubeWithLightning(game);            
            //DrawPoint(game);
            DrawWithModel(game);
            game.Run(60.0);
        }        
        public static void DrawWithModel(GameWindow game)
        {
            var model = Model.CreateModel(GetCubeData().Select(v => new ColoredVertex(v)).ToArray());
            var shader = ShaderProgram.CreateShaderProgram("./Assets/Shaders/basicVertex.shader", "./Assets/Shaders/basicFrag.shader");
            model.SetShader(shader, new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredVertex.Size, 0),
                                    new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredVertex.Size, ColoredVertex.PositionStride));
            model.OnDraw = (vbo, shader) =>
            {

                //GL.BindVertexArray(vertexArray.Id);
                //vertexArray.Shader[0].Use();
                //vertexArray.Shader[0].SetProjection(Ioc.Camera);
                //vertexArray.Shader[0].SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
                //vertexArray.Shader[0].SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
                //vertexArray.Shader[0].SetVector3("lightPos", lamp.Position);
                vbo.Bind();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vbo.VerticesCount);
                //GL.BindVertexArray(lamp.Id);
                //lamp.Shader.Use();
                //Matrix4 lampMatrix = Matrix4.Identity;
                //lampMatrix *= Matrix4.CreateScale(0.2f);
                //lampMatrix *= Matrix4.CreateTranslation(lamp.Position);
                //lamp.Shader.SetMatrix4("model", lampMatrix);
                //lamp.Shader.SetMatrix4("view", Ioc.Camera.View);
                //lamp.Shader.SetMatrix4("projection", Ioc.Camera.Projection);
                //GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            };
            game.AddModel(model);            
        }
        public static void DrawPoint(GameWindow game)
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
            vertexArray.Shader.Add(shaderProgram);
            vertexArray.VertexBuffer = vbo;
            game.AddVertexArrays(vertexArray);
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
            vaoModel.Shader.AddRange(new[] { lightningShader });
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
