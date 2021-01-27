using GLWrapper.Graphics.Vertices.Buffers;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GLWrapper
{
    public class ElementBuffer : IBuffer
    {
        private BufferUsageHint _usageHint = BufferUsageHint.StaticDraw;
        private bool _isBinded = false;
        public int Id { get; protected set; }
        public int IndicesCount { get; protected set; }

        public bool IsBinded { get {return _isBinded;} }

        public BufferUsageHint UsageHint { get { return _usageHint; } }

        protected ElementBuffer(int id,int indicesCount)
        {
            Id = id;
            IndicesCount = indicesCount;
        }
        protected ElementBuffer(int id){
            Id = id;
        }
        public static ElementBuffer CreateElementBuffer()
        {                        
            var elementBufferId = GL.GenBuffer();                        
            return new ElementBuffer(elementBufferId);
        }
        public static ElementBuffer CreateElementBuffer(int[] indices)
        {                        
            var elementBufferId = GL.GenBuffer();            
            BufferHelper.LoadBufferData(elementBufferId, indices, BufferTarget.ElementArrayBuffer);
            return new ElementBuffer(elementBufferId, indices.Length);
        }        
        public void Bind()
        {            
            OpenGL.BindBuffer(BufferTarget.ElementArrayBuffer,this.Id);
            _isBinded = true;
        }
        public void UnBind()
        {
            OpenGL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            _isBinded = false;
        }

        public void LoadData<TVertex>(TVertex[] vertices) where TVertex : struct
        {
            BufferHelper.LoadBufferData(Id,vertices,BufferTarget.ElementArrayBuffer);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}
