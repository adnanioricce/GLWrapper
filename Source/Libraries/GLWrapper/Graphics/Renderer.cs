using GLWrapper.Scene;
using OpenTK.Graphics.OpenGL4;
using System;

namespace GLWrapper.Graphics
{
    public static class Renderer
    {
        public static void Draw(VertexArray VAO,int first,int count,float time)
        {
            if (!VAO.IsBinded)
            {
                VAO.Bind();
            }
            OpenGL.DrawArrays(PrimitiveType.Triangles, first, count);
        }

        public static void Draw(Model model,float time)
        {
            if (!(model.DrawCommand is null))
            {
                model.Draw(time);
                return;
            }
            if (!model.VAO.IsBinded)
            {
                model.VAO.Bind();                
            }
            model.ShaderProgram.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.VBO.VerticesCount);
        }
        public static void DrawCanvas(float time)
        {
            Canvas.Render(time);
        }
    }
}
