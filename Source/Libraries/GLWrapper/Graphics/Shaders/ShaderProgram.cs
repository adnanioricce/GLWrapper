using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GLWrapper
{
    public class ShaderProgram : IDisposable
    {
        private int _handle;
        private bool _disposedValue = false;
        private bool _inUse = false;
        public int Id { get { return _handle; } }
        public bool InUse { get { return _inUse; } }
        protected readonly List<Shader> _shaders = new List<Shader>();
        protected readonly List<Texture> _textures = new List<Texture>();
        private readonly List<(int,TextureUnit)> _textureSlots = new List<(int, TextureUnit)>();
        private readonly List<VertexAttribute> _vertexAttributes = new List<VertexAttribute>();
        public List<Texture> Textures { get { return _textures; } }
        protected ShaderProgram()
        {
            _textureSlots.AddRange(_textures.Select(texture => (texture.Id, TextureUnit.Texture0 + _textures.IndexOf(texture))));
            _handle = GL.CreateProgram();            
        }
        public ShaderProgram(int handle,IEnumerable<Shader> shaders)
        {
            _textureSlots.AddRange(_textures.Select(texture => (texture.Id, TextureUnit.Texture0 + _textures.IndexOf(texture))));
            _handle = handle;
            _shaders.AddRange(shaders);
        }
        public ShaderProgram(int handle,IEnumerable<Shader> shaders,IEnumerable<VertexAttribute> vertexAttributes)
        {
            _textureSlots.AddRange(_textures.Select(texture => (texture.Id, TextureUnit.Texture0 + _textures.IndexOf(texture))));
            _handle = handle;
            _shaders.AddRange(shaders);
            _vertexAttributes.AddRange(vertexAttributes);
        }
        ~ShaderProgram()
        {
            GL.DeleteProgram(_handle);
        }
        protected virtual bool Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                foreach (var shader in _shaders)
                {                
                    GL.DetachShader(Id, shader.Id);
                    var infoLog = GL.GetShaderInfoLog(shader.Id);
                    var programInfoLog = GL.GetProgramInfoLog(Id);
                    LogExtensions.LogGLError(nameof(ShaderProgram), nameof(CreateShaderProgram), string.Format("Shader Info Log: {0}", infoLog));
                    LogExtensions.LogGLError(nameof(ShaderProgram), nameof(CreateShaderProgram), string.Format("Program Info Log: {0}", programInfoLog));
                    
                }
                GL.DeleteProgram(_handle);

                _disposedValue = true;
            }
            return false;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Link()
        {
            CheckIfInUse();
            GL.LinkProgram(_handle);
        }
        public void Use()
        {            
            GL.UseProgram(_handle);
            _inUse = true;            
        }        
        public void BindTextures()
        {
            CheckIfInUse();
            _textures?.ForEach(texture => texture.Bind(_textures.IndexOf(texture)));
        }
        public void UnbindTexture()
        {
            CheckIfInUse();
            _textures.ForEach(texture => texture.UnBind());
        }
        public int GetAttribPointer(string variableName)
        {
            CheckIfInUse();
            return GL.GetAttribLocation(_handle, variableName);
        }
        public int GetUniformLocation(string variableName)
        {
            CheckIfInUse();
            return GL.GetUniformLocation(_handle, variableName);
        }

        public int GetUniform(string variableName)
        {
            CheckIfInUse();
            GL.GetUniform(this.Id, GetUniformLocation(variableName), out int result);
            return result;
        }

        public void SetFloat(string variableName, float time)
        {
            CheckIfInUse();
            GL.Uniform1(GetUniformLocation(variableName), time);
        }

        public void SetInt(string variableName, int value)
        {
            CheckIfInUse();
            var location = GetUniformLocation(variableName);
            GL.Uniform1(location, value);
        }
        public void SetMatrix4(string variableName, Matrix4 value)
        {
            CheckIfInUse();
            GL.UniformMatrix4(GetUniformLocation(variableName), false,ref value);
        }
        public void SetVector3(string variableName, Vector3 value)
        {            
            CheckIfInUse();
            GL.Uniform3(GetUniformLocation(variableName),value);
        }
        public void SetProjection(Camera camera)
        {            
            CheckIfInUse();
            this.SetMatrix4(nameof(camera.Model).ToLower(), camera.Model);
            this.SetMatrix4(nameof(camera.View).ToLower(), camera.View);
            this.SetMatrix4(nameof(camera.Projection).ToLower(), camera.Projection);
        }
        public void SetVertexAttributes()
        {                  
            CheckIfInUse();      
            foreach(var attribute in _vertexAttributes) {
                attribute.Set(this);
            }
        }
        public static ShaderProgram CreateShaderProgram(string vertexShaderPath,string fragmentShaderPath,IEnumerable<VertexAttribute> attributes)
        {
            var vertexShader = Shader.CreateShader(vertexShaderPath, ShaderType.VertexShader);
            LogExtensions.LogShaderInfo(vertexShader.Id);
            var fragmentShader = Shader.CreateShader(fragmentShaderPath, ShaderType.FragmentShader);
            LogExtensions.LogShaderInfo(fragmentShader.Id);
            return ShaderProgram.CreateShaderProgram(attributes,vertexShader, fragmentShader);
        }
        public static ShaderProgram CreateShaderProgram(IEnumerable<VertexAttribute> attributes, params Shader[] shaders)
        {
            var programId = GL.CreateProgram();
            foreach(var shader in shaders) { 
                GL.AttachShader(programId, shader.Id);                
            }
            GL.LinkProgram(programId);
            var program = new ShaderProgram(programId, shaders,attributes);
            return program;
        }
        protected void CheckIfInUse(){
            if(!_inUse){
                //Should I throw a exception or try to call Use()? If the second, why create a public method ?
                throw new InvalidOperationException("Shader invalid operation: can't complete current method, because current shader is not in use. Call Use() before calling this method");
            }
        }
    }
}
