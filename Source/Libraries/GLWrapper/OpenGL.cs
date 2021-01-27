using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper
{
    public static class OpenGL
    {
        public static OpenGLWrapper Wrapper = new OpenGLWrapper();
        public static int GenerateVertexArray()
        {
            return Wrapper.GenerateVertexArray();
        }
        public static void BindVertexArray(int id)
        {
            Wrapper.BindVertexArray(id);
        }
        public static void BindBuffer(BufferTarget bufferTarget,int id)
        {
            Wrapper.BindBuffer(bufferTarget,id);
        }
        public static ErrorCode GetError()
        {
            return Wrapper.GetError();
        }
        public static void DrawArrays(PrimitiveType type, int first, int count)
        {
            Wrapper.DrawArrays(type, first, count);
        }
        public static void DrawElements(PrimitiveType type, int count, DrawElementsType elementType,int offset)
        {
            Wrapper.DrawElements(type, count, elementType, offset);
        }

    }
    public class OpenGLWrapper
    {
        public virtual int GenerateVertexArray()
        {
            return GL.GenVertexArray();
        }
        public virtual void BindVertexArray(int id)
        {
            GL.BindVertexArray(id);
        }
        public virtual void BindBuffer(BufferTarget bufferTarget,int id)
        {
            GL.BindBuffer(bufferTarget,id);
        }
        public virtual ErrorCode GetError()
        {
            return GL.GetError();
        }
        public virtual void DrawArrays(PrimitiveType type, int first, int count)
        {
            GL.DrawArrays(type, first, count);
        }
        public virtual void DrawElements(PrimitiveType type,int count,DrawElementsType elementType,int offset)
        {
            GL.DrawElements(type,count,elementType,offset);
        }
    }

}
