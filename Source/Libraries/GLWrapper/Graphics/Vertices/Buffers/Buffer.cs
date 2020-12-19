using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Graphics.Vertices.Buffers
{
    public class Buffer : IBuffer
    {        
        protected bool _isBinded = false;
        protected BufferUsageHint _usageHint = BufferUsageHint.StaticDraw;
        public int Id { get; protected set; }
        public OpenTK.Graphics.OpenGL4.BufferTarget Target { get; protected set; }
        public int VerticesCount { get; set; }
        public bool IsBinded { get { return _isBinded; }}

        public BufferUsageHint UsageHint => _usageHint;

        public virtual void Bind()
        {
            GL.BindBuffer(this.Target, this.Id);
            _isBinded = true;
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
            BufferHelper.LoadBufferData(this.Id, vertices, hintUsage: OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);
            VerticesCount = vertices.Length;
        }
        
    }
}
