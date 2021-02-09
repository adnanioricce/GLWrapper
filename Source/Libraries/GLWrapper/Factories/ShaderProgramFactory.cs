using GLWrapper.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
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
    }
}