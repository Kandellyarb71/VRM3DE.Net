using System;
using SFML.System;

namespace VRM3DE.Scene
{
    public interface ISceneObject : IUpdateable
    {

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class GlobalObject : Attribute
    {

    }

    public interface IMainWindow
    {
        public bool IsFocused { get; set; }
        public Vector2i MouseHiddenPosition { get; }
        public Vector2u Size { get; }
    }
}