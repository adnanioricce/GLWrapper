using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GLWrapper
{
    public class ElementBuffer
    {
        public int Id { get; protected set; }
        public int IndicesCount { get; protected set; }
        protected ElementBuffer(int id,int indicesCount)
        {
            Id = id;
            IndicesCount = indicesCount;
        }
        public static ElementBuffer CreateElementBuffer(uint[] indices)
        {                        
            var elementBufferId = GL.GenBuffer();            
            BufferHelper.LoadBufferData(elementBufferId, indices, BufferTarget.ElementArrayBuffer);
            return new ElementBuffer(elementBufferId, indices.Length);
        }
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,this.Id);
        }
        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
