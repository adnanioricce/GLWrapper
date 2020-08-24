using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Graphics.Vertices.Buffers
{
    public class Buffer : IBuffer
    {
        public int Id { get; protected set; }
        public BufferTarget Target { get; protected set; }
        public void Bind()
        {
            GL.BindBuffer(this.Target, this.Id);
        }        
        public void Dispose()
        {
            GL.DeleteBuffer(this.Id);
        }
        public void LoadData<TVertex>(TVertex[] vertices) where TVertex : struct
        {
            this.Bind();
            BufferHelper.LoadBufferData(this.Id, vertices, hintUsage: BufferUsageHint.StreamDraw);
        }        
    }
}
