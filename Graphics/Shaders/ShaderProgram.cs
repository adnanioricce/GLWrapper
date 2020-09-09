using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace GLWrapper
{
    public class ShaderProgram : IDisposable
    {
        private int _handle;
        private bool _disposedValue = false;
        public int Id { get { return _handle; } }
        protected readonly List<Texture> _textures = new List<Texture>();
        private readonly List<(int,TextureUnit)> _textureSlots = new List<(int, TextureUnit)>();
        public List<Texture> Textures { get { return _textures; } }
        protected ShaderProgram()
        {
            _textureSlots.AddRange(_textures.Select(texture => (texture.Id, TextureUnit.Texture0 + _textures.IndexOf(texture))));
            _handle = GL.CreateProgram();            
        }
        public ShaderProgram(int handle)
        {
            _textureSlots.AddRange(_textures.Select(texture => (texture.Id, TextureUnit.Texture0 + _textures.IndexOf(texture))));
            _handle = handle;
        }
        ~ShaderProgram()
        {
            GL.DeleteProgram(_handle);
        }
        protected virtual bool Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
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
            GL.LinkProgram(_handle);
        }
        public void Use()
        {
            GL.UseProgram(_handle);
        }        
        public void BindTextures()
        {
            _textures?.ForEach(texture => texture.Bind(_textures.IndexOf(texture)));
        }
        public void UnbindTexture()
        {
            _textures.ForEach(texture => texture.UnBind());
        }
        public int GetAttribPointer(string variableName)
        {
            return GL.GetAttribLocation(_handle, variableName);
        }
        public int GetUniformLocation(string variableName)
        {
            return GL.GetUniformLocation(_handle, variableName);
        }

        public int GetUniform(string variableName)
        {
            GL.GetUniform(this.Id, GetUniformLocation(variableName), out int result);
            return result;
        }

        public void SetInt(string variableName, int value)
        {
            var location = GetUniformLocation(variableName);
            GL.Uniform1(location, value);
        }
        public void SetMatrix4(string variableName, Matrix4 value)
        {
            GL.UniformMatrix4(GetUniformLocation(variableName), false,ref value);
        }
        public void SetVector3(string variableName, Vector3 value)
        {
            GL.Uniform3(GetUniformLocation(variableName),value);
        }
        public void SetProjection(Camera camera)
        {            
            this.SetMatrix4(nameof(camera.Model).ToLower(), camera.Model);
            this.SetMatrix4(nameof(camera.View).ToLower(), camera.View);
            this.SetMatrix4(nameof(camera.Projection).ToLower(), camera.Projection);
        }
        public void SetVertexAttributes(params VertexAttribute[] attributes)
        {
            foreach(var attribute in attributes)
            {
                attribute.Set(this);
            }
        }
        public static ShaderProgram CreateShaderProgram(string vertexShaderPath,string fragmentShaderPath)
        {
            using var vertexShader = Shader.CreateShader(vertexShaderPath, ShaderType.VertexShader);            
            using var fragmentShader = Shader.CreateShader(fragmentShaderPath, ShaderType.FragmentShader);                
            return ShaderProgram.CreateShaderProgram(vertexShader.Id, fragmentShader.Id);            
        }
        public static ShaderProgram CreateShaderProgram(params int[] shaderIds)
        {
            var programId = GL.CreateProgram();
            for (int i = 0; i < shaderIds.Length; ++i)
            {
                GL.AttachShader(programId, shaderIds[i]);                
            }
            GL.LinkProgram(programId);
            for (int i = 0; i < shaderIds.Length; ++i)
            {                
                GL.DetachShader(programId, shaderIds[i]);
                var infoLog = GL.GetShaderInfoLog(shaderIds[i]);
                var programInfoLog = GL.GetProgramInfoLog(programId);
                Logger.Log(nameof(ShaderProgram), nameof(CreateShaderProgram), string.Format("Shader Info Log: {0}", infoLog));
                Logger.Log(nameof(ShaderProgram), nameof(CreateShaderProgram), string.Format("Program Info Log: {0}", programInfoLog));
            }            
            return new ShaderProgram(programId);
        }               
    }
}
