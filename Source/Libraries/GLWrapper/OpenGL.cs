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
        public static ErrorCode GetError()
        {
            return Wrapper.GetError();
        }
        public static void DrawArrays(PrimitiveType type, int first, int count)
        {
            Wrapper.DrawArrays(type, first, count);
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
        public virtual ErrorCode GetError()
        {
            return GL.GetError();
        }
        public virtual void DrawArrays(PrimitiveType type, int first, int count)
        {
            GL.DrawArrays(type, first, count);
        }
    }

}
