using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using GLWrapper.Graphics.Vertices;

namespace GLWrapper
{
    public static class BufferHelper
    {
        public static void LoadBufferData<TVertex>(int bufferId,TVertex[] vertices, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint hintUsage = BufferUsageHint.StaticDraw) where TVertex : struct
        {
            var bufferDataTypeSize = Marshal.SizeOf(vertices[0]);
            var verticesLength = (vertices.Length * bufferDataTypeSize) / bufferDataTypeSize;
            GL.BindBuffer(target, bufferId);
            GL.BufferData(target, verticesLength * bufferDataTypeSize, vertices, hintUsage);
        }
        public static void LoadBufferData<TVertex>(GLWrapper.Graphics.Vertices.Buffers.Buffer buffer, TVertex[] vertices, BufferUsageHint hintUsage = BufferUsageHint.StaticDraw) where TVertex : struct
        {
            LoadBufferData<TVertex>(buffer.Id, vertices, buffer.Target, buffer.UsageHint);
        }
    }
}
