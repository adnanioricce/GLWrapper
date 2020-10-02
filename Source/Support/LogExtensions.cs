using OpenTK.Graphics.OpenGL4;
using Serilog;
using System;

namespace GLWrapper
{
    public static class LogExtensions
    {
        public static void MessageCallBack(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {                                    
            LogError("GL CALLBACK:{0} type: 0x{1}0, severity = 0x{2}x, message = {3}",source,type,severity,message);            
        }
        public static void LogGLError()
        {
            var error = GL.GetError();
            while (error != ErrorCode.NoError)
            {
                LogError("ERROR CODE:{code}",error);
                error = GL.GetError();
            }
        }
        public static void LogError(string template,params object[] args)
        {
            Log.Error(messageTemplate: template, args);
        }
        public static void LogGLError(string className,string method)
        {
            var error = GL.GetError();            
            while(error != ErrorCode.NoError) {                
                LogError("error on {0} class on method {1}, error code:{2}", className, method, error);
                error = GL.GetError();
            }
        }
        public static void LogGLError(string template, string className,string method,string message)
        {
            var error = GL.GetError();
            while (error != ErrorCode.NoError)
            {
                LogError(template, className, method, error, message);
                error = GL.GetError();
            }
        }
        public static void LogGLError(string className,string method,string message)
        {
            LogGLError("error on {0} class on method {1}, error code:{2} \n {3}", className, method, message);
        }
        public static void LogShaderInfo(int shaderId)
        {            
            var log = GL.GetShaderInfoLog(shaderId);
            if (!string.IsNullOrEmpty(log))
            {
                LogError(log);
            }   
        }
    }
}
