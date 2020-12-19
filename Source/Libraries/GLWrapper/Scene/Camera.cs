using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing.Drawing2D;

namespace GLWrapper
{
    public class Camera
    {
        private float _speed = 1.5f;
        private float _pitch = 1.0f;
        private float _yaw = -MathHelper.PiOver2;
        private float _sensivity = 0.01f;
        private float _fov = MathHelper.PiOver2;
        //private Matrix4 _view = Matrix4.Identity;
        //TODO:Move LightPosition to another class, this don't makes sense here
        public Vector3 LightPosition { get; set; }
        public float AspectRatio { get; set; }
        public float Yaw { 
            get { return MathHelper.RadiansToDegrees(_yaw); } 
            set 
            { 
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            } 
        }
        public float Pitch { 
            get { return MathHelper.RadiansToDegrees(_pitch); }
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float FovValue 
        { get { return MathHelper.RadiansToDegrees(_fov); } 
          set 
            {
                var angle = MathHelper.Clamp(value, 1f, 89f);
                _fov = MathHelper.DegreesToRadians(angle);
            } 
        }               
        public Matrix4 Model { get; set; }
        public Matrix4 View { get { return Matrix4.LookAt(Position, Position + Front, Up); } }
        public Matrix4 Projection { get { return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), AspectRatio, 0.01f, 100f); } }
        public Vector3 Position { get; set; } = new Vector3(0.0f, 0.0f, 3.0f);
        public Vector2 LastPosition { get; set; } = new Vector2(1.0f, 1.0f);
        public Vector3 Target { get; set; } = Vector3.Zero;
        public Vector3 Direction { get; set; }
        public Vector3 Up { get; set; } = Vector3.UnitY;
        public Vector3 Right { get; set; }
        public Vector3 Front { get; set; } = new Vector3(0.0f, 0.0f, -1.0f);
        
        protected Camera(int width, int height)
        {
            Model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));            
            //Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / height, 0.1f, 100.0f);
            Direction = Vector3.Normalize(Position - Target);
            Right = Vector3.Normalize(Vector3.Cross(Up, Direction));
            Up = Vector3.Cross(Direction, Right);
            AspectRatio = width / (float)height;
        }        
        public void Rotate(MouseState mouse,float time)
        {
            float deltaX = mouse.X - LastPosition.X;
            float deltaY = mouse.Y - LastPosition.Y;
            LastPosition = new Vector2(mouse.X, mouse.Y);
            Yaw += deltaX * _sensivity;
            Pitch -= deltaY * _sensivity;
        }
        protected void UpdateVectors()
        {
            var front = Front;
            front.Y = (float)Math.Sin(Pitch);
            front.X = (float)Math.Cos(Pitch) * (float)Math.Cos(Yaw);
            front.Z = (float)Math.Cos(Pitch) * (float)Math.Sin(Yaw);
            Front = Vector3.Normalize(front);
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
        public void Update(KeyboardState input,float time = 1f)
        {
            if (input.IsKeyDown(Key.W))
            {
                Position += Front * _speed * time; //Forward 
            }

            if (input.IsKeyDown(Key.S))
            {
                Position -= Front * _speed * time; //Backwards
            }

            if (input.IsKeyDown(Key.A))
            {
                Position -= Right * _speed * time; //Left
            }

            if (input.IsKeyDown(Key.D))
            {
                Position += Right * _speed * time; //Right
            }

            if (input.IsKeyDown(Key.Space))
            {
                Position += Up * _speed * time; //Up 
            }

            if (input.IsKeyDown(Key.LShift))
            {
                Position -= Up * _speed * time; //Down
            }
            //LastPosition = new Vector2(Position.X,Position.Y);
        }
        public static Camera CreateCamera(int width,int height)
        {
            return new Camera(width, height);
        }
    }
}
