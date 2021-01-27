using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace GLWrapper
{
    /// <summary>
    /// Vertex struct with position, color and texture coordinates. has Size of 36 bytes
    /// </summary>
    public struct ColoredTexturedVertex
    {
        public const int Size = (3 + 4 + 2 + 3) * 4;
        public const int PositionStride = 3 * sizeof(float);
        public const int NormalCoordinateStride = 3 * sizeof(float);
        public const int ColorStride = 4 * sizeof(float);
        public const int TextureCoordinateStride = 2 * sizeof(float);        
        public Vector3 Position;
        public Color4 Color;
        public Vector2 TextureCoordinate;
        public ColoredTexturedVertex(Vector3 position, Color4 color, Vector2 textureCoordinate)
        {
            Position = position;
            Color = color;
            TextureCoordinate = textureCoordinate;            
        }        
    }    
}
