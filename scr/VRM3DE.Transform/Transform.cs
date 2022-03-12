using SFML.System;

namespace VRM3DE.Transform
{
    public interface ITransform : IDataComponent
    {
        public Vector3f Position { get; set; }
        public Vector2f Rotation { get; set; }
    }

    public struct RayTransform : ITransform, IDataComponent<RayTransform>
    {
        public Vector3f Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        public Vector2f Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        private Vector3f _position;
        private Vector2f _rotation;
    }
}