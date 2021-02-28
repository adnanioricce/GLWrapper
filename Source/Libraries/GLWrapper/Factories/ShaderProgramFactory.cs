using GLWrapper.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GLWrapper.Factories
{
    public class ShaderProgramFactory
    {
        public static ShaderProgram CreateDefault2DShaderProgram()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var vertexShaderStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Shaders.Default2D.vertex.shader");
            var fragmentShaderStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Shaders.Default2D.fragment.shader");
            var attributes = new [] {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, sizeof(float) * (3 + 4), 0),
                new VertexAttribute("aColor",4, VertexAttribPointerType.Float, sizeof(float) * (3 + 4),sizeof(float) * 3)
            };
            var vertexShader = Shader.CreateShader(vertexShaderStream, ShaderType.VertexShader);
            var fragmentShader = Shader.CreateShader(fragmentShaderStream, ShaderType.FragmentShader);
            var shaderProgram = ShaderProgram.CreateShaderProgram(attributes, vertexShader, fragmentShader);
            shaderProgram.Use();
            shaderProgram.SetVertexAttributes();
            return shaderProgram;
        }
        public static ShaderProgram CreateDefault2DShaderProgramWithTexture()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var vertexShaderStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Shaders.Default2DTexture.vertex.shader");
            var fragmentShaderStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Shaders.Default2DTexture.fragment.shader");
            var attributes = new[] {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, 0),            
                new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 3),
                new VertexAttribute("aTexCoord", 2, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 7),
            };
            var vertexShader = Shader.CreateShader(vertexShaderStream, ShaderType.VertexShader);
            var fragmentShader = Shader.CreateShader(fragmentShaderStream, ShaderType.FragmentShader);
            var shaderProgram = ShaderProgram.CreateShaderProgram(attributes, vertexShader, fragmentShader);
            shaderProgram.Use();
            shaderProgram.SetVertexAttributes();
            return shaderProgram;
        }
        public static ShaderProgram CreateDefault3DShaderProgramWithTexture()
        {
            //TODO: Move Model-View-Projection concerns to it's own Shader, with it's Own buffers(Uniform Buffers)
            var shaders = GetShadersFromResource("Default3DTexture",new []{ ShaderType.VertexShader,ShaderType.FragmentShader});
            var attributes = new[] {
                new VertexAttribute("aPosition", 3, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, 0),            
                new VertexAttribute("aColor", 4, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 3),
                new VertexAttribute("aTexCoord", 2, VertexAttribPointerType.Float, ColoredTexturedVertex.Size, sizeof(float) * 7),
            };
            var shaderProgram = ShaderProgram.CreateShaderProgram(attributes,shaders[ShaderType.VertexShader],shaders[ShaderType.FragmentShader]);
            shaderProgram.Use();
            shaderProgram.SetVertexAttributes();
            return shaderProgram;
        }
        private static Dictionary<ShaderType,Shader> GetShadersFromResource(string resourceName,ShaderType[] types)
        {
            if(types.Any(t => !(t == ShaderType.VertexShader || t == ShaderType.FragmentShader))){
                throw new System.ArgumentException($"only Fragment and Vertex shader it's supported");
            }            
            var shaders = new Dictionary<ShaderType,Shader>();
            var assembly = Assembly.GetExecutingAssembly();            
            foreach(var type in types) {
                
                var shader = LoadShaderStream(assembly,type);                
                shaders[type] = shader;
            }
            return shaders;
            Shader LoadShaderStream(Assembly assembly,ShaderType shaderType)
            {
                var shaderTypeName = shaderType.ToString()
                                               .Substring(0, shaderType.ToString()
                                                                       .IndexOf("Shader"))
                                               .ToLower();
                var assemblyName = assembly.GetName().Name;
                var shaderStream = assembly.GetManifestResourceStream($"{assemblyName}.Resources.Shaders.{resourceName}.{shaderTypeName}.shader");
                return Shader.CreateShader(shaderStream,shaderType);
            }
        }        
    }
}