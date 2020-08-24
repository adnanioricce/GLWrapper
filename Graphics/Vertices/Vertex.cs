using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Graphics.Vertices
{
    /// <summary>
    /// Vertex holding only a Position Vector. Has size of 12 bytes
    /// </summary>
    public struct Vertex
    {
        public const int Size = 3 * 4;
        public const int PositionStride = 3 * sizeof(float);
        public Vector3 Position;
        public Vertex(Vector3 position)
        {
            Position = position;            
        }
        public Vertex(ColoredVertex vertex)
        {
            Position = vertex.Position;
        }
        public Vertex(ColoredTexturedVertex vertex)
        {
            Position = vertex.Position;            
        }
    }
}
