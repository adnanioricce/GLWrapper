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
        protected VertexBuffer(int vertexBufferId, int bufferDataTypeSize,int verticesCount) : this(vertexBufferId, bufferDataTypeSize)
        {
            VerticesCount = verticesCount;
        }
        protected VertexBuffer(int vertexBufferId, int bufferDataTypeSize) : this(vertexBufferId)
        {            
            _bufferDataTypeSize = bufferDataTypeSize;
        }
        protected VertexBuffer(int vertexBufferId,BufferTarget target) : this(vertexBufferId)
        {
            Target = target;
        }
        protected VertexBuffer(int vertexBufferId)
        {
            Id = vertexBufferId;
        }
        
        protected VertexBuffer(){}
        public static VertexBuffer CreateVertexBuffer()
        {
            var bufferId = GL.GenBuffer();            
            return new VertexBuffer(bufferId,BufferTarget.ArrayBuffer);
        }        
    }
}
