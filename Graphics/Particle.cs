using GLWrapper.Graphics.Vertices;

namespace GLWrapper.Graphics
{
    public struct Particle
    {
        public ColoredVertex Vertex;
        public float Speed;
        public float Life;
        public Particle(ColoredVertex? vertex = null,float speed = 1.0f)
        {
            Vertex = vertex ?? new ColoredVertex();
            Speed = speed;
        }
        public static Particle WhiteAndCenter {get {return new Particle();}}
    }
}