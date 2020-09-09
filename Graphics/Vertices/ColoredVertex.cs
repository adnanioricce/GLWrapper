using OpenTK;
using OpenTK.Graphics;

namespace GLWrapper.Graphics.Vertices
{
    /// <summary>
    /// Vertex with position and color. Has size of 28 bytes
    /// </summary>
    public struct ColoredVertex
    {
        public const int Size = (3 + 4) * 4;
        public const int PositionStride = 3 * sizeof(float);
        public const int ColorStride = 4 * sizeof(float);        
        public Vector3 Position;
        public Color4 Color;
        public ColoredVertex()
        {
            Position = Vector3.Zero;
            Color = Color4.White;
        }
        public ColoredVertex(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;            
        }
        public ColoredVertex(ColoredTexturedVertex vertex)
        {
            Position = vertex.Position;
            Color = vertex.Color;
        }
    }
}
