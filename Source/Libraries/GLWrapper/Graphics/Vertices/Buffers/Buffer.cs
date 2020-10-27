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
        public int VerticesCount { get; set; }
        public virtual void Bind()
        {
            GL.BindBuffer(this.Target, this.Id);
        }        
        public void Dispose()
        {
            GL.DeleteBuffer(this.Id);
        }
        /// <summary>
        /// Uploads the given vertices to the GPU binded with the current buffer object
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="vertices"></param>
        public void LoadData<TVertex>(TVertex[] vertices) where TVertex : struct
        {            
            BufferHelper.LoadBufferData(this.Id, vertices, hintUsage: BufferUsageHint.StaticDraw);
            VerticesCount = vertices.Length;
        }        
    }
}
