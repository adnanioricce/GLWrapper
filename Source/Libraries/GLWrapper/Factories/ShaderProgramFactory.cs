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
            var vertexShader = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Shaders.Default2D.vertex.shader");
            var fragmentShader = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Shaders.Default2D.fragment.shader");
            var shaderProgram = ShaderProgram.CreateShaderProgram(Shader.CreateShader(vertexShader, ShaderType.VertexShader), Shader.CreateShader(fragmentShader, ShaderType.FragmentShader));
            shaderProgram.Use();
            shaderProgram.SetVertexAttributes(new VertexAttribute("aPosition", 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, ColoredVertex.Size, 0),
                                              new VertexAttribute("aColor", 4, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, ColoredVertex.Size, ColoredVertex.PositionStride));
            return shaderProgram;
        }
    }
}