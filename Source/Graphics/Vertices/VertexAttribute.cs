using OpenTK.Graphics.OpenGL4;

namespace GLWrapper
{
    public class VertexAttribute
    {
        private readonly string _name;
        private readonly int _size;
        private readonly VertexAttribPointerType _type;        
        private readonly int _stride;
        private readonly int _offset;
        private readonly bool _normalize;
        public string Name { get { return _name; } }
        public VertexAttribute(string name,int size, VertexAttribPointerType type,int stride,int offset,bool normalize = false)
        {
            _name = name;
            _size = size;
            _type = type;
            _stride = stride;
            _offset = offset;
            _normalize = normalize;
        }        
        public void Set(ShaderProgram shader)
        {
            var index = shader.GetAttribPointer(_name);
            GL.VertexAttribPointer(index, _size, _type, _normalize, _stride, _offset);
            GL.EnableVertexAttribArray(index);            
        }
    }
}
