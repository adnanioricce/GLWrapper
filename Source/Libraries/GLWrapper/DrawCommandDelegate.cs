using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Threading.Tasks;

namespace GLWrapper
{
    /// <summary>
    /// Delegate for a GL draw function. May be a vertex array Id or a vertex object Id
    /// </summary>
    /// <param name="vertexObjectId">The vertex object or vertex array Id</param>
    public delegate void DrawVAOCommand(VertexArray vertexArray, int verticesCount);
    public delegate void DrawVBOCommand(VertexBuffer vbo, ShaderProgram shader);   
}
