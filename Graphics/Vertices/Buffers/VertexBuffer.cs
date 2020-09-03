using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GLWrapper.Graphics.Vertices;
namespace GLWrapper
{
    public class VertexBuffer : GLWrapper.Graphics.Vertices.Buffers.Buffer
    {
        protected readonly int _bufferDataTypeSize = 0;                
        public virtual int VerticesCount { get; protected set; }        
        protected VertexBuffer(int vertexBufferId, int bufferDataTypeSize,int verticesCount) : this(vertexBufferId, bufferDataTypeSize)
        {
            VerticesCount = verticesCount;
        }
        protected VertexBuffer(int vertexBufferId, int bufferDataTypeSize)
        {
            Id = vertexBufferId;
            _bufferDataTypeSize = bufferDataTypeSize;
        }
        protected VertexBuffer(int vertexBufferId)
        {
            Id = vertexBufferId;
        }
        protected VertexBuffer(){}
        public static VertexBuffer CreateVertexBuffer()
        {
            var bufferId = GL.GenBuffer();
            //var bufferDataTypeSize = Marshal.SizeOf(vertices[0]);
            //BufferHelper.LoadBufferData(bufferId, vertices, hintUsage: BufferUsageHint.StreamDraw);
            return new VertexBuffer(bufferId);
        }        
    }
}
