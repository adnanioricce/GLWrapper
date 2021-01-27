using GLWrapper.Graphics;
using Moq;
using System;
using Xunit;

namespace GLWrapper.Tests
{
    public class VertexArrayObjectTests
    {
        [Fact]
        public void Should_be_binded_to_draw_otherwise_bind_it()
        {
            // Arrange
            var mockGL = new OpenGLMock();            
            OpenGL.Wrapper = mockGL.Object;
            var vao = VertexArray.CreateVertexArray();
            // Act
            Renderer.Draw(vao, 0, 3);
            // Assert
            Assert.True(vao.IsBinded);
        }
        
    }
}
