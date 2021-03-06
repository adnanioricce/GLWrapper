using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLWrapper.Graphics.Vertices
{
    public struct TexturedVertex
    {
        public const int Size = (3 + 3 + 2) * 4;
        public const int PositionStride = 3 * sizeof(float);        
        public const int NormalStride = 3 * sizeof(float);
        public const int TextureCoordinateStride = 2 * sizeof(float);
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public TexturedVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }

    }
}
