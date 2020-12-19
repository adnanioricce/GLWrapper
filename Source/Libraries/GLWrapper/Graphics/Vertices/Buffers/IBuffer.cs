using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Graphics.Vertices.Buffers
{
    internal interface IBuffer : IDisposable
    {
        public bool IsBinded { get; }
        public BufferUsageHint UsageHint { get; }
        void Bind();
        void LoadData<TVertex>(TVertex[] vertices) where TVertex : struct;

    }
}
