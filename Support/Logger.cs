using OpenTK.Graphics.OpenGL4;
using System;

namespace GLWrapper
{
    public static class Logger
    {
        public static void MessageCallBack(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            Console.WriteLine("GL CALLBACK:{0} type: 0x{1}0, severity = 0x{2}x, message = {3}",source,type,severity,message);
        }
        public static void Log(string className,string method)
        {
            var error = GL.GetError();
            while(error != ErrorCode.NoError) {
                Console.WriteLine("error on {0} class on method {1}, error code:{2}", className, method, error);
                error = GL.GetError();
            }
        }
        public static void Log(string className,string method,string message)
        {
            var error = GL.GetError();
            while (error != ErrorCode.NoError)
            {
                Console.WriteLine("error on {0} class on method {1}, error code:{2} \n {3}", className, method, error,message);
                error = GL.GetError();
            }
        }
    }
}
