using GLWrapper.Scene;
using OpenTK.Graphics.OpenGL4;
using System;

namespace GLWrapper.Graphics
{
    public class Renderer
    {
        public void DrawElements(VertexArray VAO,int count,int offset = 0)
        {
            if(!VAO.IsBinded){
                VAO.Bind();
            }

            OpenGL.DrawElements(PrimitiveType.Triangles,count,DrawElementsType.UnsignedInt,offset);
        }
        public void Draw(VertexArray VAO,int first,int count)
        {
            if (!VAO.IsBinded)
            {
                VAO.Bind();
            }
            OpenGL.DrawArrays(PrimitiveType.Triangles, first, count);
        }
        
        public void Draw(Model model,float time)
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
            if(model.EBO is null){
                GL.DrawArrays(PrimitiveType.Triangles, 0, model.VBO.VerticesCount);
                return;
            }
            GL.DrawElements(PrimitiveType.Triangles,model.EBO.IndicesCount,DrawElementsType.UnsignedInt,0);
        }
        public void Draw(Model model,Camera camera,float time){
            model.VAO.Bind();            
            model.ShaderProgram.Use();
            model.ShaderProgram.SetProjection(camera);
            GL.DrawElements(PrimitiveType.Triangles,model.EBO.IndicesCount,DrawElementsType.UnsignedInt,0);
        }
    }
}
