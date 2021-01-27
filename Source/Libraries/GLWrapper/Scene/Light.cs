using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace GLWrapper
{
    public class Light
    {
        public int Id { get; protected set; }
        public Vector3 Position { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public ShaderProgram Shader { get; set; }
        protected Light(int id)
        {
            Id = id;
        }
        public void Bind()
        {
            GL.BindVertexArray(this.Id);
        }
        public void UnBind()
        {
            GL.BindVertexArray(0);
        }        
        public static Light CreateLight()
        {
            var lampId = GL.GenVertexArray();
            var lamp = new Light(lampId);
            return lamp;
        }
    }
}
