using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GLWrapper
{
    public class VertexArray : IDisposable
    {
        protected readonly List<Texture> _textures = new List<Texture>();        
        public virtual int Id { get; protected set; }
        public virtual VertexBuffer VertexBuffer { get; set; }
        public virtual List<ShaderProgram> Shaders { get; set; } = new List<ShaderProgram>();
        public virtual List<Texture> Textures { get { return _textures; } }
        public virtual ElementBuffer ElementBuffer { get; set; }
        public virtual Light Lamp { get; set; }
        public Camera Camera { get; set; }
        public bool IsBinded { get; protected set; }

        private readonly List<VertexAttribute> _vertexAttributes = new List<VertexAttribute>();        
        protected VertexArray(int vertexArrayId)
        {
            Id = vertexArrayId;
        }
        protected VertexArray(int vertexArrayId, VertexBuffer vertexBuffer) : this(vertexArrayId)
        {            
            VertexBuffer = vertexBuffer;
        }
        protected VertexArray(int vertexArrayId, VertexBuffer vertexBuffer, ShaderProgram shader, Texture texture,ElementBuffer elementBuffer, params VertexAttribute[] vertexAttributes) : this(vertexArrayId,vertexBuffer)
        {            
            Shaders.Add(shader);
            Textures.Add(texture);
            ElementBuffer = elementBuffer;
            _vertexAttributes.AddRange(vertexAttributes);
        }
        public static VertexArray CreateVertexArray()
        {
            var vertexArrayId = OpenGL.GenerateVertexArray();
            return new VertexArray(vertexArrayId);
        }
        public static VertexArray CreateVertexArray(ColoredTexturedVertex[] vertices)
        {
            var vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayId);
            var vertexBuffer = VertexBuffer.CreateVertexBuffer();
            vertexBuffer.LoadData(vertices);
            var vertexArray = new VertexArray(vertexArrayId, vertexBuffer);
            GL.BindVertexArray(0);
            return vertexArray;
        }
        public void Bind()
        {
            OpenGL.BindVertexArray(this.Id);
            var glErrors = LogExtensions.GetGLErrors();
            if (glErrors.Any())
            {
                IsBinded = false;
            }
            else {
                IsBinded = true;
            }
        }
        public void UnBind()
        {
            GL.BindVertexArray(0);
            IsBinded = false;
        }

        public static VertexArray CreateVertexArray(VertexBuffer vertexBuffer,ShaderProgram shader,ElementBuffer elementBuffer)
        {
            var vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayId);
            var vertexArray = new VertexArray(vertexArrayId, vertexBuffer);
            vertexArray.Shaders.Add(shader);
            vertexArray.ElementBuffer = elementBuffer;            
            return vertexArray;
        }
        public static VertexArray CreateVertexArray(ColoredTexturedVertex[] vertices, ShaderProgram shader, ElementBuffer elementBuffer, string textureFilepath, params VertexAttribute[] vertexAttributes)
        {
            var vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayId);
            var vertexBuffer = VertexBuffer.CreateVertexBuffer();
            vertexBuffer.LoadData(vertices);
            var texture = Texture.LoadTexture(textureFilepath);
            var vertexArray = new VertexArray(vertexArrayId, vertexBuffer, shader, texture, elementBuffer, vertexAttributes);
            return vertexArray;
        }                
        
        public void AddTextures(params Texture[] textures)
        {
            _textures.AddRange(textures);
        }        
        
        public void Dispose()
        {
            this.VertexBuffer.Dispose();
            this.Shaders.ForEach(shader => shader.Dispose());
            GL.DeleteVertexArray(this.Id);
        }
    }
}
