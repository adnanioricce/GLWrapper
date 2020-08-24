using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GLWrapper
{
    public class Shader : IDisposable
    {
        private bool _disposed = false;
        public int Id { get; protected set; }        
        public ShaderType Type { get; protected set; }
        protected Shader(int shaderId, ShaderType type)
        {
            Id = shaderId;            
            Type = type;
        }
        public static Shader CreateShader(string shaderFilepath,ShaderType type)
        {
            var shaderId = GL.CreateShader(type);
            var shaderSourceCode = LoadShaderCode(shaderFilepath);
            GL.ShaderSource(shaderId, shaderSourceCode);
            GL.CompileShader(shaderId);
            Logger.Log(nameof(Shader), nameof(CreateShader));
            LogShaderInfo(shaderId);
            return new Shader(shaderId, type);
        }
        public static string LoadShaderCode(string filepath)
        {
            using var reader = new StreamReader(filepath, Encoding.UTF8);
            return reader.ReadToEnd();            
        }
        protected static void LogShaderInfo(int shaderId)
        {
            var infoLog = GL.GetShaderInfoLog(shaderId);
            if (string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine("No error detected on shader {0}", shaderId);
                return;
            }
            Console.WriteLine(infoLog);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                GL.DeleteShader(this.Id);                
                _disposed = true;
                Logger.Log(nameof(Shader), nameof(Dispose));
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Shader()
        {
            GL.DeleteShader(this.Id);            
            LogShaderInfo(this.Id);
        }
    }
}
