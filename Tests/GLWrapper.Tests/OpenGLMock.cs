using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Tests
{
    public class OpenGLMock : Mock<OpenGLWrapper>
    {
        public OpenGLMock()
        {
            Setup(m => m.GenerateVertexArray())
                  .Returns(1);
            Setup(m => m.BindVertexArray(It.IsAny<int>()))
                  .Callback(() => Console.WriteLine("binded"));
            Setup(m => m.GetError())
                  .Returns(OpenTK.Graphics.OpenGL4.ErrorCode.NoError);
            Setup(m => m.DrawArrays(It.IsAny<OpenTK.Graphics.OpenGL4.PrimitiveType>(), It.IsAny<int>(), It.IsAny<int>()))
                  .Callback(() => Console.WriteLine("drawed"));
        }
    }
}
