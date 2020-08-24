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
        protected VertexBuffer(int vertexBufferId,BufferTarget target) : this(vertexBufferId,ColoredTexturedVertex.Size)
        {
            Id = vertexBufferId;
            Target = target;
        }
        protected VertexBuffer(){}
        
        public static VertexBuffer CreateVertexObject(ColoredTexturedVertex[] vertices) 
        {
            var bufferId = GL.GenBuffer();
            var bufferDataTypeSize = Marshal.SizeOf(vertices[0]);
            BufferHelper.LoadBufferData(bufferId, vertices, hintUsage: BufferUsageHint.StreamDraw);            
            return new VertexBuffer(bufferId, bufferDataTypeSize, vertices.Length);
        }
        public static VertexBuffer CreateVertexObject(ColoredVertex[] vertices)
        {
            var bufferId = GL.GenBuffer();
            var bufferDataTypeSize = Marshal.SizeOf(vertices[0]);
            BufferHelper.LoadBufferData(bufferId, vertices, hintUsage: BufferUsageHint.StreamDraw);
            return new VertexBuffer(bufferId, bufferDataTypeSize,vertices.Length);
        }
        public static VertexBuffer CreateVertexObject(Vertex[] vertices)
        {
            var bufferId = GL.GenBuffer();
            var bufferDataTypeSize = Marshal.SizeOf(vertices[0]);
            BufferHelper.LoadBufferData(bufferId, vertices, hintUsage: BufferUsageHint.StreamDraw);
            return new VertexBuffer(bufferId, bufferDataTypeSize, vertices.Length);
        }
        //public void Bind()
        //{
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, this.Id);
        //}                    
        //public void LoadData(float[,] vertices)
        //{
        //    var bufferDataTypeSize = Marshal.SizeOf(vertices[0, 0]);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, this.Id);
        //    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * bufferDataTypeSize, vertices, BufferUsageHint.StaticDraw);
        //}

        //public void Dispose()
        //{
        //    GL.DeleteBuffer(this.Id);
        //}
    }
}
