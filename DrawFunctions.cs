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
    public delegate void DrawFunction(VertexArray vertexArray, int verticesCount);
    public static class DrawFunctions
    {
        private static Vector3[] CubePositions = Program.CubePositions();
        public static void DrawCubeWithLightning(VertexArray vertexArray,int verticesCount)
        {
            var lamp = vertexArray.Lamp;
            GL.BindVertexArray(vertexArray.Id);
            vertexArray.Shader[0].Use();
            vertexArray.Shader[0].SetProjection(Ioc.Camera);
            vertexArray.Shader[0].SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            vertexArray.Shader[0].SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
            vertexArray.Shader[0].SetVector3("lightPos", lamp.Position);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);            
            GL.BindVertexArray(lamp.Id);            
            lamp.Shader.Use();
            Matrix4 lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f);
            lampMatrix *= Matrix4.CreateTranslation(lamp.Position);
            lamp.Shader.SetMatrix4("model", lampMatrix);
            lamp.Shader.SetMatrix4("view", Ioc.Camera.View);
            lamp.Shader.SetMatrix4("projection", Ioc.Camera.Projection);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
        public static void DrawCube(VertexArray vertexArray,int verticesCount)
        {
            vertexArray.Shader.ForEach(shader => shader.BindTextures());
            GL.BindVertexArray(vertexArray.Id);
            vertexArray.ElementBuffer.Bind();
            vertexArray.Shader.ForEach(shader => shader.SetProjection(Ioc.Camera));                        
            for (int i = 0; i < CubePositions.Length; ++i)
            {                
                float angle = 20.0f * i;
                var model = Ioc.Camera.Model * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));                
                model = model * Matrix4.CreateScale(new Vector3(0.5f,0.5f,0.5f)) * Matrix4.CreateTranslation(CubePositions[i]);
                vertexArray.Shader.ForEach(shader => shader.SetMatrix4(nameof(Ioc.Camera.Model).ToLower(), model));
                GL.DrawArrays(PrimitiveType.Triangles, 0, verticesCount);
            }
            vertexArray.ElementBuffer.UnBind();
            vertexArray.Shader.ForEach(shader => shader.UnbindTexture());
            vertexArray.UnBind();
            Logger.Log(nameof(DrawFunctions), nameof(DrawCube));            
        }
        public static void DrawElements(VertexArray vertexArray, int verticesCount)
        {
            GL.BindVertexArray(vertexArray.Id);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
