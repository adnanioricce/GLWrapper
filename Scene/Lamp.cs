using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GLWrapper
{
    public class Lamp
    {
        public int Id { get; protected set; }
        public Vector3 Position { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public ShaderProgram Shader { get; set; }
        protected Lamp(int id)
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
        public static Lamp CreateLamp()
        {
            var lampId = GL.GenVertexArray();
            var lamp = new Lamp(lampId);
            return lamp;
        }
    }
}
